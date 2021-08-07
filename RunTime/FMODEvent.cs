// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
using UnityEngine;

namespace Games.GrumpyBear.FMOD.Utilities
{
    /// <summary>
    /// A <c>ScriptableObject</c> managing an FMOD event. Using this class it's possible to hook up
    /// one-shot events directly in the editor, e.g. in a <c>UnityEvent</c>, and across scenes.
    /// </summary>
    [CreateAssetMenu(fileName = "FMOD Event", menuName = "Grumpy Bear Games/FMOD Utilities/FMOD Event")]
    public class FMODEvent : ScriptableObject
    {
        [SerializeField][FMODUnity.EventRef] internal string _fmodEvent;

        /// <summary>
        /// Play the FMOD event from position (0, 0, 0)
        /// </summary>
        public void PlayOneShot() => FMODUnity.RuntimeManager.PlayOneShot(_fmodEvent);
        
        /// <summary>
        /// Play the FMOD event from specific position
        /// </summary>
        /// <param name="origin">Origin of the sounds</param>
        public void PlayOneShot(Vector3 origin) => FMODUnity.RuntimeManager.PlayOneShot(_fmodEvent, origin);
        
        /// <summary>
        /// Play the FMOD event from a MonoBehavior's position
        /// </summary>
        /// <param name="origin">Origin of the sounds</param>
        public void PlayOneShot(MonoBehaviour origin) => PlayOneShot(origin.transform.position);
        
        /// <summary>
        /// Play the FMOD event from a GameObject's position
        /// </summary>
        /// <param name="origin">Origin of the sounds</param>
        public void PlayOneShot(GameObject origin) => PlayOneShot(origin.transform.position);
    }
}
