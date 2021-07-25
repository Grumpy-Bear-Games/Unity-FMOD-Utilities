using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utility
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
