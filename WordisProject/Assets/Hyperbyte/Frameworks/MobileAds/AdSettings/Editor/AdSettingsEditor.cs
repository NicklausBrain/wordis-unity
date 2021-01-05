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

using Hyperbyte.Ads;
using Hyperbyte.Utils;
using UnityEditor;
using UnityEngine;

namespace Hyperbyte
{
    [CustomEditor(typeof(AdSettings))]
    public class AdSettingsEditor : CustomInspectorHelper
    {
        private bool cache = false;
        AdSettings adSettings;
        GUIStyle labelStyle;

        #region AdNetworkSelectionSpecific
        string[] allNetworks;
        int selectedAdNetwork = 0;
        #endregion

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (!cache)
            {
                adSettings = (AdSettings)target;

                labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.fontStyle = FontStyle.Bold;

                allNetworks = EnumUtils.GetValuesAsStringArray<AdNetworkSelection>();
                selectedAdNetwork = ((int)adSettings.selectedAdNetwork);

                cache = true;
            }

            EditorGUILayout.Space();
            DrawConsentSettings();
            EditorGUILayout.Space();
            DrawAdNetworkSelectionSettings();

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(adSettings);
        }

        void DrawConsentSettings()
        {
            bool isExpanded = BeginFoldoutBox("Consent Settings");
            int indentLevel = EditorGUI.indentLevel;

            if (isExpanded)
            {
                EditorGUI.indentLevel = 1;

                EditorGUILayout.BeginHorizontal();
                labelStyle.fontStyle = FontStyle.Bold;
                EditorGUILayout.LabelField("Enable User Consent : ", labelStyle, GUILayout.MaxWidth(140));
                adSettings.enableConsent = EditorGUILayout.Toggle(adSettings.enableConsent);
                EditorGUILayout.EndHorizontal();

                if (adSettings.enableConsent)
                {
                    GUILayout.Space(5);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Consent Required  : ", EditorStyles.boldLabel, GUILayout.MaxWidth(250));
                    adSettings.consentSelection = (ConsentSelection)EditorGUILayout.EnumPopup(adSettings.consentSelection);
                    EditorGUILayout.EndHorizontal();

                    switch (adSettings.consentSelection)
                    {
                        case ConsentSelection.NotRequired:
                            EditorGUILayout.HelpBox("All ads will be served as personalized ads, please make sure you verified GDPR guidelines. Users won't be asked to give consent.", MessageType.Warning, true);
                            break;
                        #if HB_EEA_CONSENT
                        case ConsentSelection.ReqiuredOnlyInEEA:
                            EditorGUILayout.HelpBox("Ads will be initialized and served only after consent status verification. This selection will be applied in EEA only. Users in EEA only will be asked to give consent.", MessageType.Info, true);
                            break;
                        #endif
                        case ConsentSelection.RequiredAll:
                            EditorGUILayout.HelpBox("Ads will be initialized and served only after consent status verification. This selection will be applied to all users. All users will be asked to give consent.", MessageType.Info, true);
                            break;
                    }
                }
                GUILayout.Space(5);

                if (!adSettings.enableConsent)
                {
                    EditorGUILayout.HelpBox("Consent Settings is disabled. Users won't be asked to give consent to serve personalised ads. Please make sure you verified GDPR guidelines to provide best user experience.", MessageType.Warning, true);
                }

                if (adSettings.enableConsent && adSettings.consentSelection != ConsentSelection.NotRequired)
                {
                    labelStyle.fontStyle = FontStyle.Bold;
                    EditorGUILayout.HelpBox("You can adjust content of consent dialogue from Main Scene -> Hierarchy -> Canvas-Popups -> ConsentSetting. Also check for localized content file if you are using localization.", MessageType.None);
                }
            }
            EndBox();
            EditorGUI.indentLevel = indentLevel;
        }

        void DrawAdNetworkSelectionSettings()
        {
            bool isExpanded = BeginFoldoutBox("Ad Network Settings");
            int indentLevel = EditorGUI.indentLevel;

            if (isExpanded)
            {
                EditorGUI.indentLevel = 1;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Ads Enabled : ", labelStyle, GUILayout.MaxWidth(150));
                adSettings.adsEnabled = EditorGUILayout.Toggle(adSettings.adsEnabled);
                EditorGUILayout.EndHorizontal();

                GUI.enabled = adSettings.adsEnabled;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Active Ad Network : ", labelStyle, GUILayout.MaxWidth(150));
                labelStyle.fontStyle = FontStyle.Normal;
                selectedAdNetwork = EditorGUILayout.Popup(selectedAdNetwork, allNetworks);
                adSettings.selectedAdNetwork = (AdNetworkSelection)selectedAdNetwork;
                EditorGUILayout.EndHorizontal();


                GUILayout.Space(5);
                BeginBox();
                labelStyle.fontStyle = FontStyle.Bold;
                EditorGUILayout.LabelField("Banner Ads Settings : ", labelStyle, GUILayout.MaxWidth(250));
                GUILayout.Space(2);

                labelStyle.fontStyle = FontStyle.Normal;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Banner Ads Enabled : ", labelStyle, GUILayout.MaxWidth(250));
                adSettings.bannerAdsEnabled = EditorGUILayout.Toggle(adSettings.bannerAdsEnabled);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Show Banner On Load : ", labelStyle, GUILayout.MaxWidth(250));
                adSettings.showBannerOnLoad = EditorGUILayout.Toggle(adSettings.showBannerOnLoad);
                EditorGUILayout.EndHorizontal();
                DrawLine();

                labelStyle.fontStyle = FontStyle.Bold;
                EditorGUILayout.LabelField("Interstitial Ads Settings : ", labelStyle, GUILayout.MaxWidth(250));
                GUILayout.Space(2);

                labelStyle.fontStyle = FontStyle.Normal;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Interstitial Ads Enabled : ", labelStyle, GUILayout.MaxWidth(250));
                adSettings.interstitialAdsEnabled = EditorGUILayout.Toggle(adSettings.interstitialAdsEnabled);
                EditorGUILayout.EndHorizontal();

                if(adSettings.interstitialAdsEnabled) 
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Show Interstitial On Game Over : ", labelStyle, GUILayout.MaxWidth(250));
                    adSettings.showInterstitialOnGameOver = EditorGUILayout.Toggle(adSettings.showInterstitialOnGameOver);
                    EditorGUILayout.EndHorizontal();

                    labelStyle.fontStyle = FontStyle.Normal;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Min Delay B/W 2 Interstitial (Sec.) : ", labelStyle, GUILayout.MaxWidth(250));
                    adSettings.delayBetweenIngerstitials = EditorGUILayout.IntField(adSettings.delayBetweenIngerstitials);
                    EditorGUILayout.EndHorizontal();
                }
                DrawLine();

                labelStyle.fontStyle = FontStyle.Bold;
                EditorGUILayout.LabelField("Rewarded Ads Settings : ", labelStyle, GUILayout.MaxWidth(250));
                GUILayout.Space(2);

                labelStyle.fontStyle = FontStyle.Normal;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Rewarded Ads Enabled : ", labelStyle, GUILayout.MaxWidth(250));
                adSettings.rewardedAdsEnabled = EditorGUILayout.Toggle(adSettings.rewardedAdsEnabled);
                EditorGUILayout.EndHorizontal();
                EndBox();

                GUI.backgroundColor = Color.grey;
                GUIStyle style2 = new GUIStyle(GUI.skin.button);
                style2.richText = true;
                style2.fontStyle = FontStyle.Bold;
                style2.fixedHeight = 30;

                if (adSettings.selectedAdNetwork != AdNetworkSelection.Custom)
                {
                    string buttonText = "Configure " + ((adSettings.adsEnabled) ? ("<color=yellow>" + adSettings.selectedAdNetwork.ToString() + "</color>") : adSettings.selectedAdNetwork.ToString()) + " Settings";

                    if (GUILayout.Button(new GUIContent(buttonText), style2))
                    {
                        OpenAdNetworkConfig(adSettings.selectedAdNetwork);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("You selected custom ad network, you can write your custom ad network code in CustomAdManager.cs", MessageType.Info);
                }

                GUI.backgroundColor = Color.white;
                GUILayout.Space(5);
                GUI.enabled = true;
            }
            EndBox();
            EditorGUI.indentLevel = indentLevel;
        }

        void OpenAdNetworkConfig(AdNetworkSelection selection)
        {
            string assetPath = "Assets/Hyperbyte/Resources/AdNetworkSettings";
            string assetName = "";

            if (!System.IO.Directory.Exists(assetPath))
            {
                System.IO.Directory.CreateDirectory(assetPath);
            }

            switch (selection)
            {
                case AdNetworkSelection.UnityAds:
                    UnityAdsSettings unityAdsAsset;
                    assetName = "UnityAdsSettings.asset";

                    if (System.IO.File.Exists(assetPath + "/" + assetName))
                    {
                        unityAdsAsset = (UnityAdsSettings)(Resources.Load("AdNetworkSettings/" + System.IO.Path.GetFileNameWithoutExtension(assetName)));
                    }
                    else
                    {
                        unityAdsAsset = ScriptableObject.CreateInstance<UnityAdsSettings>();
                        AssetDatabase.CreateAsset(unityAdsAsset, assetPath + "/" + assetName);
                        AssetDatabase.SaveAssets();
                    }

                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = unityAdsAsset;
                    break;

                case AdNetworkSelection.GoogleMobileAds:
                    GoogleMobileAdsSettings googleAdsAsset;
                    assetName = "GoogleMobileAdsSettings.asset";

                    if (System.IO.File.Exists(assetPath + "/" + assetName))
                    {
                        googleAdsAsset = (GoogleMobileAdsSettings)(Resources.Load("AdNetworkSettings/" + System.IO.Path.GetFileNameWithoutExtension(assetName)));
                    }
                    else
                    {
                        googleAdsAsset = ScriptableObject.CreateInstance<GoogleMobileAdsSettings>();
                        AssetDatabase.CreateAsset(googleAdsAsset, assetPath + "/" + assetName);
                        AssetDatabase.SaveAssets();
                    }

                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = googleAdsAsset;
                    break;

                case AdNetworkSelection.IronSource:
                    IronSourceAdsSettings ironsourceAdsAsset;
                    assetName = "IronSourceAdsSettings.asset";

                    if (System.IO.File.Exists(assetPath + "/" + assetName))
                    {
                        ironsourceAdsAsset = (IronSourceAdsSettings)(Resources.Load("AdNetworkSettings/" + System.IO.Path.GetFileNameWithoutExtension(assetName)));
                    }
                    else
                    {
                        ironsourceAdsAsset = ScriptableObject.CreateInstance<IronSourceAdsSettings>();
                        AssetDatabase.CreateAsset(ironsourceAdsAsset, assetPath + "/" + assetName);
                        AssetDatabase.SaveAssets();
                    }

                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = ironsourceAdsAsset;
                    break;

                case AdNetworkSelection.AppLovinMax:
                    AppLovinAdsSettings applovinAdsAsset;
                    assetName = "AppLovinAdsSettings.asset";

                    if (System.IO.File.Exists(assetPath + "/" + assetName))
                    {
                        applovinAdsAsset = (AppLovinAdsSettings)(Resources.Load("AdNetworkSettings/" + System.IO.Path.GetFileNameWithoutExtension(assetName)));
                    }
                    else
                    {
                        applovinAdsAsset = ScriptableObject.CreateInstance<AppLovinAdsSettings>();
                        AssetDatabase.CreateAsset(applovinAdsAsset, assetPath + "/" + assetName);
                        AssetDatabase.SaveAssets();
                    }

                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = applovinAdsAsset;
                    break;
            }
        }
    }
}