//
// SliderBinding.cs
//
// Author:
//       p4p <p4p@bunkville.com>
//
// Copyright (c) 2015 p4p
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#define LOG

using UnityEngine;
using UnityEngine.UI;

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
