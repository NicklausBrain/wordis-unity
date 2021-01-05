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
using Hyperbyte;
using UnityEditor;
using UnityEngine;

namespace Hyperbyte.Localization
{
    [CustomEditor(typeof(LocalizationSettings))]
    public class LocalizationSettingsEditor : CustomInspectorHelper
    {
        private bool cache = false;
        private SerializedProperty useLocalization;
        private SerializedProperty localizeToSystemDetectedLanguage;
        private SerializedProperty localizedLanguages;
        private SerializedProperty defaultLangauge;
        LocalizationSettings localizationSettings;

        GUIStyle labelStyle;
        GUIStyle inputStyle;

        string[] allLanguages;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (!cache)
            {
                localizationSettings = (LocalizationSettings)target;
                useLocalization = serializedObject.FindProperty("useLocalization");
                localizedLanguages = serializedObject.FindProperty("localizedLanguages");
                localizeToSystemDetectedLanguage = serializedObject.FindProperty("localizeToSystemDetectedLanguage");
                defaultLangauge = serializedObject.FindProperty("defaultLangauge");

                labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.fontStyle = FontStyle.Bold;

                inputStyle = new GUIStyle(GUI.skin.textField);
                inputStyle.fontStyle = FontStyle.Bold;
                inputStyle.alignment = TextAnchor.MiddleCenter;

                RefreshAllLanaguges();
                cache = true;        
            }

            if(localizedLanguages != null) {
                ShowArrayProperty(localizedLanguages);
            }

            serializedObject.ApplyModifiedProperties();

            // if (GUI.changed)
            {
                EditorUtility.SetDirty(localizationSettings);
            }
        }

        public void ShowArrayProperty(SerializedProperty list, string label = "Product ")
        {
            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontStyle = FontStyle.Bold;
            
            BeginBox();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(5));
            EditorGUILayout.LabelField("Use Localization? : ",labelStyle,  GUILayout.MaxWidth(100));
            EditorGUILayout.LabelField("", GUILayout.MinWidth(0));
            useLocalization.boolValue = EditorGUILayout.Toggle(useLocalization.boolValue, GUILayout.Width(30));
            EditorGUILayout.LabelField("", GUILayout.Width(5));
            EditorGUILayout.EndHorizontal();
            DrawLine();

            GUI.enabled = useLocalization.boolValue;
            bool isExpanded = BeginFoldoutBox("All Languages");

            if(isExpanded) 
            {
                if(list.arraySize > 0) 
                {
                    int indentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 1;

                    GUILayout.Space(5);
                    for(int i = 0; i < list.arraySize; i++) 
                    {
                        BeginBox();
                        EditorGUILayout.BeginHorizontal();
                        
                        SerializedProperty languageName = list.GetArrayElementAtIndex(i).FindPropertyRelative("languageName");
                        bool isOpened = BeginSimpleFoldoutBox(languageName.stringValue, "Language : "+ (i+1));
                        if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(20f))) {
                            list.InsertArrayElementAtIndex(i);
                        }

                        if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20f))) {
                            list.DeleteArrayElementAtIndex(i);
                            return;
                        }
                        EditorGUILayout.EndHorizontal();

                        if(isOpened) 
                        {
                            EditorGUI.indentLevel = indentLevel + 1;
                            EditorGUILayout.BeginVertical();

                            DrawLine();
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Language Name : ",  labelStyle,GUILayout.MaxWidth(150));
                            EditorGUILayout.LabelField("", GUILayout.MinWidth(0));
                            languageName.stringValue = EditorGUILayout.TextField(languageName.stringValue, inputStyle, GUILayout.Width(100));
                            EditorGUILayout.LabelField("", GUILayout.Width(5));
                            EditorGUILayout.EndHorizontal();

                            GUILayout.Space(5);
                            SerializedProperty isLanguageEnabled = list.GetArrayElementAtIndex(i).FindPropertyRelative("isLanguageEnabled");
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Language Enabled : ",  labelStyle,GUILayout.MaxWidth(220));
                            EditorGUILayout.LabelField("", GUILayout.MinWidth(0));
                            isLanguageEnabled.boolValue = EditorGUILayout.Toggle(isLanguageEnabled.boolValue, GUILayout.Width(30));
                            EditorGUILayout.LabelField("", GUILayout.Width(5));
                            EditorGUILayout.EndHorizontal();

                            labelStyle.fontStyle = FontStyle.Normal;
                            if(isLanguageEnabled.boolValue) 
                            {
                                int subIndentLevel = EditorGUI.indentLevel;
                                EditorGUI.indentLevel = (subIndentLevel+1);
                                
                                EditorGUILayout.BeginHorizontal();
                                SerializedProperty langaugeDisplayName = list.GetArrayElementAtIndex(i).FindPropertyRelative("langaugeDisplayName");
                                EditorGUILayout.LabelField("Langauge Display Name : ",  labelStyle,GUILayout.MaxWidth(150));
                                EditorGUILayout.LabelField("", GUILayout.MinWidth(0));
                                langaugeDisplayName.stringValue = EditorGUILayout.TextField(langaugeDisplayName.stringValue, inputStyle, GUILayout.Width(100));
                                EditorGUILayout.LabelField("", GUILayout.Width(5));
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                SerializedProperty languageCode = list.GetArrayElementAtIndex(i).FindPropertyRelative("languageCode");
                                EditorGUILayout.LabelField("Language Code : ",  labelStyle,GUILayout.MaxWidth(150));
                                EditorGUILayout.LabelField("", GUILayout.MinWidth(0));
                                languageCode.stringValue = EditorGUILayout.TextField(languageCode.stringValue, inputStyle, GUILayout.Width(100));
                                EditorGUILayout.LabelField("", GUILayout.Width(5));
                                EditorGUILayout.EndHorizontal();

                                GUILayout.Space(5);

                                EditorGUILayout.BeginHorizontal();
                                SerializedProperty languageFlag = list.GetArrayElementAtIndex(i).FindPropertyRelative("languageFlag");
                                EditorGUILayout.LabelField("Language Flag : ", labelStyle, GUILayout.MaxWidth(120));
                                EditorGUILayout.LabelField("", GUILayout.MinWidth(0));
                                languageFlag.objectReferenceValue = EditorGUILayout.ObjectField(languageFlag.objectReferenceValue, typeof(Sprite), false, GUILayout.Width(160));
                                EditorGUILayout.LabelField("", GUILayout.Width(5));
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                SerializedProperty localizedTextFile = list.GetArrayElementAtIndex(i).FindPropertyRelative("localizedTextFile");
                                EditorGUILayout.LabelField("Localized XML : ", labelStyle, GUILayout.MaxWidth(120));
                                EditorGUILayout.LabelField("", GUILayout.MinWidth(0));
                                localizedTextFile.objectReferenceValue = EditorGUILayout.ObjectField(localizedTextFile.objectReferenceValue, typeof(TextAsset), false, GUILayout.Width(160));
                                EditorGUILayout.LabelField("", GUILayout.Width(5));
                                EditorGUILayout.EndHorizontal();
                            }
                            GUILayout.Space(5);
                            EditorGUILayout.EndVertical();
                        }
                        EndBox();
                        GUILayout.Space(5);
                    }
                    EditorGUI.indentLevel = indentLevel;
                }
            }
            EndBox();

            GUILayout.Space(10);
            

            labelStyle.fontStyle = FontStyle.Bold;

            BeginBox();
            if(allLanguages != null) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Default Language : ",labelStyle,  GUILayout.MaxWidth(400));
                defaultLangauge.intValue = EditorGUILayout.Popup(defaultLangauge.intValue, allLanguages);
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            labelStyle.fontStyle = FontStyle.Normal;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Localize To System Detected Language? : ",labelStyle,  GUILayout.MaxWidth(350));
            localizeToSystemDetectedLanguage.boolValue = EditorGUILayout.Toggle(localizeToSystemDetectedLanguage.boolValue);
            EditorGUILayout.EndHorizontal();

            EndBox();

            GUILayout.Space(10);

            BeginBox();
            GUIStyle style2 = new GUIStyle(GUI.skin.button);
            style2.fixedHeight = 30;
            style2.richText = true;

            GUI.backgroundColor = Color.grey;
            if (GUILayout.Button(new GUIContent("<b>Add Language</b>"), style2))
            {
                list.arraySize += 1;
                RefreshAllLanaguges();
            }

            if (GUILayout.Button(new GUIContent("<b>Refresh</b>"), style2)) {
                RefreshAllLanaguges();
            }
            GUI.backgroundColor = Color.white;
            EndBox();
            GUI.enabled = true;
            EndBox();
        }

        void RefreshAllLanaguges()
        {
            if(localizationSettings.localizedLanguages != null) {
                allLanguages = new string[localizationSettings.localizedLanguages.Length];

                int index = 0;
                foreach (LocalizedLanguage lang in localizationSettings.localizedLanguages) {
                    if (lang.isLanguageEnabled && lang.languageCode != "" && lang.localizedTextFile != null) {
                        allLanguages[index] = lang.languageName;
                        index++;
                    }
                }
            }
        }
    }
}

