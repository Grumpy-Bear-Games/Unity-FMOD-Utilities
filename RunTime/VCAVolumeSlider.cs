using UnityEngine;
using UnityEngine.UI;

namespace Games.GrumpyBear.FMOD.Utility
{
    [RequireComponent(typeof(Slider))]
    public class VCAVolumeSlider : MonoBehaviour
    {
        [SerializeField] private VCAVolume _vcaVolume;
        private Slider _slider;

        private void Awake() => _slider = GetComponent<Slider>();

        private void OnEnable()
        {
            _slider.SetValueWithoutNotify(_vcaVolume.Volume);
            _slider.onValueChanged.AddListener(OnValueChange);
        }

        private void OnDisable() => _slider.onValueChanged.RemoveListener(OnValueChange);

        private void OnValueChange(float volume) => _vcaVolume.Volume = volume;
    }
}
