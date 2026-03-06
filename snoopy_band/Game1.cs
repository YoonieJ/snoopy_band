using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace snoopy_band;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private DogSprites _snoopySprites;
    private DogSprites _marblesSprites;
    private DogSprites _belleSprites;
    private DogSprites _mollySprites;
    private DogSprites _roverSprites;
    private DogSprites _spikeSprites;
    private DogSprites _andySprites;
    
    // TODO: Olaf not currently used, but will be our Jug Dog with cheeks and jug instrument. Will likely need some custom animation for cheeks + jug interaction.
    // private DogSprites _olafSprites;

    // Band members (dog + instrument)
    private List<BandMember> _members;

    // Background texture for the stage
    private Texture2D _background;

    // Background music
    private Song backgroundMusic;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);

        _graphics.PreferredBackBufferWidth = 1200; // Set width
        _graphics.PreferredBackBufferHeight = 800; // Set height

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _members = new List<BandMember>();

        base.Initialize();
    }

    // Helper to add a string dog with its instrument to the band.
    private void AddStringDog(string name, DogSprites sprites, Vector2 pos, float scale, float tempo, float phase,
        float depth)
    {
        var dog = new StringDog(sprites, pos, scale, tempo, phase);
        var inst = new StringInstrument(sprites.StringInst); // ok if null (you just won't see it)
        _members.Add(new BandMember(dog, inst, name) { Depth = depth });
    }

    // Helper to add a drum dog with its instrument to the band.    
    private void AddDrumDog(string name, DogSprites sprites, Vector2 pos, float scale, float tempo, float phase,
        float depth)
    {
        var dog = new DrumDog(sprites, pos, scale, tempo, phase);
        var inst = new DrumInstrument(sprites.Drum);

        _members.Add(new BandMember(dog, inst, name) { Depth = depth });
    }

    // // Helper to add a Jug dog with its instrument to the band.    
    // private void AddJugDog(string name, DogSprites sprites, Vector2 pos, float scale, float tempo, float phase, float depth)
    // {
    //     var dog = new JugDog(sprites, pos, scale, tempo, phase);
    //     var inst = new JugInstrument(sprites.Jug);

    //     _members.Add(new BandMember(dog, inst, name) { Depth = depth });
    // }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Load background texture
        _background = Content.Load<Texture2D>("stage_background");

        // Each dog uses separate sprites for torso/head/arms/cheeks (cheeks only for Jug dog AKA Olaf).
        _snoopySprites = LoadDogSprites("snoopy");
        _belleSprites = LoadDogSprites("belle");
        _marblesSprites = LoadDogSprites("marbles");
        _mollySprites = LoadDogSprites("molly");
        _roverSprites = LoadDogSprites("rover");
        _spikeSprites = LoadDogSprites("spike");
        _andySprites = LoadDogSprites("andy");
        // _olafSprites = LoadDogSprites("olaf"); // not added yet

        _members.Clear(); // just in case LoadContent gets called more than once during development/testing

        // Screen: 1200x800, 100px margin from each side = usable 1000x600
        // Dogs: 400x500 sprites

        // FRONT ROW: Rover, Snoopy, Molly (left to right) at 1.3x scale
        var frontScale = 0.52f * 1.3f;
        var frontY = 450f; // margin: 800 - 350 = 450
        var frontDepth = 2f;

        AddStringDog("Rover", _roverSprites, new Vector2(300, frontY), frontScale, 2.20f, 2.0f, frontDepth);
        AddStringDog("Snoopy", _snoopySprites, new Vector2(600, frontY), frontScale, 3.00f, 0.0f, frontDepth);
        AddStringDog("Molly", _mollySprites, new Vector2(900, frontY), frontScale, 3.15f, 2.4f, frontDepth);

        // MID ROW: Marbles, Spike (left to right) at 1.1x scale
        var midScale = 0.52f * 1.1f;
        var midY = 420f;
        var midDepth = 1f;

        AddStringDog("Marbles", _marblesSprites, new Vector2(420, midY), midScale, 4.00f, 0.4f, midDepth);
        AddStringDog("Spike", _spikeSprites, new Vector2(730, midY), midScale, 2.52f, 1.6f, midDepth);

        // BACK ROW: Belle, Olaf, Andy (left to right)
        var belleScale = 0.52f;
        var olafScale = 0.52f;
        var andyScale = 0.52f * 1.3f;
        var backY = 350f;
        var backDepth = 0f;

        AddStringDog("Belle", _belleSprites, new Vector2(500, backY), belleScale, 2.50f, 0.8f, backDepth);
        // AddJugDog("Olaf", _olafSprites, new Vector2(600, backY), olafScale, 0.95f, 1.2f, backDepth);
        AddDrumDog("Andy", _andySprites, new Vector2(800, backY), andyScale, 3.10f, 0.2f, backDepth);

        // Attach instruments after dogs are constructed.
        foreach (var m in _members)
            m.AttachInstrumentToDog();

        // Load and play background music
        backgroundMusic = Content.Load<Song>("inst_music");

        // Loop the music and set volume
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.5f; // optional
        MediaPlayer.Play(backgroundMusic);
    }

    private DogSprites LoadDogSprites(string dogPrefix)
    {
        // Convention:
        // {prefix}_torso, {prefix}_head, {prefix}_armL, {prefix}_armR, {prefix}_cheekL, {prefix}_cheekR
        // Cheeks will be missing for all dogs besides Jug Dog (catch and set null).
        Texture2D TryLoad(string asset)
        {
            try
            {
                return Content.Load<Texture2D>(asset);
            }
            catch
            {
                Console.WriteLine($"Warning: Texture {asset} not found. Using fallback texture.");
                return CreateFallbackTexture();
            }
        }

        return new DogSprites
        {
            Torso = TryLoad($"{dogPrefix}_torso"),
            Head = TryLoad($"{dogPrefix}_head"),
            ArmL = TryLoad($"{dogPrefix}_armL"),
            ArmR = TryLoad($"{dogPrefix}_armR"),

            CheekL = TryLoad($"{dogPrefix}_cheekL"),
            CheekR = TryLoad($"{dogPrefix}_cheekR"),

            StringInst = TryLoad($"{dogPrefix}_stringInst"),
            Drum = TryLoad($"{dogPrefix}_drum"),
            Jug = TryLoad($"{dogPrefix}_jug")
        };
    }

    // Creates a simple 1x1 white texture to use as a fallback when a specific texture fails to load. 
    // This prevents crashes due to missing assets and allows the program to continue running, albeit with a placeholder visual.
    private Texture2D CreateFallbackTexture()
    {
        // Create a 1x1 white texture as a fallback
        var fallback = new Texture2D(GraphicsDevice, 1, 1);
        fallback.SetData(new[] { Color.White });
        return fallback;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        foreach (var m in _members)
            m.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.SandyBrown);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(
            _background,
            new Rectangle(0, 0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height),
            Color.White);

        // Draw members in order of depth (back to front) to ensure proper layering.
        foreach (var m in _members.OrderBy(m => m.Depth))
            m.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}