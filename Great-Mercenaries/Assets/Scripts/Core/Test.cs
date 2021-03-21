//
// Test.cs
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

using UnityEngine;

public class Test : MonoBehaviour
{
    public string valueMessage;
    public bool changeAutomatically = false;

    public float repeatRate = 3.0f;

    // Use this for initialization
    private void Start()
    {
        if (changeAutomatically)
        {
            BroadcastStuff();
        }
    }

    private void BroadcastStuff()
    {
        Messenger.Broadcast<MonoBehaviour, float>(valueMessage, this, Random.Range(0.0f, 1.0f));
        Invoke("BroadcastStuff", repeatRate);
    }

    public void ActionBroadcast()
    {
        Messenger.Broadcast<MonoBehaviour, float>(valueMessage, this, Random.Range(0.0f, 1.0f));
    }
}
