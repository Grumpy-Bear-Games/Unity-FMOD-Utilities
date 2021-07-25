// ReSharper disable MemberCanBePrivate.Global
using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utility
{
    [CreateAssetMenu(fileName = "FMOD Event", menuName = "Grumpy Bear Games/FMOD Utility/FMOD Event")]
    public class FMODEvent : ScriptableObject
    {
        [SerializeField][FMODUnity.EventRef] internal string _fmodEvent;
        
        public void PlayOneShot() => FMODUnity.RuntimeManager.PlayOneShot(_fmodEvent);
        public void PlayOneShot(Vector3 origin) => FMODUnity.RuntimeManager.PlayOneShot(_fmodEvent, origin);
        public void PlayOneShot(MonoBehaviour origin) => PlayOneShot(origin.transform.position);
        public void PlayOneShot(GameObject origin) => PlayOneShot(origin.transform.position);
    }
}
