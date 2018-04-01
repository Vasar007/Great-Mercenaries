//
// TextBinding.cs
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

//#define LOG

using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TextBinding : MonoBehaviour
{
    public bool messageConvertFloatToText = false;

    [SerializeField]
    private string _valueMessage;

    private Text _label;

    private void Awake()
    {
        _label = GetComponent<Text>();
        if (_label == null)
        {
            throw new ArgumentNullException("_label", "Not found bound text component!");
        }
    }

    private void OnDestroy()
    {
        if (string.IsNullOrEmpty(_valueMessage)) return;

        if (messageConvertFloatToText)
        {
            Messenger.RemoveListener<MonoBehaviour, float>(_valueMessage, HandleFloatTextMessage);
        }
        else
        {
            Messenger.RemoveListener<MonoBehaviour, string>(_valueMessage, HandleStringTextMessage);
        }
    }

    private void HandleFloatTextMessage(MonoBehaviour sender, float newValue)
    {
        if (sender == null || sender == this) return;

        #if LOG
            Debug.Log(string.Format("Text\t{0}\tHandleFloatTextMessage Source:\t{1}\tValue:{2}",
                      gameObject.name, sender.GetInstanceID(), newValue));
        #endif

        _label.text = newValue.ToString(CultureInfo.InvariantCulture);
    }

    private void HandleStringTextMessage(MonoBehaviour sender, string newValue)
    {
        if (sender == null || sender == this) return;

        #if LOG
            Debug.Log(string.Format("Text\t{0}\tHandleStringTextMessage Source:\t{1}\tValue:{2}",
                      gameObject.name, sender.GetInstanceID(), newValue));
        #endif

        _label.text = newValue;
    }

    public void CreateListener(string message)
    {
        _valueMessage = message;

        if (messageConvertFloatToText)
        {
            Messenger.AddListener<MonoBehaviour, float>(_valueMessage, HandleFloatTextMessage);
        }
        else
        {
            Messenger.AddListener<MonoBehaviour, string>(_valueMessage, HandleStringTextMessage);
        }
        #if LOG
            Debug.Log("TextBinding.CreateListener with message " + _valueMessage);
        #endif
    }

    public void SetValueMessage(string message)
    {
        _valueMessage = message;
    }

    public string GetValueMessage()
    {
        return _valueMessage;
    }
}
