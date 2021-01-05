﻿// ©2019 - 2020 HYPERBYTE STUDIOS LLP
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

namespace Hyperbyte.Localization
{
    /// <summary>
    /// This script can be attached to any UI Text component with text tag.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class LocalizedTextFormatted : MonoBehaviour
    {
        #pragma warning disable 0649
        [Tooltip("Assign Text tag containing localized text.")]
        [SerializeField] string txtTag;

        [SerializeField] string formattedValue1;
        [SerializeField] string formattedValue2;
        #pragma warning restore 0649

        Text thisText;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            thisText = GetComponent<Text>();

            if (txtTag == null) {
                enabled = false;
                return;
            }
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            LocalizationManager.OnLocalizationInitializedEvent += OnLocalizationInitialized;
            LocalizationManager.OnLanguageChangedEvent += OnLanguageChanged;

            LocalizeContent();
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            LocalizationManager.OnLocalizationInitializedEvent -= OnLocalizationInitialized;
            LocalizationManager.OnLanguageChangedEvent -= OnLanguageChanged;
        }

        /// <summary>
        /// Event callback on localization initializes.
        /// </summary>
        void OnLocalizationInitialized(LocalizedLanguage lang, bool isLocalizationSupported)
        {
            if (isLocalizationSupported) {
                LocalizeContent();
            }
        }

        /// <summary>
        /// Event callback on language change.
        /// </summary>
        void OnLanguageChanged(LocalizedLanguage lang)
        {
            LocalizeContent();
        }

        void LocalizeContent()
        {
            //if (LocalizationManager.Instance.hasLanguageChanged)
            {
                thisText.SetFormattedTextWithTag(txtTag, formattedValue1, formattedValue2);
            }
        }
    }
}
