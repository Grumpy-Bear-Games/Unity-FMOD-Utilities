---
uid: manual.fmodevents.md
---

# Playing FMOD one-shot events


- @"Games.GrumpyBear.FMOD.Utilities.FMODEvent" as a @"UnityEngine.ScriptableObject", which references a
  single [FMOD event](https://fmod.com/resources/documentation-studio?version=2.01&page=glossary.html#event),
  and allow simple one-shot playback. These instances are very convenient to reference from e.g. @"UnityEngine.Events.UnityEvent"s,
  and can make adding simple sound effects and UI sounds very easy without having to add
  [`StudioEventEmitter`](https://fmod.com/resources/documentation-unity?version=2.01&page=api-studioeventemitter.html)
  components everywhere.
