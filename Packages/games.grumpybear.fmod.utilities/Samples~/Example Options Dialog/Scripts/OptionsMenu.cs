using Games.GrumpyBear.FMOD.Utilities.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Games.GrumpyBear.FMOD.Utilities.Examples {
    [RequireComponent(typeof(UIDocument))]
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private BusVolumePreference _busVolumePreference;
        [SerializeField] private VCAVolumePreference _vcaVolumePreference;
        
        [SerializeField] private FMODEvent _sfxTestSound;
        
        private void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            root.Q<VolumeSlider>("BusVolume").VolumePreference = _busVolumePreference;
            root.Q<VolumeSlider>("VCAVolume").VolumePreference = _vcaVolumePreference;

            root.Q<Button>().clicked += () => _sfxTestSound.PlayOneShot();
        }
    }
}
