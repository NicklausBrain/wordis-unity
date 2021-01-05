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
using UnityEditor;
using UnityEngine;

namespace Hyperbyte
{	
	public class GamePlaySettingsEditorMenu
	{
		#region AppSetting
        [MenuItem("Hyperbyte/GamePlay Settings",false, 11)]
        public static void GenerateGameSettingsScriptable()
        {
            string assetPath = "Assets/Hyperbyte/Resources";
            string assetName = "GamePlaySettings.asset";

            GamePlaySettings asset;

            if (!System.IO.Directory.Exists(assetPath)) {
                System.IO.Directory.CreateDirectory(assetPath);
            }

            if (System.IO.File.Exists(assetPath + "/"+ assetName))  {
                asset = (GamePlaySettings)(Resources.Load(System.IO.Path.GetFileNameWithoutExtension(assetName)));
            }
            else   {
                asset = ScriptableObject.CreateInstance<GamePlaySettings>();
                AssetDatabase.CreateAsset(asset, assetPath + "/" + assetName);
                AssetDatabase.SaveAssets();
            }

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
        #endregion
	}
}
