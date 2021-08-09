using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    /// <summary>
    /// Focus manager, which suspends and resumes FMOD on unfocus and focus. Simply add this
    /// class to a GameObject in your scene. 
    /// This is particularly useful for WebGL.
    /// </summary>
    [AddComponentMenu("Grumpy Bear Games/FMOD Utilities/FMOD Focus Manager")]
    public class FocusManager : MonoBehaviour
    {
        [Tooltip("Scope of this manager")]
        [SerializeField] private Scope _scope = Scope.Scene;

        private void Awake()
        {
            if (_scope == Scope.Global) DontDestroyOnLoad(gameObject);
        }

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

        private enum Scope
        {
            Scene,
            Global
        }
    }
}
