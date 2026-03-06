// JugDog.cs (Olaf)
// TODO: Implement a dog with a different animation style. For example, Olaf's cheeks puff up and his face tints slightly in time with the music. 
// Can also add more parts to the hierarchy (e.g. tail, ears) if you want, but it's not required.
// blow out air effect?

using Microsoft.Xna.Framework;
using System;

namespace snoopy_band;

public class JugDog : Dog
{
    public JugDog(DogSprites sprites, Vector2 basePos, float baseScale, float tempo, float phase)
        : base(sprites, basePos, baseScale, tempo, phase)
    {
    }

    protected override void AnimateParts(float t)
    {
        // Cheek scale + face tint pulse (color change)
        _head.LocalRotation = 0.08f * (float)Math.Sin(1.0f * t);

        _armL.LocalRotation = 0.12f * (float)Math.Sin(2.0f * t + 0.3f);
        _armR.LocalRotation = -0.12f * (float)Math.Sin(2.0f * t + 0.3f);

        var puff = 1.0f + 0.18f * (float)Math.Sin(2.2f * t); // cheeks inflate/deflate
        _cheekL.LocalScale = new Vector2(puff, puff);
        _cheekR.LocalScale = new Vector2(puff, puff);

        // Face color change (simple sinusoidal tint)
        var pulse01 = 0.5f + 0.5f * (float)Math.Sin(2.2f * t);
        var r = (byte)MathHelper.Lerp(255, 255, pulse01);
        var g = (byte)MathHelper.Lerp(255, 180, pulse01);
        var b = (byte)MathHelper.Lerp(255, 180, pulse01);

        var faceTint = new Color(r, g, b);
        _head.Tint = faceTint;

        // Cheeks follow face tint (optional)
        _cheekL.Tint = faceTint;
        _cheekR.Tint = faceTint;
    }
}