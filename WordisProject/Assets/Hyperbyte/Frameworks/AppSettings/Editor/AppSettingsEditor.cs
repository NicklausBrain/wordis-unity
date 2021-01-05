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

namespace Hyperbyte
{	
	[CustomEditor(typeof(AppSettings))]
	public class AppSettingsEditor : CustomInspectorHelper 
	{
		private bool cache = false;
		AppSettings appSettings;
		private SerializedProperty dailyRewardsSettings;
		GUIStyle labelStyle;

		public override void OnInspectorGUI()
    	{
			serializedObject.Update();

			if (!cache)
			{
				appSettings = (AppSettings)target;
				dailyRewardsSettings = serializedObject.FindProperty("dailyRewardsSettings");
				
				labelStyle = new GUIStyle(GUI.skin.label);
				labelStyle.fontStyle = FontStyle.Bold;

				cache = true;
			}

			EditorGUILayout.Space();
			DrawCommonSettings();
			EditorGUILayout.Space();
			DrawReviewAppSettings();
			EditorGUILayout.Space();
			DrawDailyRewardsSettings();
			EditorGUILayout.Space();
			DrawOtherSettings();
			EditorGUILayout.Space();
			DrawMiscSettings();

			serializedObject.ApplyModifiedProperties();
			EditorUtility.SetDirty(appSettings);
		}

		void DrawCommonSettings() 
		{
			labelStyle.fontStyle = FontStyle.Bold;

			bool isExpanded = BeginFoldoutBox("Common Settings");
			int indentLevel = EditorGUI.indentLevel;
			
			if(isExpanded) {
				EditorGUI.indentLevel = 1;
				
				#if UNITY_ANDROID
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Current Store : ",  labelStyle,GUILayout.MaxWidth(140));
				appSettings.currentAndroidStore = EditorGUILayout.Popup(appSettings.currentAndroidStore, new string[] {"GOOGLE", "AMAZON", "SAMSUNG"});
				EditorGUILayout.EndHorizontal();
				#endif

				labelStyle.fontStyle = FontStyle.Normal;

				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Privacy Policy URL : ",  labelStyle,GUILayout.MaxWidth(140));
				appSettings.privacyPolicyURL = EditorGUILayout.TextField(appSettings.privacyPolicyURL);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Enable Support URL : ",  labelStyle,GUILayout.MaxWidth(140));
				appSettings.enableSupportURL = EditorGUILayout.Toggle(appSettings.enableSupportURL);
				EditorGUILayout.EndHorizontal();

				if(appSettings.enableSupportURL) 
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Support URL : ",  labelStyle,GUILayout.MaxWidth(140));
					appSettings.supportURL = EditorGUILayout.TextField(appSettings.supportURL);
					EditorGUILayout.EndHorizontal();
				}

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Apple ID (iOS Only) : ",  labelStyle,GUILayout.MaxWidth(140));
				appSettings.appleID = EditorGUILayout.TextField(appSettings.appleID);
				EditorGUILayout.EndHorizontal();
			}
			EndBox();
			EditorGUI.indentLevel = indentLevel;
		} 

		void DrawReviewAppSettings() 
		{
			labelStyle.fontStyle = FontStyle.Bold;

			bool isExpanded = BeginFoldoutBox("Review App Settings (Rate App)");
			int indentLevel = EditorGUI.indentLevel;

			if(isExpanded) 
			{
				EditorGUI.indentLevel = 1;
				labelStyle.fontStyle = FontStyle.Bold;
				
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Show Rate Popup On Launch : ",  labelStyle,GUILayout.MaxWidth(250));
				appSettings.showReviewPopupOnLaunch = EditorGUILayout.Toggle(appSettings.showReviewPopupOnLaunch);
				EditorGUILayout.EndHorizontal();

				labelStyle.fontStyle = FontStyle.Normal;
				if(appSettings.showReviewPopupOnLaunch) 
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Rate Popup On App Launch Count : ",  labelStyle,GUILayout.MaxWidth(250));
					appSettings.reviewPopupAppLaunchCount = EditorGUILayout.TextField(appSettings.reviewPopupAppLaunchCount);
					EditorGUILayout.EndHorizontal();
				}

				labelStyle.fontStyle = FontStyle.Bold;

				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Show Rate Popup On Game Over : ",  labelStyle,GUILayout.MaxWidth(250));
				appSettings.showReviewPopupOnGameOver = EditorGUILayout.Toggle(appSettings.showReviewPopupOnGameOver);
				EditorGUILayout.EndHorizontal();

				labelStyle.fontStyle = FontStyle.Normal;
				if(appSettings.showReviewPopupOnGameOver) 
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Rate Popup On Game Over Count : ",  labelStyle,GUILayout.MaxWidth(250));
					appSettings.reviewPopupGameOverCount = EditorGUILayout.TextField(appSettings.reviewPopupGameOverCount);
					EditorGUILayout.EndHorizontal();
				}

				EditorGUILayout.Space();
				labelStyle.fontStyle = FontStyle.Bold;
				EditorGUILayout.LabelField("Review Popup Setting: ",  labelStyle,GUILayout.MaxWidth(200));

				labelStyle.fontStyle = FontStyle.Normal;

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Minimum Stars To Navigate To Store : ",  labelStyle,GUILayout.MaxWidth(250));
				appSettings.minRatingToNavigateToStore = EditorGUILayout.FloatField(appSettings.minRatingToNavigateToStore);
				appSettings.minRatingToNavigateToStore = Mathf.Clamp(appSettings.minRatingToNavigateToStore, 0,5);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Show Apple Store Review Popup On iOS: ",  labelStyle,GUILayout.MaxWidth(250));
				appSettings.showAppleStoreReviewPopupOniOS = EditorGUILayout.Toggle(appSettings.showAppleStoreReviewPopupOniOS);
				EditorGUILayout.EndHorizontal();


				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Never Show Review Popup If Rated: ",  labelStyle,GUILayout.MaxWidth(250));
				appSettings.neverShowReviewPopupIfRated = EditorGUILayout.Toggle(appSettings.neverShowReviewPopupIfRated);
				EditorGUILayout.EndHorizontal();

				#if UNITY_ANDROID
				labelStyle.fontStyle = FontStyle.Bold;
				switch(appSettings.currentAndroidStore) {
					//Google
					case 0 :
					break;
					
					//Amazon
					case 1:
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Amazon Review URL: ",  labelStyle,GUILayout.MaxWidth(250));
					appSettings.amazonReviewURL = EditorGUILayout.TextField(appSettings.amazonReviewURL);
					EditorGUILayout.EndHorizontal();
					break;
					
					//Samsung
					case 2:
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Samsung Review URL: ",  labelStyle,GUILayout.MaxWidth(250));
					appSettings.amazonReviewURL = EditorGUILayout.TextField(appSettings.samsungReviewURL);
					EditorGUILayout.EndHorizontal();
					break;
					
				}
				labelStyle.fontStyle = FontStyle.Normal;
				#endif

				EditorGUILayout.Space();
				EditorGUILayout.HelpBox("App Launch Count is the number of session of game. Sessio count will increase on each app launch. Please review SessionManager.cs for more details. Game Over Count is count of total number of times game over being called.",MessageType.Info);
			}
			EndBox();
			EditorGUI.indentLevel = indentLevel;
		} 

		void DrawDailyRewardsSettings() 
		{
			if(dailyRewardsSettings != null) {
				labelStyle.fontStyle = FontStyle.Bold;

				bool isExpanded = BeginFoldoutBox("Daily Rewards Settings");
				int indentLevel = EditorGUI.indentLevel;

				if(isExpanded) 
				{
					EditorGUI.indentLevel = 1;
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Daily Rewards Enabled : ",  labelStyle,GUILayout.MaxWidth(250));
					appSettings.useDailyRewards = EditorGUILayout.Toggle(appSettings.useDailyRewards);
					EditorGUILayout.EndHorizontal();

					if(appSettings.useDailyRewards) {
						DrawDailyRewardsSetup();
					}
				}
				EndBox();
				EditorGUI.indentLevel = indentLevel;
			} 
		}

		void DrawDailyRewardsSetup() 
		{
			SerializedProperty allDayRewards = dailyRewardsSettings.FindPropertyRelative("allDayRewards");
			
			labelStyle.fontStyle = FontStyle.Bold;

			if(allDayRewards.arraySize > 0) {
				BeginBox();
				for(int i = 0; i < allDayRewards.arraySize; i++) 
				{
					BeginBox();
					EditorGUILayout.BeginHorizontal();
					
					bool isOpened = BeginSimpleFoldoutBox("Day : "+(i+1));
					if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(20f))) {
						allDayRewards.InsertArrayElementAtIndex(i);
					}

					if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20f))) {
						allDayRewards.DeleteArrayElementAtIndex(i);
						//Debug.Log(allDayRewards.arraySize);
						return;
					}
					EditorGUILayout.EndHorizontal();

					if(isOpened) {
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Reward Amount : ", GUILayout.MaxWidth(200));
						allDayRewards.GetArrayElementAtIndex(i).intValue = EditorGUILayout.IntField(allDayRewards.GetArrayElementAtIndex(i).intValue);
						EditorGUILayout.EndHorizontal();
					}
					EndBox();
				}
				EndBox();
			}

			EditorGUI.indentLevel = 0;
			EditorGUILayout.HelpBox("All rewards are given in GEMS currency.",MessageType.Info);

			GUI.backgroundColor = Color.grey;
        	GUIStyle style2 = new GUIStyle(GUI.skin.button);
        	style2.richText = true;
        	style2.fixedHeight = 30;
        
			if (GUILayout.Button(new GUIContent("<b>Add Element</b>"), style2)){
				allDayRewards.arraySize += 1;
			}
        	GUI.backgroundColor = Color.white;
			GUILayout.Space(5);
		}

		void DrawMiscSettings() 
		{
			labelStyle.fontStyle = FontStyle.Bold;

			bool isExpanded = BeginFoldoutBox("Misc. Settings");
			int indentLevel = EditorGUI.indentLevel;

			if(isExpanded) 
			{
				EditorGUI.indentLevel = 1;
				labelStyle.fontStyle = FontStyle.Normal;
				
				GUILayout.Space(2);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Initial Gems Amount : ",  labelStyle,GUILayout.MaxWidth(300));
				appSettings.defaultGemsAmount = EditorGUILayout.IntField(appSettings.defaultGemsAmount);
				EditorGUILayout.EndHorizontal();
				
				GUILayout.Space(2);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Watch Video Reward (GEMS) : ",  labelStyle,GUILayout.MaxWidth(300));
				appSettings.watchVideoRewardAmount = EditorGUILayout.IntField(appSettings.watchVideoRewardAmount);
				EditorGUILayout.EndHorizontal();

				GUILayout.Space(2);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Rescue Cost (GEMS) : ",  labelStyle,GUILayout.MaxWidth(300));
				appSettings.rescueGameGemsAmount = EditorGUILayout.IntField(appSettings.rescueGameGemsAmount);
				EditorGUILayout.EndHorizontal();
			}
			EndBox();
			EditorGUI.indentLevel = indentLevel;
		} 

		void DrawOtherSettings() 
		{
			labelStyle.fontStyle = FontStyle.Bold;
		
			bool isExpanded = BeginFoldoutBox("Vibration Settings");
			int indentLevel = EditorGUI.indentLevel;

			if(isExpanded) {
				EditorGUI.indentLevel = 1;

				// EditorGUILayout.LabelField("Vibration Settings : ",  labelStyle,GUILayout.MaxWidth(250));
				EditorGUILayout.HelpBox("Vibrations will use Haptic Feedback on iOS if supported by hardware. For android, app will use standard vibrations, Android SDK 26 and above will support amplitude.", MessageType.Info);

				labelStyle.fontStyle = FontStyle.Normal;
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Vibration Enabled : ",  labelStyle,GUILayout.MaxWidth(250));
				appSettings.enableVibrations = EditorGUILayout.Toggle(appSettings.enableVibrations);
				EditorGUILayout.EndHorizontal();
			}
			EndBox();
		}
	}
}