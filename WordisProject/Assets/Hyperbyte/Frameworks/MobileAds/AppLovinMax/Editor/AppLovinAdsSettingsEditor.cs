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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hyperbyte.Utils;
using UnityEditor;
using UnityEngine;

namespace Hyperbyte.Ads
{	
	[CustomEditor(typeof(AppLovinAdsSettings))]
	public class AppLovinAdsSettingsEditor : CustomInspectorHelper 
	{
		private bool cache = false;
		AppLovinAdsSettings adSettings;
		GUIStyle labelStyle;

		SDKInfo thisSDKInfo = new SDKInfo("AppLovinMax", "AppLovinMax", "HB_APPLOVINMAX");
		static bool thisSdkExists = false;
		static bool thisDefineSymbolExists = false;

		SerializedProperty sdkKey_android;
		SerializedProperty bannerId_android;
		SerializedProperty interstitialId_android;
		SerializedProperty rewardedId_android;
		
		SerializedProperty sdkKey_iOS;
		SerializedProperty bannerId_iOS;
		SerializedProperty interstitialId_iOS;
		SerializedProperty rewardedId_iOS;

		SerializedProperty bannerAdPosition;
		SerializedProperty bannerBGColor;

		string[] bannerAdPositions;
		int selectedBannerPosition = 0;

		public override void OnInspectorGUI()
    	{
			serializedObject.Update();

			if (!cache) {
				adSettings = (AppLovinAdsSettings)target;				
				AutoDetectSdk();
				labelStyle = new GUIStyle(GUI.skin.label);
				labelStyle.fontStyle = FontStyle.Bold;

				sdkKey_android = serializedObject.FindProperty("sdkKey_android");
				bannerId_android = serializedObject.FindProperty("bannerId_android");
				interstitialId_android = serializedObject.FindProperty("interstitialId_android");
				rewardedId_android = serializedObject.FindProperty("rewardedId_android");
		
				sdkKey_iOS = serializedObject.FindProperty("sdkKey_iOS");
				bannerId_iOS = serializedObject.FindProperty("bannerId_iOS");
				interstitialId_iOS = serializedObject.FindProperty("interstitialId_iOS");
				rewardedId_iOS = serializedObject.FindProperty("rewardedId_iOS");

				bannerAdPosition = serializedObject.FindProperty("bannerAdPosition");
				bannerBGColor = serializedObject.FindProperty("bannerBGColor");

				bannerAdPositions = EnumUtils.GetValuesAsStringArray<BannerAdPosition>();
				selectedBannerPosition = bannerAdPosition.intValue;

				cache = true;
			}

			EditorGUILayout.Space();
			if(bannerBGColor != null) {
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
			EditorGUILayout.LabelField("AppLovin Max Ads Settings", labelStyle);
			EditorGUILayout.EndHorizontal();
			DrawLine();

			EditorGUILayout.LabelField("Android Settings : ",labelStyle);
			GUILayout.Space(3);
			labelStyle.fontStyle = FontStyle.Normal;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("SDK Key : ",labelStyle, GUILayout.MaxWidth(140));
			sdkKey_android.stringValue = EditorGUILayout.TextField(sdkKey_android.stringValue);
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
			EditorGUILayout.LabelField("SDK Key : ",labelStyle, GUILayout.MaxWidth(140));
			sdkKey_iOS.stringValue = EditorGUILayout.TextField(sdkKey_iOS.stringValue);
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

		void DrawScriptingDefineSymbol() {
			
			GUI.enabled = !EditorApplication.isCompiling;
			BeginBox();
			GUI.backgroundColor = Color.grey;
			GUIStyle style2 = new GUIStyle(GUI.skin.button);
			style2.richText = true;
			style2.fontStyle = FontStyle.Bold;
			style2.fixedHeight = 30;
	
			if(thisSdkExists) {
				if(!thisDefineSymbolExists) {
					if (GUILayout.Button(new GUIContent("Add Scripting Define Symbol"), style2)){
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
			EditorGUILayout.HelpBox("AppLovin Max sdk not detected, Please import to serve ads with AppLovin Max. ", MessageType.Warning, true);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("You can download AppLovin Max SDK from",GUILayout.MaxWidth(235));

			Color fontColor = labelStyle.normal.textColor;
			labelStyle.fontStyle = FontStyle.Bold;
			labelStyle.normal.textColor = Color.red;

			if(GUILayout.Button("Here.", labelStyle)) {
				Application.OpenURL("https://www.applovin.com/max/");
			}
			labelStyle.normal.textColor = fontColor;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
			EndBox();
		}

		void AutoDetectSdk() 
		{
			thisSdkExists = (System.IO.Directory.Exists(Application.dataPath + "/MaxSdk"));
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