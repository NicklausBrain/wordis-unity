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
    /// This script component is attached to game pauser screen.
    /// </summary>
	public class PauseGame : MonoBehaviour {

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable() {
            /// Pauses the game when it gets enabled.
            GamePlayUI.Instance.PauseGame();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable() {
            /// Resumes the game when it gets enabled.
            GamePlayUI.Instance.ResumeGame();
        }

        /// <summary>
        /// Resumes the game.
        /// </summary>
        public void OnResumeButtonPressed() {
            if (InputManager.Instance.canInput()) {
                UIFeedback.Instance.PlayButtonPressEffect();
                gameObject.Deactivate();
            }
        }

        /// <summary>
        /// Restarts the game.
        /// </summary>
        public void OnRestartButtonPressed() {
            if (InputManager.Instance.canInput()) {
                UIFeedback.Instance.PlayButtonPressEffect();
                GamePlayUI.Instance.RestartGame();
                gameObject.Deactivate();
            }
        }

        /// <summary>
        /// Closes the gameplay and navigates to home screen.
        /// </summary>
        public void OnHomeButtonPressed() {
            if (InputManager.Instance.canInput()) {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.OpenHomeScreenFromPauseGame();
            }
        }

        /// <summary>
        /// Closes pause screen and resumes gameplay.
        /// </summary>
        public void OnCloseButtonPressed() {
            if (InputManager.Instance.canInput()) {
                UIFeedback.Instance.PlayButtonPressEffect();
                gameObject.Deactivate();
            }
        }
    }
}
