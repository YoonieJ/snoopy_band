// DrumInstrument.cs
// Drum instrument class, which creates a simple drum and animates it with a bounce. 
// It attaches to the dog's torso so it moves with the dog.
// The drum is represented as a single texture, and the animation is a simple vertical bounce using a sine wave.

using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace snoopy_band;

public class DrumInstrument : InstrumentBase
{
    private readonly SceneNode _drum;

    public DrumInstrument(Texture2D drumTex)
    {
        _drum = new SceneNode(drumTex);

        // keep it at (0,0) as the art should be designed to be positioned correctly relative to the torso
        _drum.LocalPosition = Vector2.Zero;

        // Set the origin to the center of the drum texture for proper rotation and scaling
        _drum.Origin = new Vector2(200, 250);

        _root.AddChild(_drum);

        _drum.RecomputeLocal();
        _root.RecomputeLocal();
    }

    // @Override to attach the drum to the dog's torso and position it appropriately
    protected override void AttachToAnchors()
    {
        _dog.TorsoNode.AddChild(_root);
        _root.LocalPosition = Vector2.Zero; // Ensure it starts at the torso's origin
    }

    // @Override to animate the drum with a simple bounce effect
    protected override void Animate(float t)
    {
        _root.LocalPosition = new Vector2(0, 3f * (float)Math.Sin(4.0f * t)); // Bounce relative to torso
        _root.RecomputeLocal();
    }
}