// StringDog.cs
// 6 Dogs: Snoopy, Marbles, Belle, Molly, Rover, and Spike
// Snoopy: Guitar: default string dog behavior
// Marbles: Banjo: hand movement range smaller, a little faster than default 
// Belle: Fiddle (1) : Hand position differs from other string Isnt and different speed from default
// Molly: Mandolin : hand movement range smaller, a little faster than default
// Rover: Steel Guitar : hand movement range larger, a little faster than default
// Spike: Fiddle (2): Hand position differs from other string Isnt and different speed from both default and Belle

using Microsoft.Xna.Framework;
using System;

namespace snoopy_band;

public class StringDog : Dog
{
    public StringDog(DogSprites sprites, Vector2 basePos, float baseScale, float tempo, float phase)
        : base(sprites, basePos, baseScale, tempo, phase)
    {
    }

    protected override void AnimateParts(float t)
    {
        // Gentle sway is handled by root.
        // one-hand movement + small head + tiny other hand
        var headAmp = 0.16f;
        var strumAmp = 0.18f;
        var supportAmp = 0.18f;

        _head.LocalRotation = headAmp * (float)Math.Sin(1.6f * t);

        // "One hand movement": left arm strums more
        _armL.LocalRotation = strumAmp * (float)Math.Sin(3.2f * t);

        // Right arm smaller counter-motion
        _armR.LocalRotation = -supportAmp * (float)Math.Sin(3.2f * t + 0.8f);

        // cheeks default off / neutral
        _cheekL.LocalScale = Vector2.One;
        _cheekR.LocalScale = Vector2.One;
        _cheekL.Tint = Color.White;
        _cheekR.Tint = Color.White;
    }
}