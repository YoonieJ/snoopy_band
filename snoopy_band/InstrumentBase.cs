// InstrumentBase.cs
// Base class for all instruments, which defines the common logic for attaching to a dog and animating.
// Each specific instrument type (DrumInstrument, GuitarInstrument, JugInstrument) will inherit from this class
// and implement its own AttachToAnchors and Animate methods to define how it attaches to the dog and how it animates over time.
// The base class manages the scene graph for the instrument and provides a common interface for updating and drawing the instrument as part of the dog's hierarchy.

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace snoopy_band;

public abstract class InstrumentBase
{
    protected Dog _dog;

    // Instrument has its own mini-hierarchy
    protected SceneNode _root; // parented under a dog anchor
    protected float _time;

    protected InstrumentBase()
    {
        _root = new SceneNode(null); // root node usually has no texture
    }

    public void AttachToDog(Dog dog)
    {
        _dog = dog;

        // Attach the instrument behind the left arm
        _dog.TorsoNode.InsertChild(0, _root); // Insert at the beginning of the torso's children
        AttachToAnchors();
        Console.WriteLine($"Instrument attached to dog: {_dog}");
    }

    public void Update(GameTime gt)
    {
        var dt = (float)gt.ElapsedGameTime.TotalSeconds;
        _time += dt;
        Animate(_time);
    }

    public void Draw(SpriteBatch sb)
    {
        _root.Draw(sb);
    }

    // Choose where to parent in the dog hierarchy
    protected abstract void AttachToAnchors();

    // Animate instrument parts locally
    protected abstract void Animate(float t);
}