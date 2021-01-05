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
using Hyperbyte.Ads;
using UnityEngine;

namespace Hyperbyte
{
    public class ConsentSetting : MonoBehaviour
    {
        /// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
        private void Awake()
        {
            PlayerPrefs.SetInt("ConsentRequired", 1);
        }


        /// <summary>
        /// Privacy button click event, will open privacy policy url.
        /// </summary>
        public void OnPrivacyPolicyButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                StartCoroutine(NavigateToUrl(ProfileManager.Instance.GetAppSettings().privacyPolicyURL));
            }
        }

        IEnumerator NavigateToUrl(string url)
        {
            yield return new WaitForSeconds(0.2F);
            Application.OpenURL(url);
        }

        /// <summary>
        /// Approve/Accept consent.
        /// </summary>
        public void OnContinueButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                AdManager.Instance.SetConsentStatus(true);
                gameObject.Deactivate();
            }
        }

        /// <summary>
        /// Not accepting consent.
        /// </summary>
        public void OnNotNowButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                AdManager.Instance.SetConsentStatus(false);
                gameObject.Deactivate();
            }
        }
    }
}
