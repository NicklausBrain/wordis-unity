// ©2019 - 2020 HYPERBYTE STUDIOS LLP
// All rights reserved
// Redistribution of this software is strictly not allowed.
// Copy of this software can be obtained from unity asset store only.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections;
using Assets.Wordis.Frameworks.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Wordis.Frameworks.InputManager.Scripts
{
    public class InputManager : Singleton<InputManager>
    {
        private static bool _isTouchAvailable = true;
        public EventSystem eventSystem;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            if (eventSystem == null)
            {
                eventSystem = FindObjectOfType<EventSystem>() as EventSystem;
            }
        }

        public bool canInput(float delay = 0.25F, bool disableOnAvailable = true)
        {
            bool status = _isTouchAvailable;
            if (status && disableOnAvailable)
            {
                _isTouchAvailable = false;
                eventSystem.enabled = false;

                StopCoroutine(nameof(EnableTouchAfterDelay));
                StartCoroutine(nameof(EnableTouchAfterDelay), delay);
            }

            return status;
        }

        public void DisableTouch()
        {
            _isTouchAvailable = false;
            eventSystem.enabled = false;
        }

        public void DisableTouchForDelay(float delay = 0.25F)
        {
            _isTouchAvailable = false;
            eventSystem.enabled = false;

            StopCoroutine(nameof(EnableTouchAfterDelay));
            StartCoroutine(nameof(EnableTouchAfterDelay), delay);
        }

        public void EnableTouch()
        {
            _isTouchAvailable = true;
            eventSystem.enabled = true;
        }

        public IEnumerator EnableTouchAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            EnableTouch();
        }
    }
}