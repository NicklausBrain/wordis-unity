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

namespace Hyperbyte.Ads
{
    /// <summary>
    /// This class component will be added to game dynamically if applovin selected as active ad network.
    /// All the callbacks will be forwarded to ad manager.
    /// </summary>
	public class AppLovinAdManager : AdHelper
    {
        AppLovinAdsSettings settings;

        /// <summary>
        /// Initialized the ad network.
        /// </summary>
		public override void InitializeAdNetwork()
        {
            settings = (AppLovinAdsSettings)(Resources.Load("AdNetworkSettings/AppLovinAdsSettings"));

            #if HB_APPLOVINMAX
            MaxSdk.SetSdkKey(settings.GetSDkKey());
            MaxSdk.InitializeSdk();

              MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
                if(Debug.isDebugBuild) {
                    MaxSdk.ShowMediationDebugger();
                }
                // GDPR Consent
                MaxSdk.SetHasUserConsent(AdManager.Instance.consentAllowed);

                // CCPA
                MaxSdk.SetDoNotSell(!AdManager.Instance.consentAllowed);
                StartLoadingAds();
            };
            #endif
        }

        /// <summary>
        /// Loads ads after initialization.
        /// </summary>
        public void StartLoadingAds()
        {
            RequestBannerAds();
            RequestInterstitial();
            RequestRewarded();
        }

        // Requests banner ad.        
        void RequestBannerAds()
        {
            #if HB_APPLOVINMAX
            MaxSdk.CreateBanner(settings.GetBannetAdUnitId(), settings.GetBannerPosition());
            MaxSdk.SetBannerBackgroundColor(settings.GetBannetAdUnitId(), ColorUtils.GetColorFromHexa(settings.GetBannerBgColor()));
            #endif
        }

        // Requests intestitial ad.
        void RequestInterstitial()
        {
            #if HB_APPLOVINMAX
            MaxSdk.LoadInterstitial(settings.GetInterstitialAdUnityId());
            #endif
        }

        // Requests rewarded ad.
        void RequestRewarded()
        {
            #if HB_APPLOVINMAX
            MaxSdk.LoadRewardedAd(settings.GetRewardedAdUnitId());
            #endif
        }

        // Shows banner ad.
        public override void ShowBanner()
        {
            #if HB_APPLOVINMAX
            MaxSdk.ShowBanner(settings.GetBannetAdUnitId());
            #endif
        }

        // Hides banner ad.
        public override void HideBanner()
        {
            #if HB_APPLOVINMAX
            MaxSdk.HideBanner(settings.GetBannetAdUnitId());
            #endif
        }

        // Check if interstial ad ready to show.
        public override bool IsInterstitialAvailable()
        {
            #if HB_APPLOVINMAX
            return MaxSdk.IsInterstitialReady(settings.GetInterstitialAdUnityId()); 
            #endif
            return false;
        }

        // Shows interstitial ad if available.
        public override void ShowInterstitial()
        {
            #if HB_APPLOVINMAX
            if (MaxSdk.IsInterstitialReady(settings.GetInterstitialAdUnityId())) {
                MaxSdk.ShowInterstitial(settings.GetInterstitialAdUnityId());
            }
            #endif
        }

        // Checks if rewarded ad ready to show.
        public override bool IsRewardedAvailable()
        {
            #if HB_APPLOVINMAX
            return MaxSdk.IsRewardedAdReady(settings.GetRewardedAdUnitId());
            #endif
            return false;
        }

        // Shows rewarded ad if loaded.
        public override void ShowRewarded()
        {
            #if HB_APPLOVINMAX
            if (MaxSdk.IsRewardedAdReady(settings.GetRewardedAdUnitId())) {
                MaxSdk.ShowRewardedAd(settings.GetRewardedAdUnitId());
            }
            #endif
        }

        #if HB_APPLOVINMAX
        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable() {
            
            //TODO : Banner Ad CallBacks
            MaxSdkCallbacks.OnBannerAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.OnBannerAdLoadFailedEvent += OnBannerAdLoadFailedEvent;

            //TODO : Interstitial Ad Callbacks.
            MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;

            //TODO : Rewarded Ad CallBacks
            MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += RewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable() {
             //TODO : Banner Ad CallBacks
            MaxSdkCallbacks.OnBannerAdLoadedEvent -= OnBannerAdLoadedEvent;
            MaxSdkCallbacks.OnBannerAdLoadFailedEvent -= OnBannerAdLoadFailedEvent;

            //TODO : Interstitial Ad Callbacks.
            MaxSdkCallbacks.OnInterstitialLoadedEvent -= OnInterstitialLoadedEvent;
            MaxSdkCallbacks.OnInterstitialLoadFailedEvent -= OnInterstitialFailedEvent;
            MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent -= InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.OnInterstitialHiddenEvent -= OnInterstitialDismissedEvent;

            //TODO : Rewarded Ad CallBacks
            MaxSdkCallbacks.OnRewardedAdLoadedEvent -= OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.OnRewardedAdHiddenEvent -= OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.OnRewardedAdLoadFailedEvent -= OnRewardedAdFailedEvent;
            MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent -= RewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;
        }

        #region Max Banner Callbacks
        //Banner ad  event callbacks.
        private void OnBannerAdLoadedEvent(string adUnitId) {
            if(AdManager.Instance.adSettings.showBannerOnLoad) {
                ShowBanner();
            }
            AdManager.Instance.OnBannerLoaded();
        }

        private void OnBannerAdLoadFailedEvent(string adUnitId, int errorCode)
        {
            Invoke("RequestBannerAds",5F);
            AdManager.Instance.OnBannerLoadFailed(errorCode.ToString());
        }
        #endregion

        #region Max Interstitial Callbacks
        // Interstitial ad event callbacks.
        private void OnInterstitialLoadedEvent(string adUnitId) {
            AdManager.Instance.OnInterstitialLoaded();
        }

        private void OnInterstitialFailedEvent(string adUnitId, int errorCode) { 
            Invoke("RequestInterstitial", 2);
            AdManager.Instance.OnInterstitialLoadFailed(errorCode.ToString());
        }

        private void OnInterstitialDismissedEvent(string adUnitId) {
            RequestInterstitial();
            AdManager.Instance.OnInterstitialClosed();
        }
        #endregion

        private void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)  {
            Invoke("RequestInterstitial", 2);
            AdManager.Instance.OnInterstitialLoadFailed(errorCode.ToString());
        }

        #region Max Interstitial Callbacks
        // Rewarded ad event callbacks.
        private void OnRewardedAdLoadedEvent(string adUnitId) {
            AdManager.Instance.OnRewardedLoaded();
        }

        private void OnRewardedAdFailedEvent(string adUnitId, int errorCode) {
            Invoke("RequestRewarded", 2);
            AdManager.Instance.OnRewardedLoadFailed(errorCode.ToString());
        }

        private void RewardedAdFailedToDisplayEvent(string adUnitId, int errorCode){
            Invoke("RequestRewarded", 2);
            AdManager.Instance.OnRewardedLoadFailed(errorCode.ToString());
        }

        private void OnRewardedAdDismissedEvent(string adUnitId)  {
           RequestRewarded();
           AdManager.Instance.OnRewardedClosed();
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdkBase.Reward reward) {
           AdManager.Instance.OnRewardedAdRewarded();
        }
        #endregion
        #endif
    }
}