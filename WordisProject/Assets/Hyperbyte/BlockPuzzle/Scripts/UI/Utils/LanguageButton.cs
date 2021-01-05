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
using UnityEngine.UI;
using Hyperbyte.Localization;

namespace Hyperbyte
{
    /// <summary>
    /// This script is attached to each langauge selection button. This script will change app language to
    /// selected on pressing buhtton.!--
    /// </summary>
    public class LanguageButton : MonoBehaviour
    {
        /// Instance of current button's localize language.
        LocalizedLanguage currentButtonLanaguage;

        #pragma warning disable 0649
		// Name of langauge.
        [SerializeField] Text txtLangaugeName;

		// Check mark enabled if current language is this.
        [SerializeField] Image imgCheckMark;

		// Line below localization button.
        [SerializeField] Image imgLine;
        #pragma warning restore 0649


        // Language is active?
        bool isActiveLangauge = false;

        /// <summary>
        ///  Initializes language button and restores its state.
        /// </summary>
        public void SetLanaguage(LocalizedLanguage lang, bool isActive = false)
        {
            currentButtonLanaguage = lang;
            txtLangaugeName.text = currentButtonLanaguage.langaugeDisplayName;

            isActiveLangauge = isActive;
            imgCheckMark.enabled = isActiveLangauge;
        }

        /// <summary>
        /// Language select button listener.
        /// </summary>
        public void OnLanagueButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                InputManager.Instance.DisableTouchForDelay();
                UIFeedback.Instance.PlayButtonPressEffect();

                if (!isActiveLangauge)
                {
                    LocalizationManager.Instance.SetLocalizedLanguage(currentButtonLanaguage);
                }
            }
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            LocalizationManager.OnLanguageChangedEvent += OnLanguageChanged;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            LocalizationManager.OnLanguageChangedEvent -= OnLanguageChanged;
        }

        /// <summary>
        /// App langauge will be changed and language change callback will be invoked if current language is different then selected.
        /// </summary>
        void OnLanguageChanged(LocalizedLanguage lang)
        {
            if (currentButtonLanaguage.languageCode.Equals(lang.languageCode))
            {
                if (!isActiveLangauge)
                {
                    imgCheckMark.enabled = true;
                    isActiveLangauge = true;
                }
            }
            else
            {
                if (isActiveLangauge)
                {
                    imgCheckMark.enabled = false;
                }
                isActiveLangauge = false;
            }
        }
    }
}
