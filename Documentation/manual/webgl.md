---
uid: manual.webgl.md
---

# Using FMOD with WebGL

Using FMOD with WebGL has some special requirements and limitations.

1. FMOD must be initialized through an action, which comes from direct user input, e.g. the user 
   clicking a button.
2. All FMOD banks must be loaded before interacting with them.
3. FMOD should be suspended and resumed on focus events to prevent audio glitches.

See more details in [Fix blocked FMOD audio in Browsers](https://alessandrofama.com/tutorials/fmod-unity/fix-blocked-audio-browsers/) and
[Scripting Examples | Asynchronous Loading]("https://fmod.com/resources/documentation-unity?version=2.01&page=examples-async-loading.html)

This package provides two classes for helping with implementing this right.

## @"Games.GrumpyBear.FMOD.Utilities.WebGLInitializer"

@"Games.GrumpyBear.FMOD.Utilities.WebGLInitializer" handles initialization of FMOD and bank loading.

Usage:
1. Set `InitializeOnEnable` to `false` on all @"Games.GrumpyBear.FMOD.Utilities.VolumePreference" instances in the editor. 
2. Create an empty scene with a camera, a canvas and a button. Give the button a label like "Start Game" or similar.
3. Create a new GameObject with @"Games.GrumpyBear.FMOD.Utilities.WebGLInitializer".
4. In the editor, configure @"Games.GrumpyBear.FMOD.Utilities.WebGLInitializer". This is easiest done by clicking the
   "Fix All Problems" button.
6. (Optionally) Style and decorate the scene any way you want.
7. Save this scene and make it the first scene in the [build settings](https://docs.unity3d.com/Manual/BuildSettings.html).

When started, @"Games.GrumpyBear.FMOD.Utilities.WebGLInitializer" will load the first scene asynchronously and wait for
the FMOD banks to load. The button is kept hidden until both scene and FMOD banks are ready. When the player clicks the button,
FMOD is initialized (thus fulfilling the requirement of direct under interaction), and switches to the real first scene.

> [!IMPORTANT]
> Make sure to configure FMOD banks to load automatically under `Initialization -> Load Banks` in the FMOD Settings (`FMOD -> Edit Settings`)  

## @"Games.GrumpyBear.FMOD.Utilities.FocusManager"

When a Unity application loses focus in WebGL, e.g. the user switches to a different tab, FMOD gets stuck playing the same sounds.
@"Games.GrumpyBear.FMOD.Utilities.FocusManager" is class, which helps handle unfocus and focus events correctly by suspending and
resuming FMOD. Simply add @"Games.GrumpyBear.FMOD.Utilities.FocusManager" to a @"UnityEngine.GameObject" in your active scene.

> [!TIP]
> It's recommended to keep the @"Games.GrumpyBear.FMOD.Utilities.FocusManager" in a global scene, if you're using a multi-scene setup.
