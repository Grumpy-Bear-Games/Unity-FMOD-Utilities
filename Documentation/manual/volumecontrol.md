---
uid: manual.volumecontrol.md
---

# Persistent Volume Control
A common task for any game is to allow the player can adjust audio volume from 
a settings menu and persist this setting between play sessions.

This package contain several components for making this easy.

- @"Games.GrumpyBear.FMOD.Utilities.VCAVolumePreference" and @"Games.GrumpyBear.FMOD.Utilities.BusVolumePreference"
  are `ScriptableObject`s for managing FMOD [VCAs](https://fmod.com/resources/documentation-studio?version=2.01&page=mixing.html#vcas) an
  [buses](https://fmod.com/resources/documentation-studio?version=2.01&page=glossary.html#bus) respectively.
  Each instance manages a single [VCA](https://fmod.com/resources/documentation-studio?version=2.01&page=mixing.html#vcas) or
  [bus](https://fmod.com/resources/documentation-studio?version=2.01&page=glossary.html#bus), and persists the current volume to PlayerPrefs.
- @"Games.GrumpyBear.FMOD.Utilities.VolumePreferenceSlider" is a component, which binds a  @"Games.GrumpyBear.FMOD.Utilities.VCAVolumePreference"
  or @"Games.GrumpyBear.FMOD.Utilities.BusVolumePreference" to a `UnityEngine.UI.Slider` for building settings menus.

See the sample for how to build a simple audio settings menu.
