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

#if HB_UNITYADS 
using UnityEngine.Advertisements;
using UnityEngine.Monetization;
#endif

namespace Hyperbyte.Ads
{
    /// <summary>
    /// This class component will be added to game dynamically if Unity Ads selected as active ad network.
    /// All the callbacks will be forwarded to ad manager.
    /// </summary>
	public class UnityAdsManager : AdHelper
    {
        UnityAdsSettings settings;
        bool showBannerCalled = false;

        /// <summary>
        /// Initialized the ad network.
        /// </summary>
		public override void InitializeAdNetwork()
        {
            settings = (UnityAdsSettings)(Resources.Load("AdNetworkSettings/UnityAdsSettings"));

            #if HB_UNITYADS
            Monetization.Initialize(settings.GetGameId(), settings.GetTestMode());

            Invoke("StartLoadingAds", 2F);
            #endif
        }

        /// <summary>
        /// Loads ads after initialization.
        /// </summary>
        public void StartLoadingAds()
        {
            #if HB_UNITYADS
            // GDPR Consent 
            UnityEngine.Advertisements.MetaData gdprMetaData = new UnityEngine.Advertisements.MetaData("gdpr");
            gdprMetaData.Set("consent", (AdManager.Instance.consentAllowed) ? "true" : "false");
            Advertisement.SetMetaData(gdprMetaData);

            // CCPA Consent
            // If the user opts out of targeted advertising:
            UnityEngine.Advertisements.MetaData privacyMetaData = new UnityEngine.Advertisements.MetaData("privacy");
            privacyMetaData.Set("consent",(AdManager.Instance.consentAllowed) ? "true" : "false");
            Advertisement.SetMetaData(privacyMetaData);
            #endif
            RequestBannerAds();
        }

        // Requests banner ad.        
        void RequestBannerAds()
        {
            #if HB_UNITYADS
            BannerLoadOptions options = new BannerLoadOptions();
            options.loadCallback += OnBannerLoaded;
            options.errorCallback = OnBannerLoadFailed;
            Advertisement.Banner.SetPosition(settings.GetBannerPosition());
            Advertisement.Banner.Load(settings.GetBannerPlacement(), options);
            #endif
        }

        // Requests intestitial ad.
        void RequestInterstitial()
        {
            #if HB_UNITYADS
            ShowAdCallbacks options = new ShowAdCallbacks();
            options.finishCallback = InterstitialShowResult;
            ShowAdPlacementContent ad = Monetization.GetPlacementContent(settings.GetInterstitialPlacement()) as ShowAdPlacementContent;
            ad.Show(options);
            #endif
        }

        // Requests rewarded ad.
        void RequestRewarded()
        {
            #if HB_UNITYADS
            ShowAdCallbacks options = new ShowAdCallbacks();
            options.finishCallback = RewardedShowResult;
            ShowAdPlacementContent ad = Monetization.GetPlacementContent(settings.GetRewardedPlacement()) as ShowAdPlacementContent;
            ad.Show(options);
            #endif
        }

        // Shows banner ad.
        public override void ShowBanner()
        {
            showBannerCalled = true;
            #if HB_UNITYADS
            if(Advertisement.Banner.isLoaded) {
                Advertisement.Banner.Show();
            }
            else  {
                Invoke("RequestBannerAds",2F);
            }
            #endif
        }

        // Hides banner ad.
        public override void HideBanner()
        {
            #if HB_UNITYADS
            Advertisement.Banner.Hide();
            #endif
        }

        // Check if interstial ad ready to show.
        public override bool IsInterstitialAvailable() { return true; }

        // Shows interstitial ad if available.
        public override void ShowInterstitial()
        {
            #if HB_UNITYADS
            if(Advertisement.IsReady(settings.GetInterstitialPlacement())) {
               RequestInterstitial();
            } 
            #endif
        }
        // Checks if rewarded ad ready to show.
        public override bool IsRewardedAvailable() { return true; }

        // Shows rewarded ad if loaded.
        public override void ShowRewarded()
        {
            #if HB_UNITYADS
             if(Advertisement.IsReady(settings.GetRewardedPlacement())) {
               RequestRewarded();
            } 
            #endif
        }

        #region  Banner Ad Callback
        //Banner ad  event callbacks.
        public void OnBannerLoaded()
        {
            if (AdManager.Instance.adSettings.showBannerOnLoad || showBannerCalled)
            {
                #if HB_UNITYADS
                Advertisement.Banner.Show(); 
                #endif
            }
            AdManager.Instance.OnBannerLoaded();
        }

        public void OnBannerLoadFailed(string message)
        {
            Invoke("RequestBannerAds", 5F);
            AdManager.Instance.OnBannerLoadFailed(message);
        }
        #endregion

        #region Interstitial Ad Callback
        // Interstitial ad  event callbacks.
        #if HB_UNITYADS
        void InterstitialShowResult(UnityEngine.Monetization.ShowResult result)
        {
            if (result == UnityEngine.Monetization.ShowResult.Finished) {
                AdManager.Instance.OnInterstitialClosed();
            }
            else if (result == UnityEngine.Monetization.ShowResult.Skipped)  {
                AdManager.Instance.OnInterstitialClosed();
            }
            else if (result == UnityEngine.Monetization.ShowResult.Failed)  {
                AdManager.Instance.OnInterstitialLoadFailed(result.ToString());
            }
        }
        #endif
        #endregion

        #region Rewarded Ad Callback
        // Rewarded ad  event callbacks.
        #if HB_UNITYADS
        void RewardedShowResult(UnityEngine.Monetization.ShowResult result)
        {
            if (result == UnityEngine.Monetization.ShowResult.Finished)  {
                AdManager.Instance.OnRewardedAdRewarded();
            }
            else if (result == UnityEngine.Monetization.ShowResult.Skipped) {
                AdManager.Instance.OnRewardedClosed();
            }
            else if (result == UnityEngine.Monetization.ShowResult.Failed) {
                AdManager.Instance.OnRewardedLoadFailed(result.ToString());
            }
        }
        #endif
        #endregion
    }
}
