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

using UnityEngine;

namespace Hyperbyte 
{   
    /// <summary>
    /// This script is attached to purchase success popup.
    /// </summary>
    public class PurchaseSuccess : MonoBehaviour
    {
        public RectTransform rewardAnimPosition;

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable() {
            UIController.Instance.PlayAddGemsAnimationAtPosition(Vector3.zero, 0.2F);
        }

        /// <summary>
        /// Close button click listener.
        /// </summary>
        public void OnCloseButtonPressed() {
			if(InputManager.Instance.canInput()) {
                UIFeedback.Instance.PlayButtonPressEffect();
				gameObject.Deactivate();
			}
		}

        /// <summary>
        /// Ok button click listener.
        /// </summary>
        public void OnOkButtonPressed() {
			if(InputManager.Instance.canInput()) {
                UIFeedback.Instance.PlayButtonPressEffect();
				gameObject.Deactivate();
			}
		}
    }
}
