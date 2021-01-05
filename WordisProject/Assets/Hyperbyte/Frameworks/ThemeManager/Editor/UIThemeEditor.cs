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

using Hyperbyte;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UITheme))]
public class UIThemeSettingsEditor : CustomInspectorHelper
{
    private bool cache = false;
    private SerializedProperty colorTags;
    private SerializedProperty spriteTags;
    UITheme uiTheme;

    public override void OnInspectorGUI()
    {
		serializedObject.Update();

		if (!cache)
		{
            uiTheme = (UITheme)target;
            colorTags = serializedObject.FindProperty("colorTags");
            spriteTags = serializedObject.FindProperty("spriteTags");
            cache = true;
        }

        if(colorTags != null) {
            ShowColorTagsArray(colorTags);
            ShowImageTagsArray(spriteTags);
        }

		serializedObject.ApplyModifiedProperties();

		// if (GUI.changed) {
			EditorUtility.SetDirty(uiTheme);
		//}
    }

   

    public void ShowColorTagsArray(SerializedProperty list, string label = "ColorThemeTags ")
    {
        bool isExpanded = BeginFoldoutBox("Color Tags");

        if (isExpanded)
        {
            if (list.arraySize > 0)
            {
                //BeginBox();
                int indentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 1;

                //GUILayout.Space(5);
                for (int i = 0; i < list.arraySize; i++)
                {
                    BeginBox();
                    EditorGUILayout.BeginHorizontal();

                    SerializedProperty tagName = list.GetArrayElementAtIndex(i).FindPropertyRelative("tagName");
                    bool isOpened = BeginSimpleFoldoutBox(tagName.stringValue, "ColorTag : " + (i + 1));
                    if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(20f)))
                    {
                        list.InsertArrayElementAtIndex(i);
                    }

                    if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20f)))
                    {
                        list.DeleteArrayElementAtIndex(i);
                        return;
                    }
                    EditorGUILayout.EndHorizontal();

                    if (isOpened)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUI.indentLevel = indentLevel + 1;
                        EditorGUILayout.BeginVertical();

                        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                        labelStyle.fontStyle = FontStyle.Bold;

                        DrawLine();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Tag Name : ", labelStyle, GUILayout.MaxWidth(120));
                        tagName.stringValue = EditorGUILayout.TextField(tagName.stringValue);
                        EditorGUILayout.EndHorizontal();

                        //GUILayout.Space(5);

                        EditorGUILayout.BeginHorizontal();
                        SerializedProperty tagColor = list.GetArrayElementAtIndex(i).FindPropertyRelative("tagColor");
                        EditorGUILayout.LabelField("Tag Color : ", labelStyle, GUILayout.MaxWidth(120));
                        tagColor.colorValue = EditorGUILayout.ColorField(tagColor.colorValue);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }
                    EndBox();
                    //GUILayout.Space(5);
                }
                EditorGUI.indentLevel = indentLevel;
                //EndBox();
            }

            //GUILayout.Space(5);
            GUI.backgroundColor = Color.grey;
            GUIStyle style2 = new GUIStyle(GUI.skin.button);
            style2.richText = true;
            style2.fixedHeight = 20;

            if (GUILayout.Button(new GUIContent("<b>Add Color Tag</b>"), style2))
            {
                list.arraySize += 1;
            }
            GUI.backgroundColor = Color.white;
            //GUILayout.Space(5);
        }
        EndBox();
    }

    public void ShowImageTagsArray(SerializedProperty list, string label = "ImageThemeTags ")
    {
        bool isExpanded = BeginFoldoutBox("Sprite Tags");

        if (isExpanded)
        {
            if (list.arraySize > 0)
            {
                // BeginBox();
                int indentLevel = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 1;

                //GUILayout.Space(5);
                for (int i = 0; i < list.arraySize; i++)
                {
                    BeginBox();
                    EditorGUILayout.BeginHorizontal();

                    SerializedProperty tagName = list.GetArrayElementAtIndex(i).FindPropertyRelative("tagName");
                    bool isOpened = BeginSimpleFoldoutBox(tagName.stringValue, "ImageTag : " + (i + 1));
                    if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(20f)))
                    {
                        list.InsertArrayElementAtIndex(i);
                    }

                    if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20f)))
                    {
                        list.DeleteArrayElementAtIndex(i);
                        return;
                    }
                    EditorGUILayout.EndHorizontal();

                    if (isOpened)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUI.indentLevel = indentLevel + 1;
                        EditorGUILayout.BeginVertical();

                        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                        labelStyle.fontStyle = FontStyle.Bold;

                        DrawLine();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Tag Name : ", labelStyle, GUILayout.MaxWidth(120));
                        tagName.stringValue = EditorGUILayout.TextField(tagName.stringValue);
                        EditorGUILayout.EndHorizontal();

                        //GUILayout.Space(5);

                        EditorGUILayout.BeginHorizontal();
                        SerializedProperty tagSprite = list.GetArrayElementAtIndex(i).FindPropertyRelative("tagSprite");
                        EditorGUILayout.LabelField("Tag Sprite : ", labelStyle, GUILayout.MaxWidth(120));
                        //EditorGUILayout.ObjectField(tagSprite, typeof(Sprite));
                        tagSprite.objectReferenceValue = EditorGUILayout.ObjectField(tagSprite.objectReferenceValue, typeof(Sprite), false);
                        EditorGUILayout.EndHorizontal();


                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }
                    EndBox();
                    //GUILayout.Space(5);
                }
                EditorGUI.indentLevel = indentLevel;
                // EndBox();
            }

            //GUILayout.Space(5);
            GUI.backgroundColor = Color.grey;
            GUIStyle style2 = new GUIStyle(GUI.skin.button);
            style2.richText = true;
            style2.fixedHeight = 20;

            if (GUILayout.Button(new GUIContent("<b>Add Image Tag</b>"), style2))
            {
                list.arraySize += 1;
            }
            GUI.backgroundColor = Color.white;
            //GUILayout.Space(5);
        }
        EndBox();
    }
}

