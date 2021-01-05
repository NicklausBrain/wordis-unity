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
    /// This script component is required only in case when you want to use different ad network then available to select from ad manager. 
    /// Ads need to initialize and handled manually in this case.
    /// All the ad callback need to be forwarded to admanager to work ads properly.
    
    // ********** IMPORTANT **********
    // This script is intended to use custom ad netork, so please add code specific to targeted ad nerwork. We've only given a blank template to make 
    // Implementation easy.
    /// </summary>
	public class CustomAdManager : AdHelper
    {
        // Initialize your ad network. Will be called automatically. Please put your initialization code here. Also be sure to call
        // StartLoadingAds methods once initialization done successfully.
        
        public override void InitializeAdNetwork() { }

        // Start Loading banner, intestitial and rewarded ads. Please call this method once initialization is done.
        public void StartLoadingAds()
        {
            RequestBannerAds();
            RequestInterstitial();
            RequestRewarded();
        }

        // Request banner ad.
        public void RequestBannerAds() { }

        //Request interstitial ad.
        public void RequestInterstitial() { }

        //Request rewarded ad.
        public void RequestRewarded() { }

        // Shows banner ad.
        public override void ShowBanner() { }

        //Hide banner ad.
        public override void HideBanner() { }

        // Checks if interstitial ad is available.
        public override bool IsInterstitialAvailable() { return false; }

        // Shows interstitial ad if loaded.
        public override void ShowInterstitial() { }

        // Checks if rewarded ad available.
        public override bool IsRewardedAvailable() { return false; }

        // Shows rewarded ad if loaded.
        public override void ShowRewarded() { }


        // You must register ad delegate and forwaerd the ad status responce to ad manager. you can follow below commented code as reference.
        // NOTE : Callback implementation may be diffetrent then below sample code as per the ad network, so kindly review that part from the 
        // selected ad network's offical documentation.

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable() {
            
            //TODO : Banner Ad CallBacks
            
            /*YOUR_AD_SDK.OnBannerAdLoadedEvent += OnBannerAdLoadedEvent;
            YOUR_AD_SDK.OnBannerAdLoadFailedEvent += OnBannerAdLoadFailedEvent; */

            //TODO : Interstitial Ad Callbacks.
            
            /*YOUR_AD_SDK.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
            YOUR_AD_SDK.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
            YOUR_AD_SDK.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
            YOUR_AD_SDK.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent; */

            //TODO : Rewarded Ad CallBacks
            
            /*YOUR_AD_SDK.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
            YOUR_AD_SDK.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
            YOUR_AD_SDK.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
            YOUR_AD_SDK.OnRewardedAdFailedToDisplayEvent += RewardedAdFailedToDisplayEvent;
            YOUR_AD_SDK.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent; */
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable() {
            //TODO : Banner Ad CallBacks
            
            /*YOUR_AD_SDK.OnBannerAdLoadedEvent -= OnBannerAdLoadedEvent;
            YOUR_AD_SDK.OnBannerAdLoadFailedEvent -= OnBannerAdLoadFailedEvent; */

            //TODO : Interstitial Ad Callbacks.
            
            /*YOUR_AD_SDK.OnInterstitialLoadedEvent -= OnInterstitialLoadedEvent;
            YOUR_AD_SDK.OnInterstitialLoadFailedEvent -= OnInterstitialFailedEvent;
            YOUR_AD_SDK.OnInterstitialAdFailedToDisplayEvent -= InterstitialFailedToDisplayEvent;
            YOUR_AD_SDK.OnInterstitialHiddenEvent -= OnInterstitialDismissedEvent; */

            //TODO : Rewarded Ad CallBacks
            
            /*YOUR_AD_SDK.OnRewardedAdLoadedEvent -= OnRewardedAdLoadedEvent;
            YOUR_AD_SDK.OnRewardedAdHiddenEvent -= OnRewardedAdDismissedEvent;
            YOUR_AD_SDK.OnRewardedAdLoadFailedEvent -= OnRewardedAdFailedEvent;
            YOUR_AD_SDK.OnRewardedAdFailedToDisplayEvent -= RewardedAdFailedToDisplayEvent;
            YOUR_AD_SDK.OnRewardedAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent; */
        }

        #region Your ad network Banner Callbacks
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

        #region Your ad network Interstitial Callbacks
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

        #region Your ad network Interstitial Callbacks
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

        private void OnRewardedAdReceivedRewardEvent(string adUnitId) {
           AdManager.Instance.OnRewardedAdRewarded();
        }
        #endregion
    }
}
