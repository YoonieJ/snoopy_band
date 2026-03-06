// DrumDog.cs (Andy)
// Defines the DrumDog, which has a strong up/down bob and both arms active in a drumming motion.
// The head also has a slight sway. The cheeks are neutral (not animated).
// The root transform is composed of a scale, rotation, and translation to achieve the desired movement.

using Microsoft.Xna.Framework;
using System;

namespace snoopy_band;

public class DrumDog : Dog
{
    public DrumDog(DogSprites sprites, Vector2 basePos, float baseScale, float tempo, float phase)
        : base(sprites, basePos, baseScale, tempo, phase)
    {
    }

    // @Override the root transform to create a stronger up/down bob and a slight sway for the head. 
    protected override void UpdateRootTransform(float t)
    {
        // Stronger up/down bob (composed matrix)
        var sway = 0.06f * (float)Math.Sin(t * 0.7f);
        var bob = 18.0f * (float)Math.Abs(Math.Sin(2.4f * t)); // punchy bounce

        var pos = _basePos + new Vector2(0, -bob);

        _rootWorld =
            Matrix.CreateScale(_baseScale, _baseScale, 1f) *
            Matrix.CreateRotationZ(sway) *
            Matrix.CreateTranslation(pos.X, pos.Y, 0f);
    }

    // @Override to animate both arms in a drumming motion, with a slight head sway.
    protected override void AnimateParts(float t)
    {
        // Both arms active, with slightly different movements
        var leftArmAmp = 0.35f; // Smaller sway for the left arm
        var rightArmAmp = 0.45f; // Larger sway for the right arm

        _head.LocalRotation = 0.10f * (float)Math.Sin(1.2f * t);

        _armL.LocalRotation = leftArmAmp * (float)Math.Sin(4.0f * t);
        _armR.LocalRotation = rightArmAmp * (float)Math.Sin(4.0f * t + (float)Math.PI);

        // cheeks neutral
        _cheekL.LocalScale = Vector2.One;
        _cheekR.LocalScale = Vector2.One;
    }
}