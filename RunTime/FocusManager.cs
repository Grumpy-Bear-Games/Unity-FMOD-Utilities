using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    [AddComponentMenu("Grumpy Bear Games/FMOD Utilities/Focus Manager")]
    public class FocusManager : MonoBehaviour
    {
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
                FMODUnity.RuntimeManager.CoreSystem.mixerResume();
            else
                FMODUnity.RuntimeManager.CoreSystem.mixerSuspend();
        }

        #if UNITY_EDITOR
        [UnityEditor.MenuItem("GameObject/Grumpy Bear Games/FMOD Utilities/Focus Manager", false, 10)]
        private static void CreateGameObject()
        {
            var go = new GameObject("[FMOD Focus Manager]", typeof(FocusManager));
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Create {go.name}");
            UnityEditor.Selection.activeObject = go;
        }
        #endif
    }
}
