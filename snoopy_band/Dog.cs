// Dog.cs (base class for all dogs)
// Each specific dog type (GuitarDog, DrumDog, JugDog) will inherit from this class and implement its own AnimateParts method to define how the arms and other parts move.
// Sprite size: 400x500

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace snoopy_band;

public abstract class Dog
{
    // Base parameters for all dogs (position, scale, tempo, phase)
    protected Vector2 _basePos;
    protected float _baseScale;
    protected float _tempo;
    protected float _phase;

    protected float _time; // global time accumulator for animation

    // Root uses a Matrix composed of 2+ transforms
    protected Matrix _rootWorld = Matrix.Identity;

    // Scene graph nodes for body parts
    protected SceneNode _torso; // always the root of the dog hierarchy
    protected SceneNode _head;
    protected SceneNode _armL;
    protected SceneNode _armR;

    protected SceneNode _handLAnchor; // no texture, used for instrument attachment
    protected SceneNode _handRAnchor; // no texture, used for instrument attachment

    protected SceneNode _cheekL; // optional
    protected SceneNode _cheekR; // optional

    // Public accessors for instrument attachment
    public SceneNode HandLAnchor => _handLAnchor;

    // public SceneNode HandLAnchor
    // {
    //     get { return _handLAnchor; }
    // }

    public SceneNode HandRAnchor => _handRAnchor;
    public SceneNode TorsoNode => _torso;

    protected Dog(DogSprites sprites, Vector2 basePos, float baseScale, float tempo, float phase)
    {
        _basePos = basePos;
        _baseScale = baseScale;
        _tempo = tempo;
        _phase = phase;

        BuildHierarchy(sprites);
    }

    // Build the scene graph hierarchy for the dog based on the provided sprites.
    // Sets up the nodes, their textures, local positions, origins, and parent-child relationships.
    private void BuildHierarchy(DogSprites s)
    {
        // Nodes
        _torso = new SceneNode(s.Torso);
        _head = new SceneNode(s.Head);
        _armL = new SceneNode(s.ArmL);
        _armR = new SceneNode(s.ArmR);

        _handLAnchor = new SceneNode(null);
        _handRAnchor = new SceneNode(null);

        // Optional cheeks (safe if null)
        _cheekL = new SceneNode(s.CheekL);
        _cheekR = new SceneNode(s.CheekR);

        _torso.Layer = 0f; // torso is base
        _head.Layer = 1f; // above torso
        _armR.Layer = 2f; // right arm
        _armL.Layer = 4f; // left arm in front

        // // Debug output to verify that textures are loaded correctly.
        // Console.WriteLine($"Torso texture: {(s.Torso != null ? "Loaded" : "Missing")}");
        // Console.WriteLine($"Head texture: {(s.Head != null ? "Loaded" : "Missing")}");
        // Console.WriteLine($"Left arm texture: {(s.ArmL != null ? "Loaded" : "Missing")}");
        // Console.WriteLine($"Right arm texture: {(s.ArmR != null ? "Loaded" : "Missing")}");
        // Console.WriteLine($"Left cheek texture: {(s.CheekL != null ? "Loaded" : "Missing")}");
        // Console.WriteLine($"Right cheek texture: {(s.CheekR != null ? "Loaded" : "Missing")}");

        // Default local layout for proper allignment (adjusted based on actual sprite sizes and desired anchor points)
        _torso.LocalPosition = Vector2.Zero;
        _head.LocalPosition = Vector2.Zero;
        _armL.LocalPosition = Vector2.Zero;
        _armR.LocalPosition = Vector2.Zero;

        // Anchors at "hand" area on arms (relative to arm sprites)
        _handLAnchor.LocalPosition = new Vector2(180, 380);
        _handRAnchor.LocalPosition = new Vector2(180, 380);

        // Cheeks are children of head (relative to head)
        _cheekL.LocalPosition = new Vector2(-70, 40);
        _cheekR.LocalPosition = new Vector2(70, 40);

        // Set pivots (centered for rotation)
        _torso.Origin = new Vector2(200, 250); // Center of torso
        _head.Origin = new Vector2(200, 250); // Center of head
        _armL.Origin = new Vector2(190, 250); // Center of left arm (adjusted for better hand pivot)
        _armR.Origin = new Vector2(210, 250); // Center of right arm (adjusted for better hand pivot)
        if (_cheekL.Texture != null)
            _cheekL.Origin = new Vector2(_cheekL.Texture.Width / 2f, _cheekL.Texture.Height / 2f);
        if (_cheekR.Texture != null)
            _cheekR.Origin = new Vector2(_cheekR.Texture.Width / 2f, _cheekR.Texture.Height / 2f);

        // Build hierarchy
        _torso.AddChild(_head);
        _torso.AddChild(_armL);
        _torso.AddChild(_armR);
        _armL.AddChild(_handLAnchor);
        _armR.AddChild(_handRAnchor);

        // Cheeks are children of head (optional)
        _head.AddChild(_cheekL);
        _head.AddChild(_cheekR);
    }

    // Adjust hand layering if needed
    // public void SetHandOrder(bool leftOnTop)
    // {
    //     if (leftOnTop)
    //     {
    //         _armR.Layer = 2f;
    //         _armL.Layer = 4f;
    //     }
    //     else
    //     {
    //         _armL.Layer = 2f;
    //         _armR.Layer = 4f;
    //     }
    // }

    public void Update(GameTime gt)
    {
        var dt = (float)gt.ElapsedGameTime.TotalSeconds; // convert to seconds
        _time += dt * _tempo; // apply tempo to time progression

        var t = _time + _phase; // apply phase offset

        UpdateRootTransform(t); // Matrix composition
        AnimateParts(t); // subclass-specific local animation
        RecomputeMatrices(); // push transformations down the hierarchy
    }

    protected virtual void UpdateRootTransform(float t)
    {
        // Default gentle sway + bob.
        // RootWorld = Scale * Rotation * Translation
        var sway = 0.10f * (float)Math.Sin(t);
        var bob = 8.0f * (float)Math.Sin(2.0f * t);

        var pos = _basePos + new Vector2(0, bob);

        // Compose the root world matrix with scale, rotation, and translation.
        _rootWorld =
            Matrix.CreateScale(_baseScale, _baseScale, 1f) *
            Matrix.CreateRotationZ(sway) *
            Matrix.CreateTranslation(pos.X, pos.Y, 0f);
    }

    // Inheritance hook: each role uses a different part animation style
    protected abstract void AnimateParts(float t);

    private void RecomputeMatrices()
    {
        // Recompute local matrices
        _torso.RecomputeLocal();
        _head.RecomputeLocal();
        _armL.RecomputeLocal();
        _armR.RecomputeLocal();

        _handLAnchor.RecomputeLocal();
        _handRAnchor.RecomputeLocal();

        _cheekL.RecomputeLocal();
        _cheekR.RecomputeLocal();

        // Propagate world from root
        _torso.RecomputeWorld(_rootWorld);
    }

    public virtual void Draw(SpriteBatch sb)
    {
        _torso.Draw(sb);
    }
}