// JugInstrument.cs (placeholder skeleton)

using Microsoft.Xna.Framework;
using System;

namespace snoopy_band;

public class JugInstrument : InstrumentBase
{
    private SceneNode _jugBody;

    public JugInstrument() : base()
    {
        _jugBody = new SceneNode(null); // TODO: assign texture later
        _root.AddChild(_jugBody);
        _jugBody.LocalPosition = Vector2.Zero;
    }

    protected override void AttachToAnchors()
    {
        // Attach between hands (parent to torso and position near hands)
        _dog.TorsoNode.AddChild(_root);
        _root.LocalPosition = new Vector2(280, 330); // TODO tune
    }

    protected override void Animate(float t)
    {
        _root.LocalRotation = 0.05f * (float)Math.Sin(2.2f * t);
        _root.RecomputeLocal();
    }
}