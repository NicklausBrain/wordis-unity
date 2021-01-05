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
using Hyperbyte.Ads;

namespace Hyperbyte
{
    /// <summary>
    /// This script is a demo script to test ad.
    /// </summary>
    public class AdDemo : MonoBehaviour
    {
        public Text txtResult;

        /// <summary>
        /// Loads banner ad.
        /// </summary>
        public void ShowBanner()
        {
            AdManager.Instance.ShowBanner();
        }

        /// <summary>
        /// Hides banner ad if loaded.
        /// </summary>
        public void HideBanner()
        {
            AdManager.Instance.HideBanner();
        }

        /// <summary>
        /// Checks if interstitial ad is available.
        /// </summary>
        public void IsInterstitialAvailable()
        {
            bool isAvailable = AdManager.Instance.IsInterstitialAvailable();

            ShowText("Interstitial Available : " + isAvailable);
        }

        /// <summary>
        /// Shows interstitial ad if available.
        /// </summary>
        public void ShowInterstitial()
        {
            AdManager.Instance.ShowInterstitial();
        }

        /// <summary>
        /// Checks if rewarded ad is available.
        /// </summary>
        public void IsRewardedAvailable()
        {
            bool isAvailable = AdManager.Instance.IsRewardedAvailable();
            ShowText("Rewarded Available : " + isAvailable);
        }

        /// <summary>
        /// Shows rewarded ad if available.
        /// </summary>
        public void ShowRewarded()
        {
            AdManager.Instance.ShowRewardedWithTag("demo");
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            //Registers Ad status callbacks.
            AdManager.OnBannerLoadedEvent += OnBannerLoaded;
            AdManager.OnBannerLoadFailedEvent += OnBannerFailed;

            AdManager.OnInterstitialLoadedEvent += OnInterstitialLoaded;
            AdManager.OnInterstitialLoadFailedEvent += OnInterstitialLoadFailed;
            AdManager.OnInterstitialShownEvent += OnInterstitialShown;
            AdManager.OnInterstitialClosedEvent += OnInterstitialClosed;

            AdManager.OnRewardedLoadedEvent += OnRewardedLoaded;
            AdManager.OnRewardedLoadFailedEvent += OnRewardedLoadFailed;
            AdManager.OnRewardedShownEvent += OnRewardedShown;
            AdManager.OnRewardedClosedEvent += OnRewardedClosed;
            AdManager.OnRewardedAdRewardedEvent += OnRewardedAdRewarded;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            //Unregisters Ad status callbacks.
            AdManager.OnBannerLoadedEvent -= OnBannerLoaded;
            AdManager.OnBannerLoadFailedEvent -= OnBannerFailed;

            AdManager.OnInterstitialLoadedEvent -= OnInterstitialLoaded;
            AdManager.OnInterstitialLoadFailedEvent -= OnInterstitialLoadFailed;
            AdManager.OnInterstitialShownEvent -= OnInterstitialShown;
            AdManager.OnInterstitialClosedEvent -= OnInterstitialClosed;

            AdManager.OnRewardedLoadedEvent -= OnRewardedLoaded;
            AdManager.OnRewardedLoadFailedEvent -= OnRewardedLoadFailed;
            AdManager.OnRewardedShownEvent -= OnRewardedShown;
            AdManager.OnRewardedClosedEvent -= OnRewardedClosed;
            AdManager.OnRewardedAdRewardedEvent -= OnRewardedAdRewarded;
        }

		// Invokes callback on banner ad loaded and ready to show.
        void OnBannerLoaded()
        {
            ShowText("Banner Loaded.");
        }

		// Invokes callback on banner ad fails to load.
        void OnBannerFailed(string reason)
        {
            ShowText("Banner Load Failed : " + reason);
        }

		// Invokes callback on interstitial ad loaded and ready to show.
        void OnInterstitialLoaded()
        {
            ShowText("Interstitial Loaded.");
        }

		// Invokes callback on interstitial ad fails to load.
        void OnInterstitialLoadFailed(string reason)
        {
            ShowText("Interstitial Load Failed : " + reason);
        }

		// Invokes callback on showing interstitial.
        void OnInterstitialShown()
        {
            ShowText("Interstitial Shown.");
        }

		// Invokes callback on closing interstitial.
        void OnInterstitialClosed()
        {
            ShowText("Interstitial Closed.");
        }

		// Invokes callback rewarded ad loaded and ready to show.
        void OnRewardedLoaded()
        {
            ShowText("Rewarded Loaded.");
        }

		// Invokes callback on rewarded ad fails to load.
        void OnRewardedLoadFailed(string reason)
        {
            ShowText("Rewarded Load Failed : " + reason);
        }

		// Invokes callback on starting rewarded ad. 
        void OnRewardedShown()
        {
            ShowText("Rewarded Shown.");
        }

		// Invokes callback on rewarded ad closed.
        void OnRewardedClosed()
        {
            ShowText("Rewarded Closed.");
        }

		// Invokes callback on rewarded ad finishes ad rewarded.
        void OnRewardedAdRewarded(string tag)
        {
            ShowText("Rewarded Rewarded : " + tag);
        }

		// Close ad demo popup.
        public void OnCloseButtonPressed()
        {
            gameObject.SetActive(false);
        }

		// Shows the debug text.
        void ShowText(string text)
        {
            txtResult.text = text;
            Invoke("HideText", 3F);
        }

		// Hides debug text.
        void HideText()
        {
            txtResult.text = "";
        }
    }
}
