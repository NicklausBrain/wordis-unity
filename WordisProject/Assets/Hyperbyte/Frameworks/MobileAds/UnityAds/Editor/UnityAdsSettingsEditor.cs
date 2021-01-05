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

using Hyperbyte.Utils;
using UnityEditor;
using UnityEngine;

namespace Hyperbyte.Ads
{	
	[CustomEditor(typeof(UnityAdsSettings))]
	public class UnityAdsSettingsEditor : CustomInspectorHelper 
	{
		private bool cache = false;
		UnityAdsSettings adSettings;
		GUIStyle labelStyle;

		SDKInfo thisSDKInfo = new SDKInfo("UnityAds", "UnityEngine.Monetization", "HB_UNITYADS");
		static bool thisSdkExists = false;
		static bool thisDefineSymbolExists = false;

		SerializedProperty gameId_android;
		SerializedProperty bannerPlacement_android;
		SerializedProperty interstitialPlacement_android;
		SerializedProperty rewardedPlacement_android;
		
		SerializedProperty gameId_iOS;
		SerializedProperty bannerPlacement_iOS;
		SerializedProperty interstitialPlacement_iOS;
		SerializedProperty rewardedPlacement_iOS;

		SerializedProperty enableTestAds;
		SerializedProperty bannerAdPosition;

		string[] bannerAdPositions;
		int selectedBannerPosition = 0;

		public override void OnInspectorGUI()
    	{
			serializedObject.Update();
			if (!cache) {
				adSettings = (UnityAdsSettings)target;				
				AutoDetectSdk();
				labelStyle = new GUIStyle(GUI.skin.label);
				labelStyle.fontStyle = FontStyle.Bold;

				gameId_android = serializedObject.FindProperty("gameId_android");
				bannerPlacement_android = serializedObject.FindProperty("bannerPlacement_android");
				interstitialPlacement_android = serializedObject.FindProperty("interstitialPlacement_android");
				rewardedPlacement_android = serializedObject.FindProperty("rewardedPlacement_android");
		
				gameId_iOS = serializedObject.FindProperty("gameId_iOS");
				bannerPlacement_iOS = serializedObject.FindProperty("bannerPlacement_iOS");
				interstitialPlacement_iOS = serializedObject.FindProperty("interstitialPlacement_iOS");
				rewardedPlacement_iOS = serializedObject.FindProperty("rewardedPlacement_iOS");

				enableTestAds = serializedObject.FindProperty("enableTestAds");
				bannerAdPosition = serializedObject.FindProperty("bannerAdPosition");

				bannerAdPositions = EnumUtils.GetValuesAsStringArray<BannerAdPosition>();
				selectedBannerPosition = bannerAdPosition.intValue;

				cache = true;
			}

			if(cache) 
			{
				EditorGUILayout.Space();
				
				if(gameId_android != null) {
					DrawAdsSettings();
				}
				EditorGUILayout.Space();
				DrawScriptingDefineSymbol();
			}

			serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(adSettings);
		}

		void DrawAdsSettings() 
		{
			labelStyle.fontStyle = FontStyle.Bold;
			
			BeginBox();
			GUILayout.Space(2);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Unity Ads Settings", labelStyle);
			EditorGUILayout.EndHorizontal();
			DrawLine();

			EditorGUILayout.LabelField("Android Settings : ",labelStyle);
			GUILayout.Space(3);
			labelStyle.fontStyle = FontStyle.Normal;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Game Id : ",labelStyle, GUILayout.MaxWidth(140));
			gameId_android.stringValue = EditorGUILayout.TextField(gameId_android.stringValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Banner Placement : ",labelStyle, GUILayout.MaxWidth(140));
			bannerPlacement_android.stringValue = EditorGUILayout.TextField(bannerPlacement_android.stringValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Interstitial Placement : ",labelStyle, GUILayout.MaxWidth(140));
			interstitialPlacement_android.stringValue = EditorGUILayout.TextField(interstitialPlacement_android.stringValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Rewarded Placement : ",labelStyle, GUILayout.MaxWidth(140));
			rewardedPlacement_android.stringValue = EditorGUILayout.TextField(rewardedPlacement_android.stringValue);
			EditorGUILayout.EndHorizontal();

			labelStyle.fontStyle = FontStyle.Bold;
			GUILayout.Space(10);
			EditorGUILayout.LabelField("iOS Settings : ",labelStyle);
			GUILayout.Space(3);
			labelStyle.fontStyle = FontStyle.Normal;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Game Id : ",labelStyle, GUILayout.MaxWidth(140));
			gameId_iOS.stringValue = EditorGUILayout.TextField(gameId_iOS.stringValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Banner Placement : ",labelStyle, GUILayout.MaxWidth(140));
			bannerPlacement_iOS.stringValue = EditorGUILayout.TextField(bannerPlacement_iOS.stringValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Interstitial Placement : ",labelStyle, GUILayout.MaxWidth(140));
			interstitialPlacement_iOS.stringValue = EditorGUILayout.TextField(interstitialPlacement_iOS.stringValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Rewarded Placement : ",labelStyle, GUILayout.MaxWidth(140));
			rewardedPlacement_iOS.stringValue = EditorGUILayout.TextField(rewardedPlacement_iOS.stringValue);
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(2);
			DrawLine();
			GUILayout.Space(2);

			labelStyle.fontStyle = FontStyle.Bold;
			EditorGUILayout.LabelField("Common Settings : ",labelStyle);
			GUILayout.Space(3);
			labelStyle.fontStyle = FontStyle.Normal;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Enable Test Ads : ",labelStyle, GUILayout.MaxWidth(140));
			enableTestAds.boolValue = EditorGUILayout.Toggle(enableTestAds.boolValue);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Banner Ad Placement : ",labelStyle, GUILayout.MaxWidth(140));
			selectedBannerPosition =  EditorGUILayout.Popup(selectedBannerPosition, bannerAdPositions);
			bannerAdPosition.intValue = selectedBannerPosition;
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
			EditorGUILayout.HelpBox("Unity Monetization SDK not detected, Please import to serve ads with Unity Ads. ", MessageType.Warning, true);
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("You can download Unity Ads SDK from",GUILayout.MaxWidth(210));

			Color fontColor = labelStyle.normal.textColor;
			labelStyle.fontStyle = FontStyle.Bold;
			labelStyle.normal.textColor = Color.red;

			if(GUILayout.Button("Here.", labelStyle)) {
				Application.OpenURL("https://assetstore.unity.com/packages/add-ons/services/unity-monetization-3-2-0-66123");
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