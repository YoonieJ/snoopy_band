# MONOGAME: Snoopy Band

This MonoGame project demonstrates a 2D scene-graph animation: a band of dogs made from hierarchical sprites (torso, head, arms, etc.) with attached instruments. Animations are driven by composed root matrices and local transforms to produce coordinated, looping movement.

<p align="center">
  <img src="snoopy_reunion.png" width="800">
</p>

Quick features
- Scene-graph (SceneNode) with local/world matrices, origins, layers.
- Multiple Dog types (StringDog, DrumDog) with per-part animation.
- Instruments attach to dog anchors and animate independently.
- Robust texture loading with a fallback texture for missing assets.
- Resolution: 1200×800 (100px margins used for layout).

How to run
Prerequisites
- .NET SDK compatible with MonoGame (or Visual Studio with MonoGame).
- MonoGame (installed per platform instructions).

Build and run
1. Open the solution file (group_12_assignment4.sln) in Visual Studio or your preferred .NET IDE.
2. Restore NuGet packages and ensure MonoGame references resolve.
3. Build the solution.
4. Run the project (F5 in Visual Studio or dotnet run configured for the game project).

If running from the command line, ensure the correct project is targeted and MonoGame is available:
- dotnet build
- dotnet run --project path/to/game/project

Controls / Interaction
- ESC (or gamepad Back) — exit the application.
- Current project is presentation-only; no additional interactive controls implemented.

Project structure (high-level)
- Game1.cs — setup, load assets, layout band members, main loop.
- SceneNode.cs — scene graph node with local/world matrices and drawing.
- Dog.cs — base dog class with hierarchical parts and composed root transform.
- StringDog.cs, DrumDog.cs — concrete dog behaviors.
- InstrumentBase.cs, StringInstrument.cs, DrumInstrument.cs — instruments attached to dogs.
- BandMember.cs — binds a Dog and an Instrument and updates/draws them.

Description:
- 2D animation in Game class (MonoGame): Game1 builds and updates the scene.
- Multiple animated objects: several Dog instances animated with different tempos, amplitudes and phases.
- Class design with hierarchy: Dog class has at least two levels (root matrix + local part transforms). SceneNode uses matrices to apply transforms hierarchically.
- Multiple instances: multiple dogs reuse Dog-derived classes with different parameters (position, scale, tempo, phase).
- Root-level matrix composition: root transform composes scale, rotation and translation (see Dog.UpdateRootTransform).
- Animations run continuously and are time-based (frame-rate independent).

Assets and fallbacks
- Expected sprite naming (Content): {prefix}_torso, {prefix}_head, {prefix}_armL, {prefix}_armR, {prefix}_cheekL, {prefix}_cheekR, {prefix}_stringInst, {prefix}_drum, {prefix}_jug.
- Missing textures are replaced with a 1×1 white fallback so the app still runs.

Notes and known issues
- Olaf (Jug dog) is scaffolded but not added/implemented in the current layout. (I will add him in the future.)
- Tuning of origins and anchor positions may be required if sprite sizes differ from 400×500.
- No extra input controls implemented beyond exit.

License
- All images are created by Yoon Jang
