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

using UnityEditor;
using UnityEngine;
using Hyperbyte.Utils;

namespace Hyperbyte.Ads
{	
	[CustomEditor(typeof(GoogleMobileAdsSettings))]
	public class GoogleMobileAdsSettingsEditor : CustomInspectorHelper 
	{
		private bool cache = false;
		GoogleMobileAdsSettings adSettings;
		GUIStyle labelStyle;

		SDKInfo thisSDKInfo = new SDKInfo("Admob", "GoogleMobileAds", "HB_ADMOB");
		static bool thisSdkExists = false;
		static bool thisDefineSymbolExists = false;

		private SerializedProperty appId_android;
		private SerializedProperty bannerId_android;
		private SerializedProperty interstitialId_android;
		private SerializedProperty rewardedId_android;
		
		private SerializedProperty appId_iOS;
		private SerializedProperty bannerId_iOS;
		private SerializedProperty interstitialId_iOS;
		private SerializedProperty rewardedId_iOS;

		private SerializedProperty tagForChildTreatment;
		private SerializedProperty bannerAdPosition;
		private SerializedProperty bannerBGColor;

		string[] bannerAdPositions;
		int selectedBannerPosition = 0;

		public override void OnInspectorGUI()
    	{
			serializedObject.Update();

			if (!cache) 
			{
				adSettings = (GoogleMobileAdsSettings)target;				
				AutoDetectSdk();
				labelStyle = new GUIStyle(GUI.skin.label);
				labelStyle.fontStyle = FontStyle.Bold;

				appId_android = serializedObject.FindProperty("appId_android");
				bannerId_android = serializedObject.FindProperty("bannerId_android");
				interstitialId_android = serializedObject.FindProperty("interstitialId_android");
				rewardedId_android = serializedObject.FindProperty("rewardedId_android");
		
				appId_iOS = serializedObject.FindProperty("appId_iOS");
				bannerId_iOS = serializedObject.FindProperty("bannerId_iOS");
				interstitialId_iOS = serializedObject.FindProperty("interstitialId_iOS");
				rewardedId_iOS = serializedObject.FindProperty("rewardedId_iOS");

				tagForChildTreatment = serializedObject.FindProperty("tagForChildTreatment");
				bannerAdPosition = serializedObject.FindProperty("bannerAdPosition");
				bannerBGColor = serializedObject.FindProperty("bannerBGColor");

				bannerAdPositions = EnumUtils.GetValuesAsStringArray<BannerAdPosition>();
				selectedBannerPosition = bannerAdPosition.intValue;

				cache = true;
			}

			EditorGUILayout.Space();

			if(tagForChildTreatment != null) {
				DrawAdsSettings();
			} 
			EditorGUILayout.Space();
			DrawScriptingDefineSymbol();

			serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(adSettings);
		}

		void DrawAdsSettings() 
		{
			labelStyle.fontStyle = FontStyle.Bold;
			
			BeginBox();
			GUILayout.Space(2);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Google Mobile Ads Settings", labelStyle);
			EditorGUILayout.EndHorizontal();
			DrawLine();

			EditorGUILayout.LabelField("Android Settings : ",labelStyle);
			GUILayout.Space(3);
			labelStyle.fontStyle = FontStyle.Normal;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("App Id : ",labelStyle, GUILayout.MaxWidth(140));
			appId_android.stringValue = EditorGUILayout.TextField(appId_android.stringValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Banner Ad Unit Id : ",labelStyle, GUILayout.MaxWidth(140));
			bannerId_android.stringValue = EditorGUILayout.TextField(bannerId_android.stringValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Interstitial Ad Unit Id : ",labelStyle, GUILayout.MaxWidth(140));
			interstitialId_android.stringValue = EditorGUILayout.TextField(interstitialId_android.stringValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Rewarded Ad Unit Id : ",labelStyle, GUILayout.MaxWidth(140));
			rewardedId_android.stringValue = EditorGUILayout.TextField(rewardedId_android.stringValue);
			EditorGUILayout.EndHorizontal();

			labelStyle.fontStyle = FontStyle.Bold;
			GUILayout.Space(10);
			EditorGUILayout.LabelField("iOS Settings : ",labelStyle);
			GUILayout.Space(3);
			labelStyle.fontStyle = FontStyle.Normal;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("App Id : ",labelStyle, GUILayout.MaxWidth(140));
			appId_iOS.stringValue = EditorGUILayout.TextField(appId_iOS.stringValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Banner Ad Unit Id : ",labelStyle, GUILayout.MaxWidth(140));
			bannerId_iOS.stringValue = EditorGUILayout.TextField(bannerId_iOS.stringValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Interstitial Ad Unit Id : ",labelStyle, GUILayout.MaxWidth(140));
			interstitialId_iOS.stringValue = EditorGUILayout.TextField(interstitialId_iOS.stringValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Rewarded Ad Unit Id : ",labelStyle, GUILayout.MaxWidth(140));
			rewardedId_iOS.stringValue = EditorGUILayout.TextField(rewardedId_iOS.stringValue);
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(2);
			DrawLine();
			GUILayout.Space(2);

			labelStyle.fontStyle = FontStyle.Bold;
			EditorGUILayout.LabelField("Common Settings : ",labelStyle);
			GUILayout.Space(3);
			labelStyle.fontStyle = FontStyle.Normal;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Tag For Child Treatment : ",labelStyle, GUILayout.MaxWidth(140));
			tagForChildTreatment.boolValue = EditorGUILayout.Toggle(tagForChildTreatment.boolValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Banner Ad Placement : ",labelStyle, GUILayout.MaxWidth(140));
			selectedBannerPosition =  EditorGUILayout.Popup(selectedBannerPosition, bannerAdPositions);
			bannerAdPosition.intValue = selectedBannerPosition;
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Banner Ad BG Color : ",labelStyle, GUILayout.MaxWidth(140));
			bannerBGColor.stringValue = EditorGUILayout.TextField(bannerBGColor.stringValue);
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(5);

			EndBox();
		}

		void DrawScriptingDefineSymbol() 
		{
			GUI.enabled = !EditorApplication.isCompiling;
			BeginBox();
			GUI.backgroundColor = Color.grey;
			GUIStyle style2 = new GUIStyle(GUI.skin.button);
			style2.richText = true;
			style2.fontStyle = FontStyle.Bold;
			style2.fixedHeight = 30;
	
			if(thisSdkExists) {
				if(!thisDefineSymbolExists) {
					if (GUILayout.Button(new GUIContent("Add Scripting Define Symbol"), style2)) {
						AddScriptingDefineSymbol(thisSDKInfo.sdkName, thisSDKInfo.sdkScriptingDefineSymbol, true);
					}
					EditorGUILayout.HelpBox("Scripting symbol is required in order to serve ads with selected sdk. ", MessageType.None, true);
				} else {
					if (GUILayout.Button(new GUIContent("Remove Scripting Define Symbol"), style2)){
						RemoveScriptingDefineSymbol(thisSDKInfo.sdkName, thisSDKInfo.sdkScriptingDefineSymbol, true);
					}
				}
			} else {
				DrawSDKDetection();
			}
			GUI.backgroundColor = Color.white;
			EndBox();
		}

		void DrawSDKDetection()
		{
			BeginBox();
			EditorGUILayout.BeginVertical();
			EditorGUILayout.HelpBox("Google Mobile Ads SDK not detected, Please import to serve ads with Google Mobile Ads SDK. ", MessageType.Warning, true);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("You can download Google Mobile Ads SDK from",GUILayout.MaxWidth(260));

			Color fontColor = labelStyle.normal.textColor;
			labelStyle.fontStyle = FontStyle.Bold;
			labelStyle.normal.textColor = Color.red;

			if(GUILayout.Button("Here.", labelStyle)) {
				Application.OpenURL("https://github.com/googleads/googleads-mobile-plugins/releases/latest");
			}
			labelStyle.normal.textColor = fontColor;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			EndBox();
		}

		void AutoDetectSdk() 
		{
			thisSdkExists = NamespaceUtils.CheckNamespacesExists(thisSDKInfo.sdkNameSpace);
			thisDefineSymbolExists = ScriptingDefineSymbolEditor.HasDefineSymbol(thisSDKInfo.sdkScriptingDefineSymbol);
			VerifySDKImportInfo(thisSDKInfo, thisSdkExists, thisDefineSymbolExists);
		}

		static void VerifySDKImportInfo(SDKInfo currentSdkInfo, bool sdkExists, bool defineSymbolExists) {
			if(sdkExists) {
				if(!defineSymbolExists) {
					AddScriptingDefineSymbol(currentSdkInfo.sdkName, currentSdkInfo.sdkScriptingDefineSymbol);
				}
			} else {
				RemoveScriptingDefineSymbol(currentSdkInfo.sdkName, currentSdkInfo.sdkScriptingDefineSymbol);
			}
		}

		static void AddScriptingDefineSymbol( string sdkName, string symbol, bool addForced = false) {
			if((!EditorPrefs.HasKey("userRemoved_"+sdkName)) || addForced) {
				ScriptingDefineSymbolEditor.AddScriptingDefineSymbol(symbol);
				thisDefineSymbolExists = true;
			}
			
		}

		static void RemoveScriptingDefineSymbol(string sdkName, string symbol, bool forced = false) {
			if(ScriptingDefineSymbolEditor.HasDefineSymbol(symbol)) {
				ScriptingDefineSymbolEditor.RemoveScriptingDefineSymbol(symbol);
			}

			if(forced) {
				EditorPrefs.SetInt("userRemoved_"+sdkName, 1);
			}
			thisDefineSymbolExists = false;
		}
	}
}