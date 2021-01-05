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
using UnityEditor;
using System.IO;

namespace Hyperbyte
{
    [CustomEditor(typeof(UIThemeSettings))]
    public class UIThemeSettingsEditor : CustomInspectorHelper
    {
        private bool cache = false;
        private SerializedProperty allThemeSettings;
        UIThemeSettings uiThemeSettings;

        GUIStyle labelStyle;
        GUIStyle inputStyle;

        string[] allThemes;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (!cache)
            {
                uiThemeSettings = (UIThemeSettings)target;
                allThemeSettings = serializedObject.FindProperty("allThemeConfigs");

                labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.fontStyle = FontStyle.Bold;

                inputStyle = new GUIStyle(GUI.skin.textField);
                inputStyle.fontStyle = FontStyle.Bold;
                inputStyle.alignment = TextAnchor.MiddleCenter;
                RefreshAllThemes();
                cache = true;
            }

            if (allThemeSettings != null)
            {
                ShowArrayProperty(allThemeSettings);
            }

            serializedObject.ApplyModifiedProperties();

            // if (GUI.changed)
            {
                EditorUtility.SetDirty(uiThemeSettings);
            }
        }

        public void ShowArrayProperty(SerializedProperty list, string label = "theme ")
        {
            BeginBox();
            labelStyle.fontStyle = FontStyle.Bold;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.Width(5));
            EditorGUILayout.LabelField("Use UI Theme : ", labelStyle, GUILayout.MaxWidth(200));
            EditorGUILayout.LabelField("", GUILayout.MinWidth(0));
            uiThemeSettings.useUIThemes = EditorGUILayout.Toggle(uiThemeSettings.useUIThemes, GUILayout.Width(30));
            EditorGUILayout.LabelField("", GUILayout.Width(5));
            EditorGUILayout.EndHorizontal();
            DrawLine();

            GUI.enabled = uiThemeSettings.useUIThemes;

            bool isExpanded = BeginFoldoutBox("All Themes");

            if (isExpanded)
            {
                if (list.arraySize > 0)
                {
                    int indentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 1;

                    GUILayout.Space(5);
                    for (int i = 0; i < list.arraySize; i++)
                    {
                        BeginBox();
                        EditorGUILayout.BeginHorizontal();
                        labelStyle.fontStyle = FontStyle.Normal;

                        SerializedProperty themeName = list.GetArrayElementAtIndex(i).FindPropertyRelative("themeName");
                        bool isOpened = BeginSimpleFoldoutBox(themeName.stringValue, "Theme : " + (i + 1));

                        if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(20f)))
                        {
                            list.InsertArrayElementAtIndex(i);
                        }

                        if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20f)))
                        {
                            list.DeleteArrayElementAtIndex(i);
                            return;
                        }
                        EditorGUILayout.LabelField("", labelStyle, GUILayout.Width(5));
                        EditorGUILayout.EndHorizontal();

                        if (isOpened)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUI.indentLevel = indentLevel + 1;
                            EditorGUILayout.BeginVertical();
                            DrawLine();

                            GUILayout.Space(5);
                            SerializedProperty isEnabled = list.GetArrayElementAtIndex(i).FindPropertyRelative("isEnabled");
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Theme Enabled : ", labelStyle, GUILayout.MaxWidth(120));
                            EditorGUILayout.LabelField("", labelStyle, GUILayout.MinWidth(0));
                            isEnabled.boolValue = EditorGUILayout.Toggle(isEnabled.boolValue, GUILayout.Width(30));
                            EditorGUILayout.LabelField("", labelStyle, GUILayout.Width(5));
                            EditorGUILayout.EndHorizontal();

                            SerializedProperty uiTheme = null;

                            if (isEnabled.boolValue)
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField("Theme Name : ", labelStyle, GUILayout.MaxWidth(100));
                                EditorGUILayout.LabelField("", labelStyle, GUILayout.MinWidth(0));
                                themeName.stringValue = EditorGUILayout.TextField(themeName.stringValue, inputStyle, GUILayout.Width(150));
                                EditorGUILayout.LabelField("", labelStyle, GUILayout.Width(5));
                                EditorGUILayout.EndHorizontal();

                                SerializedProperty demoSprite = list.GetArrayElementAtIndex(i).FindPropertyRelative("demoSprite");
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField("Demo Sprite : ", labelStyle, GUILayout.MaxWidth(100));
                                EditorGUILayout.LabelField("", labelStyle, GUILayout.MinWidth(0));
                                demoSprite.objectReferenceValue = EditorGUILayout.ObjectField(demoSprite.objectReferenceValue, typeof(Sprite), false, GUILayout.Width(150));
                                EditorGUILayout.LabelField("", labelStyle, GUILayout.Width(5));
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField("UI Theme : ", labelStyle, GUILayout.MaxWidth(100));
                                EditorGUILayout.LabelField("", labelStyle, GUILayout.MinWidth(0));
                                uiTheme = list.GetArrayElementAtIndex(i).FindPropertyRelative("uiTheme");
                                uiTheme.objectReferenceValue = EditorGUILayout.ObjectField(uiTheme.objectReferenceValue, typeof(UITheme), false, GUILayout.Width(150));
                                EditorGUILayout.LabelField("", labelStyle, GUILayout.Width(5));
                                EditorGUILayout.EndHorizontal();

                                SerializedProperty defaultStatus = list.GetArrayElementAtIndex(i).FindPropertyRelative("defaultStatus");
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField("Default State : ", labelStyle, GUILayout.MaxWidth(100));
                                EditorGUILayout.LabelField("", labelStyle, GUILayout.MinWidth(0));
                                defaultStatus.intValue = EditorGUILayout.Popup (defaultStatus.intValue, new string[]{"LOCKED", "UNLOCKED"}, GUILayout.Width(150));
                                EditorGUILayout.LabelField("", labelStyle, GUILayout.Width(5));
                                EditorGUILayout.EndHorizontal();

                                if(defaultStatus.intValue == 0) {
                                    SerializedProperty unlockCost = list.GetArrayElementAtIndex(i).FindPropertyRelative("unlockCost");
                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField("Unlock Cost (GEMS) : ", labelStyle, GUILayout.MaxWidth(150));
                                    EditorGUILayout.LabelField("", labelStyle, GUILayout.MinWidth(0));
                                    unlockCost.intValue = EditorGUILayout.IntField (unlockCost.intValue, inputStyle, GUILayout.Width(150));
                                    EditorGUILayout.LabelField("", labelStyle, GUILayout.Width(5));
                                    EditorGUILayout.EndHorizontal();
                                }
                                
                            }

                        
                            DrawLine();
                            GUI.backgroundColor = Color.grey;
                            GUIStyle style = new GUIStyle(GUI.skin.button);
                            style.richText = true;
                            style.fixedHeight = 24;

                            if (GUILayout.Button(new GUIContent("<b>EDIT THEME</b>"), style))
                            {
                                Selection.activeObject = uiTheme.objectReferenceValue;
                                EditorGUIUtility.PingObject(uiTheme.objectReferenceValue);
                                EditorUtility.FocusProjectWindow();
                            }
                        
                            GUI.backgroundColor = Color.white;

                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();
                            GUILayout.Space(5);
                        }
                        EndBox();
                    }
                    EditorGUI.indentLevel = indentLevel;
                }
            }
            EndBox();

            BeginBox();
            if (allThemes != null)
            {
                GUILayout.Space(5);
                EditorGUILayout.BeginHorizontal();
                labelStyle.fontStyle = FontStyle.Bold;
                EditorGUILayout.LabelField("", GUILayout.Width(5));
                EditorGUILayout.LabelField("Default Theme : ", labelStyle, GUILayout.MaxWidth(140));
                EditorGUILayout.LabelField("", GUILayout.MinWidth(0));
                uiThemeSettings.defaultTheme = EditorGUILayout.Popup(uiThemeSettings.defaultTheme, allThemes, GUILayout.Width(120));
                EditorGUILayout.LabelField("", GUILayout.Width(5));
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5);
            }

            EndBox();

            GUI.backgroundColor = Color.grey;
            GUIStyle style2 = new GUIStyle(GUI.skin.button);
            style2.richText = true;
            style2.fixedHeight = 30;

            if (GUILayout.Button(new GUIContent("<b>Add New Theme</b>"), style2))
            {
                list.InsertArrayElementAtIndex(list.arraySize);
            }

            if (GUILayout.Button(new GUIContent("<b>Create New Theme</b>"), style2))
            {
                CreateNewTheme();
            }

            if (GUILayout.Button(new GUIContent("<b>Refresh</b>"), style2))
            {
                RefreshAllThemes();
            }
            GUI.backgroundColor = Color.white;
            GUI.enabled = true;
            EndBox();
        }

        void CreateNewTheme()
        {
            string assetPath = "Assets/Hyperbyte/Resources/UIThemes";
            string folderPath = Application.dataPath + "/" + "Hyperbyte/Resources/UIThemes";

            int existingThemes = new DirectoryInfo(folderPath).GetFiles("*.asset").Length;
            string assetName = "Theme-" + existingThemes + ".asset";

            UITheme asset;

            if (!System.IO.Directory.Exists(assetPath))
            {
                System.IO.Directory.CreateDirectory(assetPath);
            }

            if (System.IO.File.Exists(assetPath + "/" + assetName))
            {
                asset = (UITheme)(Resources.Load(System.IO.Path.GetFileNameWithoutExtension(assetName)));
            }
            else
            {
                asset = ScriptableObject.CreateInstance<UITheme>();
                AssetDatabase.CreateAsset(asset, assetPath + "/" + assetName);
                AssetDatabase.SaveAssets();
            }

            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(assetPath + "/" + assetName, typeof(UnityEngine.Object));
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        void RefreshAllThemes()
        {
            if (uiThemeSettings.allThemeConfigs != null)
            {
                allThemes = new string[uiThemeSettings.allThemeConfigs.Length];

                int index = 0;
                foreach (ThemeConfig theme in uiThemeSettings.allThemeConfigs)
                {
                    if (theme.isEnabled && theme.themeName != "" && theme.uiTheme != null && theme.defaultStatus == 1)
                    {
                        allThemes[index] = theme.themeName;
                        index++;
                    }
                }
            }
        }
    }
}
