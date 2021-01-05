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

using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace Hyperbyte.Ads
{
    /// <summary>
    /// AdManager is singleton class component to control ads during game. Typically this class will initialize, load and shows ads with selected ad network.
    /// Ad settings can be configured from ad manager.
    /// </summary>
    public class AdManager : Singleton<AdManager>
    {
        bool hasInitialised = false;
        AdNetworkSelection selectedAdNetwork;
        MonoBehaviour adBehaviour;
        Type adComponentType;
        string rewardedVideoTag = "";

        // Ad manager event for tracking all events of ad status. 
        public static event Action OnSdkInitialisedEvent;
        public static event Action OnBannerLoadedEvent;
        public static event Action<string> OnBannerLoadFailedEvent;

        public static event Action OnInterstitialLoadedEvent;
        public static event Action<string> OnInterstitialLoadFailedEvent;
        public static event Action OnInterstitialShownEvent;
        public static event Action OnInterstitialClosedEvent;

        public static event Action OnRewardedLoadedEvent;
        public static event Action<string> OnRewardedLoadFailedEvent;
        public static event Action OnRewardedShownEvent;
        public static event Action OnRewardedClosedEvent;
        public static event Action<string> OnRewardedAdRewardedEvent;

        [System.NonSerialized] public AdSettings adSettings;

        // Time when last interstitial was shown, to handle min delay between 2 interstitials.
        float lastInterstitialShownTime = 0;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            adSettings = (AdSettings)Resources.Load("AdSettings");
            VerifyConsent();
        }

        // Starts loading ad upon verification of consent.
        void OnConsentVerificationCompleted()
        {
            consentVerified = PlayerPrefs.HasKey("consentVerified");
            consentAllowed = (PlayerPrefs.GetInt("consentAllowed", 0) != 0);

            if (OnConsentVerifiedEvent != null)
            {
                OnConsentVerifiedEvent.Invoke(consentVerified, consentAllowed);
            }
            if (adSettings.adsEnabled)
            {
                Initialise();
            }
        }

        #region  Initialise
        /// <summary>
        /// Initializes the selected ad network.
        /// </summary>
        void Initialise()
        {
            if (!hasInitialised)
            {
                selectedAdNetwork = adSettings.selectedAdNetwork;
                AddRequiredComponent(selectedAdNetwork);
                Invoke("InitializeAdNetwork", 0.1F);
                hasInitialised = true;
            }
        }

        // adBehaviour is dynamic typed mono behaviour and will aquire type of selectd ad network script component.
        void AddRequiredComponent(AdNetworkSelection _selectedAdNetwork)
        {
            if (adSettings.adsEnabled)
            {
                switch (_selectedAdNetwork)
                {
                    case AdNetworkSelection.UnityAds:
                        adBehaviour = gameObject.AddComponent<UnityAdsManager>();
                        adComponentType = typeof(UnityAdsManager);
                        break;

                    case AdNetworkSelection.GoogleMobileAds:
                        adBehaviour = gameObject.AddComponent<GoogleMobileAdsManager>();
                        adComponentType = typeof(GoogleMobileAdsManager);
                        break;

                    case AdNetworkSelection.AppLovinMax:
                        adBehaviour = gameObject.AddComponent<AppLovinAdManager>();
                        adComponentType = typeof(AppLovinAdManager);
                        break;

                    case AdNetworkSelection.IronSource:
                        adBehaviour = gameObject.AddComponent<IronSourceAdManager>();
                        adComponentType = typeof(IronSourceAdManager);
                        break;

                    case AdNetworkSelection.Custom:
                        adBehaviour = gameObject.AddComponent<CustomAdManager>();
                        adComponentType = typeof(CustomAdManager);
                        break;
                }
            }
        }
        #endregion

        #region Component Method Calls
        /// <summary>
        /// Initializes the selected ad network.
        /// </summary>
        void InitializeAdNetwork()
        {
            if (adSettings.adsEnabled)
            {
                MethodInfo info = adComponentType.GetMethod("InitializeAdNetwork", BindingFlags.Public | BindingFlags.Instance);
                info.Invoke(adBehaviour, null);
            }
        }

        /// <summary>
        /// Shows banner ad.
        /// </summary>
        public void ShowBanner()
        {
            if (adSettings.adsEnabled && adSettings.bannerAdsEnabled)
            {
                MethodInfo info = adComponentType.GetMethod("ShowBanner", BindingFlags.Public | BindingFlags.Instance);
                info.Invoke(adBehaviour, null);
            }
        }

        /// <summary>
        /// Hides banner ad if loaded.
        /// </summary>
        public void HideBanner()
        {
            if(adSettings.adsEnabled && adSettings.bannerAdsEnabled) {
                MethodInfo info = adComponentType.GetMethod("HideBanner", BindingFlags.Public | BindingFlags.Instance);
                info.Invoke(adBehaviour, null);
            }
        }

        /// <summary>
        /// Checks if interstitial ad available.
        /// </summary>
        public bool IsInterstitialAvailable()
        {
            if (adSettings.adsEnabled && adSettings.interstitialAdsEnabled)
            {
                MethodInfo info = adComponentType.GetMethod("IsInterstitialAvailable", BindingFlags.Public | BindingFlags.Instance);
                object isInterstitalAvailable = info.Invoke(adBehaviour, null);
                return (bool)isInterstitalAvailable;
            }
            return false;
        }

        /// <summary>
        /// Shows interstitial ad if available.
        /// </summary>
        public void ShowInterstitial()
        {
            // Handles delay between 2 interstitial ads.
            if(lastInterstitialShownTime <= 0 || ((Time.time - lastInterstitialShownTime) >= adSettings.delayBetweenIngerstitials)) 
            {    
                if (adSettings.adsEnabled && adSettings.interstitialAdsEnabled)
                {
                    MethodInfo info = adComponentType.GetMethod("ShowInterstitial", BindingFlags.Public | BindingFlags.Instance);
                    info.Invoke(adBehaviour, null);        
                }
            }
        }

        /// <summary>
        /// Checks if rewarded ad is available.
        /// </summary>
        public bool IsRewardedAvailable()
        {
            if (adSettings.adsEnabled && adSettings.rewardedAdsEnabled)
            {
                #if UNITY_EDITOR
                return true;
                #else
				MethodInfo info = adComponentType.GetMethod("IsRewardedAvailable", BindingFlags.Public | BindingFlags.Instance);
				object isRewardedAvailable = info.Invoke(adBehaviour, null);
				return (bool) isRewardedAvailable; 
                #endif
            }
            return false;
        }

        /// <summary>
        /// Show rewarded ad if available. tag is to identify from which location rewarded ad is called.
        /// </summary>
        public void ShowRewardedWithTag(string _rewardedVideoTag)
        {
            rewardedVideoTag = _rewardedVideoTag;
            if (adSettings.adsEnabled && adSettings.rewardedAdsEnabled)
            {
                #if UNITY_EDITOR
                OnRewardedAdRewarded();
                #else
				MethodInfo info = adComponentType.GetMethod("ShowRewarded", BindingFlags.Public | BindingFlags.Instance);
				info.Invoke(adBehaviour, null);
                #endif
            }
        }
        #endregion

        #region Callback Integration
        /// Invokes event callback of varient ad status.

        public void OnSdkInitialised() { if (OnSdkInitialisedEvent != null) { OnSdkInitialisedEvent.Invoke(); } }
        public void OnBannerLoaded() { if (OnBannerLoadedEvent != null) { OnBannerLoadedEvent.Invoke(); } }
        public void OnBannerLoadFailed(string reason) { if (OnBannerLoadFailedEvent != null) { OnBannerLoadFailedEvent.Invoke(reason); } }
        public void OnInterstitialLoaded() { if (OnInterstitialLoadedEvent != null) { OnInterstitialLoadedEvent.Invoke(); } }
        public void OnInterstitialLoadFailed(string reason) { if (OnInterstitialLoadFailedEvent != null) { OnInterstitialLoadFailedEvent.Invoke(reason); } }
        public void OnInterstitialShown() { if (OnInterstitialShownEvent != null) { OnInterstitialShownEvent.Invoke(); } }
        public void OnInterstitialClosed() { lastInterstitialShownTime = Time.time; if (OnInterstitialClosedEvent != null) { OnInterstitialClosedEvent.Invoke(); } }
        public void OnRewardedLoaded() { if (OnRewardedLoadedEvent != null) { OnRewardedLoadedEvent.Invoke(); } }
        public void OnRewardedLoadFailed(string reason) { { if (OnRewardedLoadFailedEvent != null) { OnRewardedLoadFailedEvent.Invoke(reason); } } }
        public void OnRewardedShown() { if (OnRewardedShownEvent != null) { OnRewardedShownEvent.Invoke(); } }
        public void OnRewardedClosed() { if (OnRewardedClosedEvent != null) { OnRewardedClosedEvent.Invoke(); } }
        public void OnRewardedAdRewarded() { if (OnRewardedAdRewardedEvent != null) { OnRewardedAdRewardedEvent.Invoke(rewardedVideoTag); } }
        #endregion

        public static event Action<bool, bool> OnConsentVerifiedEvent;

        [System.NonSerialized] public bool consentVerified = false;
        [System.NonSerialized] public bool consentAllowed = false;

        #if HB_EEA_CONSENT
        UserLocation userLocation = UserLocation.Unknown;
        #endif
        
        bool consentCheckAttempted = false;

        #region Consent Verification
        /// <summary>
        /// Verifies consent status and take consent if not taken.
        /// </summary>
        public void VerifyConsent()
        {
            if (PlayerPrefs.HasKey("consentVerified"))
            {
                consentVerified = PlayerPrefs.HasKey("consentVerified");
                consentAllowed = (PlayerPrefs.GetInt("consentAllowed", 0) != 0);
                OnConsentVerificationCompleted();
            }
            else
            {
                if (!consentCheckAttempted)
                {
                    GetUserConsent();
                }
            }
        }

        /// <summary>
        /// If user consent is reuired then app will fetch user location and takes consent from user for personalizing ads.
        /// </summary>
        void GetUserConsent()
        {
            bool consentDialogueRequired = false;

            // Will try to get consent if enabled from ad manager settings.
            if (adSettings.enableConsent)
            {
                // Consent status will automatically be considered as allowed if ad maneger setting has consent selection as not required.
                if (adSettings.consentSelection == ConsentSelection.NotRequired)
                {
                    consentVerified = true;
                    PlayerPrefs.SetInt("consentVerified", 1);
                    PlayerPrefs.SetInt("consentAllowed", 0);
                    OnConsentVerificationCompleted();
                    return;
                }

                // Will load consent dialog if required for all.
                else if (adSettings.consentSelection == ConsentSelection.RequiredAll)
                {
                    consentDialogueRequired = true;
                }

                #if HB_EEA_CONSENT
                // If consent only required for EEA then user location will be fetched.
                else if (adSettings.consentSelection == ConsentSelection.ReqiuredOnlyInEEA)
                {
                    if (PlayerPrefs.HasKey("userLocationVerified"))
                    {
                        userLocation = (UserLocation)PlayerPrefs.GetInt("userLocation", 0);
                        if (userLocation == UserLocation.InEEA || userLocation == UserLocation.Unknown)
                        {
                            consentDialogueRequired = true;
                        }
                        else
                        {
                            PlayerPrefs.SetInt("consentVerified", 1);
                            PlayerPrefs.SetInt("consentAllowed", 1);
                            OnConsentVerificationCompleted();
                            return;
                        }
                    }
                    else
                    {
                        StartCoroutine(GetUserLocation());
                        return;
                    }
                }
                #endif
                if (consentDialogueRequired)
                {
                    UIController.Instance.ShowConsentDialogue();
                    return;
                }
            }
            else
            {
                PlayerPrefs.SetInt("consentVerified", 1);
                PlayerPrefs.SetInt("consentAllowed", 0);
                OnConsentVerificationCompleted();
            }
        }

        /// <summary>
        /// Set status of consent.
        /// </summary>
        public void SetConsentStatus(bool allowed)
        {
            PlayerPrefs.SetInt("consentVerified", 1);
            PlayerPrefs.SetInt("consentAllowed", (allowed) ? 1 : 0);

            OnConsentVerificationCompleted();
        }

        /// <summary>
        /// Fetches the user location. 
        /// </summary>
        private IEnumerator GetUserLocation()
        {
            string url = "http://adservice.google.com/getconfig/pubvendors";
            consentCheckAttempted = true;

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();
                bool isError = false;

                isError = webRequest.isNetworkError || webRequest.isHttpError;

                // isError = true;
                if (!isError)
                {
                    ConsentResponse response = JsonUtility.FromJson<ConsentResponse>(webRequest.downloadHandler.text);
                    response.is_request_in_eea_or_unknown = true;
                    PlayerPrefs.SetInt("userLocationVerified", 1);
                    PlayerPrefs.SetInt("userLocation", (!response.is_request_in_eea_or_unknown) ? 2 : 1);
                    GetUserConsent();
                }
                else
                {
                    //User Location Verification fetched error. App will serve Non Personalised Ads Untill user location gets verified.
                    OnConsentVerificationCompleted();
                }
            }
        }
        #endregion
    }
}
