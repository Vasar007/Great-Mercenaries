#define LOG

using UnityEngine;
using UnityEngine.UI;
using GreatMercenaries.Assets.Scripts.Core.Events;

namespace GreatMercenaries.Assets.Scripts.UI
{
    public class SliderBinding : MonoBehaviour
    {
        public string valueMessage;
        public bool broadcasts = true;

        private Slider _slider;


        private void Awake()
        {
            _slider = GetComponent<Slider>();
            if (_slider == null) return;

            if (string.IsNullOrEmpty(valueMessage)) return;

            Messenger.AddListener<MonoBehaviour, float>(valueMessage, HandleSliderChangedMessage);

            if (broadcasts)
            {
                _slider.onValueChanged.AddListener(onValueChanged);
            }
        }

        private void OnDestroy()
        {
            if (string.IsNullOrEmpty(valueMessage)) return;

            Messenger.RemoveListener<MonoBehaviour, float>(valueMessage, HandleSliderChangedMessage);

            if (broadcasts)
            {
                _slider.onValueChanged.RemoveListener(onValueChanged);
            }
        }

        private void HandleSliderChangedMessage(MonoBehaviour sender, float newValue)
        {
            if (sender == null || sender == this) return;

#if LOG
            Debug.Log(string.Format("Slider\t{0}\tHandleSliderChangedMessage Source:\t{1}\tValue:{2}",
                      gameObject.name, sender.GetInstanceID(), newValue));
#endif

            _slider.value = newValue;
        }

        private void onValueChanged(float newValue)
        {
#if LOG
            Debug.Log(string.Format("Slider\t{0}\tonValueChanged:\t{1}",
                      gameObject.name, newValue));
#endif

            Messenger.Broadcast<MonoBehaviour, float>(valueMessage, this, newValue);
        }
    }
}
