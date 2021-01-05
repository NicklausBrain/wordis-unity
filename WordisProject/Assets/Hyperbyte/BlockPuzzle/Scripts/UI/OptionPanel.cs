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
	/// Varies option button listner attached to this on home screen.
	/// </summary>
    public class OptionPanel : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] GameObject themeSettingButton;
        #pragma warning restore 0649

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            if(!ThemeManager.Instance.UIThemeEnabled) {
                themeSettingButton.SetActive(false);
            }
        }

		/// <summary>
		/// Opens setting screen.
		/// </summary>
        public void OnSettingsButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                InputManager.Instance.DisableTouchForDelay();
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.settingScreen.Activate();
            }
        }

		/// <summary>
		/// Opens shop popup.
		/// </summary>
        public void OnShopButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                InputManager.Instance.DisableTouchForDelay();
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.shopScreen.Activate();
            }
        }

		/// <summary>
		/// Opens language selection popup.
		/// </summary>
        public void OnSelectLangaugeButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                InputManager.Instance.DisableTouchForDelay();
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.lanagueSelectionScreen.Activate();
            }
        }

		/// <summary>
		/// Opens review popup or store review nag.
		/// </summary>
        public void OnRateButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                InputManager.Instance.DisableTouchForDelay();
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.reviewScreen.Activate();
            }
        }	

		/// <summary>
		/// Open theme selection popup.
		/// </summary>
        public void OnThemeButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                InputManager.Instance.DisableTouchForDelay();
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.selectThemeScreen.Activate();
            }
        }
    }
}

