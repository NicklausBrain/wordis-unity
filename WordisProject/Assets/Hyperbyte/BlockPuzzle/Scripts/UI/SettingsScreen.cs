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
using UnityEngine;
using UnityEngine.UI;
using Hyperbyte.Localization;

namespace Hyperbyte
{
	/// <summary>
	/// Settings screen controlls different user selection like sound, music, langauge etc.
	/// </summary>
    public class SettingsScreen : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] GameObject dataSettingsOption;
        [SerializeField] GameObject supportButton;
        [SerializeField] GameObject vibrationToggleButton;
        [SerializeField] GameObject selectLanguageButton;
        [SerializeField] Text txtVersion;
        #pragma warning restore 0649

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            dataSettingsOption.SetActive((PlayerPrefs.GetInt("ConsentRequired", 0) == 0) ? false : true);
            supportButton.SetActive((ProfileManager.Instance.GetAppSettings().enableSupportURL) ? true : false);
            vibrationToggleButton.SetActive((ProfileManager.Instance.GetAppSettings().enableVibrations) ? true : false);

            int activeLanguages = 0;
            foreach (LocalizedLanguage lang in LocalizationManager.Instance.allLocalizedLanaguages)
            {
                if (lang.isLanguageEnabled)
                {
                    activeLanguages += 1;
                }
            }
            selectLanguageButton.SetActive(((LocalizationManager.Instance.isLocalizationSupported) && (activeLanguages > 1)));
            txtVersion.text = "Version : " + Application.version;
        }

		/// <summary>
		/// Close button click listener.
		/// </summary>
        public void OnCloseButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                gameObject.Deactivate();
            }
        }

		/// <summary>
		/// Support button click listener.
		/// </summary>
        public void OnSupportButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                StartCoroutine(NavigateToUrl(ProfileManager.Instance.GetAppSettings().supportURL));
            }
        }

		/// <summary>
		/// Privacy policy button click listener.
		/// </summary>
        public void OnPrivacyPolicyButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                StartCoroutine(NavigateToUrl(ProfileManager.Instance.GetAppSettings().privacyPolicyURL));
            }
        }

		/// <summary>
		/// Data privacy button click listener.
		/// </summary>
        public void OnDataSettingsButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.ShowConsentDialogue();
            }
        }

		/// <summary>
		/// Lanaguage select button click listener.
		/// </summary>
        public void OnSelectLanguageButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.lanagueSelectionScreen.Activate();
                gameObject.Deactivate();
            }
        }

		/// <summary>
		/// Navigate to given URL.
		/// </summary>
        IEnumerator NavigateToUrl(string url)
        {
            yield return new WaitForSeconds(0.2F);
            Application.OpenURL(url);
        }
    }
}
