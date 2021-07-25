using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    public class FocusManager : MonoBehaviour
    {
        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus)
                FMODUnity.RuntimeManager.CoreSystem.mixerResume();
            else
                FMODUnity.RuntimeManager.CoreSystem.mixerSuspend();
        }
    }
}
