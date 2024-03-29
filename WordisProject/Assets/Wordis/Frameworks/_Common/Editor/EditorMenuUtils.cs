﻿// ©2019 - 2020 HYPERBYTE STUDIOS LLP
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
using UnityEditor;
using UnityEngine;

namespace Assets.Wordis.Frameworks._Common.Editor
{
    public static class EditorMenuUtils
    {
        [MenuItem("Wordis/Misc./Clear All PlayerPrefs")]
        public static void ClearAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Wordis/Misc./Clear Define Symbol EditorPrefs")]
        public static void ClearDefineSymbolEditorPrefs()
        {
            EditorPrefs.DeleteKey("userRemoved_" + "UnityIAP");
            EditorPrefs.DeleteKey("userRemoved_" + "UnityAds");
            EditorPrefs.DeleteKey("userRemoved_" + "Admob");
            EditorPrefs.DeleteKey("userRemoved_" + "IronSource");
            EditorPrefs.DeleteKey("userRemoved_" + "AppLovinMax");
            EditorPrefs.DeleteKey("userRemoved_" + "Vungle");
        }

        #region CaptureScreenshot
        [MenuItem("Wordis/Misc./Capture Screenshot/1X")]
        private static void Capture1XScreenshot()
        {
            CaptureScreenshot(1);
        }

        [MenuItem("Wordis/Misc./Capture Screenshot/2X")]
        private static void Capture2XScreenshot()
        {
            CaptureScreenshot(2);
        }

        [MenuItem("Wordis/Misc./Capture Screenshot/3X")]
        private static void Capture3XScreenshot()
        {
            CaptureScreenshot(3);
        }

        public static void CaptureScreenshot(int supersize)
        {
            string imgName =
                $"IMG-{DateTime.Now.Year}{DateTime.Now.Month:00}{DateTime.Now.Day:00}-{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}{DateTime.Now.Second:00}.png";
            ScreenCapture.CaptureScreenshot($"{Application.dataPath}/{imgName}", supersize);
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
        #endregion
    }
}
