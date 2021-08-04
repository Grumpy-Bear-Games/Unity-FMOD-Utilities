using UnityEngine;
using UnityEngine.UI;

namespace Games.GrumpyBear.FMOD.Utilities
{
    [AddComponentMenu("Grumpy Bear Games/FMOD Utilities/Volume Preference Slider")]
    [RequireComponent(typeof(Slider))]
    public class VolumePreferenceSlider : MonoBehaviour
    {
        [SerializeField] private VolumePreference _volumePreference;
        private Slider _slider;

        private void Awake() => _slider = GetComponent<Slider>();

        private void OnEnable()
        {
            _slider.SetValueWithoutNotify(_volumePreference.Volume);
            _slider.onValueChanged.AddListener(OnValueChange);
        }

        private void OnDisable() => _slider.onValueChanged.RemoveListener(OnValueChange);

        private void OnValueChange(float volume) => _volumePreference.Volume = volume;
    }
}
