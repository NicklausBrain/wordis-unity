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

namespace Hyperbyte
{
    public class CustomInspectorHelper : Editor
    {
        private Texture2D lineTexture;
        private Texture2D LineTexture
        {
            get
            {
                if (lineTexture == null)
                {
                    lineTexture = new Texture2D(1, 1);
                    lineTexture.SetPixel(0, 0, new Color(37f / 255f, 37f / 255f, 37f / 255f));
                    lineTexture.Apply();
                }

                return lineTexture;
            }
        }

        protected void DrawLine()
        {
            GUIStyle lineStyle = new GUIStyle();
            lineStyle.normal.background = LineTexture;

            GUILayout.Space(3);
            GUILayout.BeginVertical(lineStyle);
            GUILayout.Space(1);
            GUILayout.EndVertical();
            GUILayout.Space(3);
        }

        protected void BeginBox(string boxTitle = "")
        {
            GUIStyle style = new GUIStyle("HelpBox");
            style.padding.left = 0;
            style.padding.right = 0;

            GUILayout.BeginVertical(style);

            if (!string.IsNullOrEmpty(boxTitle))
            {
                DrawBoldLabel(boxTitle);

                DrawLine();
            }
        }

        protected void EndBox()
        {
            GUILayout.EndVertical();
        }

        protected void DrawBoldLabel(string text)
        {
            EditorGUILayout.LabelField(text, EditorStyles.boldLabel);
        }

        protected bool BeginFoldoutBox(string boxTitle)
        {
            GUIStyle style = new GUIStyle("HelpBox");
            style.padding.left = 15;
            style.padding.right = 0;

            GUILayout.BeginVertical(style);

            if (!string.IsNullOrEmpty(boxTitle))
            {
                bool wasExpanded = IsBoxExpanded(boxTitle);

                bool isExpanded = DrawBoldFoldout(wasExpanded, boxTitle);

                if (isExpanded)
                {
                    //DrawLine();
                }

                if (wasExpanded != isExpanded)
                {
                    if (isExpanded)
                    {
                        SetBoxExpanded(boxTitle);
                    }
                    else
                    {
                        SetBoxCollapsed(boxTitle);
                    }
                }

                return isExpanded;
            }

            return true;
        }

        protected bool BeginSimpleFoldoutBox(string boxTitle)
        {
            if (!string.IsNullOrEmpty(boxTitle))
            {
                bool wasExpanded = IsBoxExpanded(boxTitle);
                bool isExpanded = DrawBoldFoldout(wasExpanded, boxTitle);

                if (wasExpanded != isExpanded)
                {
                    if (isExpanded)
                    {
                        SetBoxExpanded(boxTitle);
                    }
                    else
                    {
                        SetBoxCollapsed(boxTitle);
                    }
                }

                return isExpanded;
            }

            return true;
        }

        protected bool BeginSimpleFoldoutBox(string boxTitle, string toggleKey)
        {
            if (!string.IsNullOrEmpty(boxTitle))
            {
                bool wasExpanded = IsBoxExpanded(toggleKey);
                bool isExpanded = DrawBoldFoldout(wasExpanded, boxTitle);

                if (wasExpanded != isExpanded)
                {
                    if (isExpanded)
                    {
                        SetBoxExpanded(toggleKey);
                    }
                    else
                    {
                        SetBoxCollapsed(toggleKey);
                    }
                }

                return isExpanded;
            }

            return true;
        }

        protected bool BeginFoldoutBox(string boxTitle, string toggleKey)
        {
            GUIStyle style = new GUIStyle("HelpBox");
            style.padding.left = 15;
            style.padding.right = 0;

            GUILayout.BeginVertical(style);

            if (!string.IsNullOrEmpty(toggleKey))
            {
                bool wasExpanded = IsBoxExpanded(toggleKey);

                bool isExpanded = DrawBoldFoldout(wasExpanded, boxTitle);

                if (isExpanded)
                {
                    //DrawLine();
                }

                if (wasExpanded != isExpanded)
                {
                    if (isExpanded)
                    {
                        SetBoxExpanded(toggleKey);
                    }
                    else
                    {
                        SetBoxCollapsed(toggleKey);
                    }
                }

                return isExpanded;
            }

            return true;
        }

        protected bool IsBoxExpanded(string key)
        {
            string[] editorExpandedBoxes = EditorPrefs.GetString("hb-toggle-on").Split(';');

            for (int i = 0; i < editorExpandedBoxes.Length; i++)
            {
                if (editorExpandedBoxes[i] == key)
                {
                    return true;
                }
            }

            return false;
        }


        protected void SetBoxExpanded(string prefKey)
        {
            string boxExpandedStr = EditorPrefs.GetString("hb-toggle-on");

            if (!string.IsNullOrEmpty(boxExpandedStr))
            {
                boxExpandedStr += ";";
            }

            boxExpandedStr += prefKey;

            EditorPrefs.SetString("hb-toggle-on", boxExpandedStr);
        }

        protected void SetBoxCollapsed(string prefKey)
        {
            string[] editorExpandedBoxes = EditorPrefs.GetString("hb-toggle-on").Split(';');

            string expandName = "";

            for (int i = 0; i < editorExpandedBoxes.Length; i++)
            {
                if (editorExpandedBoxes[i] == prefKey)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(expandName))
                {
                    expandName += ";";
                }

                expandName += editorExpandedBoxes[i];
            }

            EditorPrefs.SetString("hb-toggle-on", expandName);
        }


        protected bool DrawBoldFoldout(bool isExpanded, string text)
        {
            GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
            foldoutStyle.fontStyle = FontStyle.Bold;
            return EditorGUILayout.Foldout(isExpanded, text, foldoutStyle);
        }

        protected bool DrawAddElementButton(string btnText) 
        {
            Color bgColor = GUI.backgroundColor;

            GUI.backgroundColor = Color.grey;
            GUIStyle style2 = new GUIStyle(GUI.skin.button);
            style2.richText = true;

            bool isPressed = (GUILayout.Button("<b>" +btnText + "</b>", style2));
            GUI.backgroundColor = bgColor;
            return isPressed;
        }
    }
}