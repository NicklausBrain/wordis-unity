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

#if HB_ADMOB
using GoogleMobileAds.Api;
using System;
#endif

namespace Hyperbyte.Ads
{
    /// <summary>
    /// This class component will be added to game dynamically if Google Ads is selected as active ad network.
    /// All the callbacks will be forwarded to ad manager.
    /// </summary>
    public class GoogleMobileAdsManager : AdHelper
    {
        GoogleMobileAdsSettings settings;

        #if HB_ADMOB
        private BannerView bannerView;
        private InterstitialAd interstitial;
        private RewardedAd rewardedAd;
        #endif

        /// <summary>
        /// Initialized the ad network.
        /// </summary>
        public override void InitializeAdNetwork()
        {
            settings = (GoogleMobileAdsSettings)(Resources.Load("AdNetworkSettings/GoogleMobileAdsSettings"));

            #if HB_ADMOB
            MobileAds.SetiOSAppPauseOnBackground(true);
            MobileAds.Initialize(settings.GetAppId());
            #endif

            Invoke("StartLoadingAds", 2F);
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
        public void RequestBannerAds()
        {
            #if HB_ADMOB
            if (this.bannerView != null) {
                this.bannerView.Destroy();
            }
            bannerView = new BannerView(settings.GetBannetAdUnitId(), AdSize.Banner, settings.GetBannerPosition());
            AdRequest request = new AdRequest.Builder().Build();

            // Register for ad events.
            this.bannerView.OnAdLoaded += this.HandleBannerAdLoaded;
            this.bannerView.OnAdFailedToLoad += this.HandleBannerAdFailedToLoad;
            this.bannerView.OnAdOpening += this.HandleBannerAdOpened;
            this.bannerView.OnAdClosed += this.HandleBannerAdClosed;
            this.bannerView.OnAdLeavingApplication += this.HandleBannerAdLeftApplication;
            bannerView.LoadAd(this.CreateAdRequest());
            bannerView.Hide();

            if (AdManager.Instance.adSettings.showBannerOnLoad) {
                bannerView.Show();
            }
            #endif
        }

        // Requests intestitial ad.
        public void RequestInterstitial()
        {
            #if HB_ADMOB
            if (this.interstitial != null) {
                this.interstitial.Destroy();
            }
            // Create an interstitial.
            this.interstitial = new InterstitialAd(settings.GetInterstitialAdUnityId());

            // Register for ad events.
            this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
            this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
            this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
            this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
            this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

            // Load an interstitial ad.
            this.interstitial.LoadAd(this.CreateAdRequest());
            #endif
        }

        // Requests rewarded ad.
        public void RequestRewarded()
        {
            #if HB_ADMOB
            // Create new rewarded ad instance.
            this.rewardedAd = new RewardedAd(settings.GetRewardedAdUnitId());
            // Called when an ad request has successfully loaded.
            this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
            // Called when an ad request failed to load.
            this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            // Called when an ad is shown.
            this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
            // Called when an ad request failed to show.
            this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            // Called when the user should be rewarded for interacting with the ad.
            this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            // Called when the ad is closed.
            this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

            // Create an empty ad request.
            AdRequest request = this.CreateAdRequest();
            // Load the rewarded ad with the request.
            this.rewardedAd.LoadAd(request);
            #endif
        }

        #if HB_ADMOB
        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder()
                .AddExtra("npa", (AdManager.Instance.consentAllowed) ? "0" : "1")
                //.AddTestDevice(AdRequest.TestDeviceSimulator)
                //.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
                .AddKeyword("game")
                //.SetGender(Gender.Male)
                //.SetBirthday(new DateTime(1985, 1, 1))
                .TagForChildDirectedTreatment(settings.GetTagForChildTreatment())
                .AddExtra("color_bg", settings.GetBannerBgColor())
                .Build();

        }
        #endif
        
        // Shows banner ad.
        public override void ShowBanner()
        {
            #if HB_ADMOB
            if (this.bannerView != null) {
                this.bannerView.Show();
            }
            #endif
        }
    
        // Hides banner ad.
        public override void HideBanner()
        {
            #if HB_ADMOB
            if (this.bannerView != null) {
                this.bannerView.Hide();
            }
            #endif
        }

        // Check if interstial ad ready to show.
        public override bool IsInterstitialAvailable()
        {
            #if HB_ADMOB
            return this.interstitial.IsLoaded(); 
            #endif
            return false;
        }
        
        // Shows interstitial ad if available.
        public override void ShowInterstitial()
        {
            #if HB_ADMOB
            if (this.interstitial.IsLoaded()) {
                this.interstitial.Show();
            }
            #endif
        }

        // Checks if rewarded ad ready to show.
        public override bool IsRewardedAvailable()
        {
            #if HB_ADMOB
            return this.rewardedAd.IsLoaded(); 
            #endif
            return false;
        }

        // Shows rewarded ad if loaded.
        public override void ShowRewarded()
        {
            #if HB_ADMOB
            if (this.rewardedAd.IsLoaded()) {
                this.rewardedAd.Show();
            }
            #endif
        }

        #if HB_ADMOB
        #region Banner callback handlers
        // Banner ad  event callbacks.
        public void HandleBannerAdLoaded(object sender, EventArgs args) {
            AdManager.Instance.OnBannerLoaded();
        }

        public void HandleBannerAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
            AdManager.Instance.OnBannerLoadFailed(args.Message);
        }

        public void HandleBannerAdOpened(object sender, EventArgs args) {
        }

        public void HandleBannerAdClosed(object sender, EventArgs args) {
        }

        public void HandleBannerAdLeftApplication(object sender, EventArgs args) {
        }
        #endregion

        #region Interstitial callback handlers
        // Interstitial ad event callbacks.
        public void HandleInterstitialLoaded(object sender, EventArgs args) {
            AdManager.Instance.OnInterstitialLoaded();
        }

        public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
            AdManager.Instance.OnInterstitialLoadFailed(args.Message);
        }

        public void HandleInterstitialOpened(object sender, EventArgs args) {
            AdManager.Instance.OnInterstitialShown();
        }

        public void HandleInterstitialClosed(object sender, EventArgs args) {
            MonoBehaviour.print("HandleInterstitialClosed event received");
            RequestInterstitial();
            AdManager.Instance.OnInterstitialClosed();
        }

        public void HandleInterstitialLeftApplication(object sender, EventArgs args) {
            MonoBehaviour.print("HandleInterstitialLeftApplication event received");
        }

        #endregion

        #region RewardedAd callback handlers
        // Rewarded ad event callbacks.
        public void HandleRewardedAdLoaded(object sender, EventArgs args) {
            AdManager.Instance.OnRewardedLoaded();
        }

        public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args) {
            AdManager.Instance.OnRewardedLoadFailed(args.Message);
        }

        public void HandleRewardedAdOpening(object sender, EventArgs args) {
           AdManager.Instance.OnRewardedShown();
        }

        public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)  {
        }

        public void HandleRewardedAdClosed(object sender, EventArgs args)
        {
            RequestRewarded();
            AdManager.Instance.OnRewardedClosed();
        }

        public void HandleUserEarnedReward(object sender, Reward args) {
            string type = args.Type;
            double amount = args.Amount;
            AdManager.Instance.OnRewardedAdRewarded();
        }
        #endregion
        #endif
    }
}
