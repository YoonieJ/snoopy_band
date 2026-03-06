// Creates a simple string instrument for StringDog and animates it with a gentle wobble to simulate strumming.
// The instrument is attached to the dog's torso so it moves with the dog.
// The animation is a small rotation back and forth to give the impression of strumming,
// and the texture is positioned to align with the dog's arms for a cohesive look.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace snoopy_band;

public class StringInstrument : InstrumentBase
{
    // For simplicity, we use a single texture for the string instrument.
    // Can expand this to multiple parts in the future.
    private readonly SceneNode _stringInst;

    public StringInstrument(Texture2D stringInstTex)
    {
        _stringInst = new SceneNode(stringInstTex);

        _root.Layer = 3f;
        _stringInst.Layer = 3f; // same layer as root so it doesn't get sorted behind the dog parts

        // keep at (0,0)
        _stringInst.LocalPosition = Vector2.Zero;

        // Pivot for wobble
        // tune this to align with the dog's arms (probably around the torso origin)
        _stringInst.Origin = new Vector2(200, 250);

        _root.AddChild(_stringInst); // add to root so it moves with the dog

        // Recompute local matrices after setting up the hierarchy and transforms
        _stringInst.RecomputeLocal();
        _root.RecomputeLocal();
    }

    // Attach the instrument to the dog's hierarchy.
    protected override void AttachToAnchors()
    {
        // Parent instrument to torso so it inherits dog root motion
        _dog.TorsoNode.AddChild(_root);

        // Set the local position and rotation to align it properly with the dog's arms.

        _root.LocalPosition = Vector2.Zero;
        _root.LocalRotation = 0f;
        _root.LocalScale = Vector2.One;

        _root.RecomputeLocal();
    }

    protected override void Animate(float t)
    {
        // Instrument rotation back and forth to simulate strumming
        _stringInst.LocalRotation = 0.05f * (float)Math.Sin(2.6f * t);
        _stringInst.RecomputeLocal();
    }
}