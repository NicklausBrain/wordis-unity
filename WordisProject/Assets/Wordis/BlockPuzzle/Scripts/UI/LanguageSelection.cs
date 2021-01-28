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

using Assets.Wordis.BlockPuzzle.Scripts.Controller;
using Assets.Wordis.BlockPuzzle.Scripts.UI.Extensions;
using Assets.Wordis.BlockPuzzle.Scripts.UI.Utils;
using Assets.Wordis.Frameworks.InputManager.Scripts;
using Assets.Wordis.Frameworks.Localization.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Wordis.BlockPuzzle.Scripts.UI
{
    /// <summary>
    /// This script is attached to language selection popup and sets app language as user selected language. This script also prepares screen and
    /// creates selection button for each available languages. Languages settings can be updated from Hyperbyte -> Localization Settings menu item.
    /// </summary>
    public class LanguageSelection : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] GameObject languageButtonTemplate;
        [SerializeField] GameObject languageListContent;
#pragma warning restore 0649

        /// <summary>
        /// Close button listener.
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
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            PrepareLanguageSelectionScreen();
        }

        /// <summary>
        /// Create selection button for each available language.
        /// </summary>
        void PrepareLanguageSelectionScreen()
        {
            LocalizedLanguage currentLanguage = LocalizationManager.Instance.GetCurrentLanguage();

            if (currentLanguage != null)
            {
                foreach (LocalizedLanguage lang in LocalizationManager.Instance.allLocalizedLanaguages)
                {
                    if (lang.isLanguageEnabled)
                    {
                        GetLanguageButton(lang, lang.languageCode.Equals(currentLanguage.languageCode));
                    }
                }

                GetMoreToComeButton();
            }
        }

        /// <summary>
        /// Instantiates a button from template.
        /// </summary>
        /// <returns></returns>
        private void GetLanguageButton(LocalizedLanguage lang, bool isActive = false)
        {
            GameObject langButton = Instantiate(languageButtonTemplate);
            langButton.transform.SetParent(languageListContent.transform);
            langButton.name = lang.languageName;
            langButton.transform.localScale = Vector3.one;
            langButton.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            langButton.transform.SetAsLastSibling();
            langButton.GetComponent<LanguageButton>().SetLanaguage(lang, isActive);
            langButton.SetActive(true);
        }

        private void GetMoreToComeButton()
        {
            GameObject langButton = Instantiate(languageButtonTemplate);
            langButton.transform.SetParent(languageListContent.transform);
            langButton.name = "moreToComeBtn";
            langButton.transform.localScale = Vector3.one;
            langButton.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            langButton.transform.SetAsLastSibling();
            langButton.GetComponentInChildren<Text>().text = "More to come...";
            langButton.GetComponent<Button>().interactable = false;
            langButton.SetActive(true);
        }
    }
}