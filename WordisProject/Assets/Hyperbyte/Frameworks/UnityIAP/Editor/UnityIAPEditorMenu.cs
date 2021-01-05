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
using Hyperbyte.Utils;

namespace  Hyperbyte
{    
    public class UnityIAPEditorMenu : MonoBehaviour
    {
       	#region MobileAdsSetting
        [MenuItem("Hyperbyte/Unity IAP Settings", false, 14)]
        public static void GenerateMobileAdsManager()
        {
            string assetPath = "Assets/Hyperbyte/Resources";
            string assetName = "IAPProducts.asset";

            IAPProducts asset;

            if (!System.IO.Directory.Exists(assetPath))
            {
                System.IO.Directory.CreateDirectory(assetPath);
            }

            if (System.IO.File.Exists(assetPath + "/"+ assetName))
            {
                asset = (IAPProducts)(Resources.Load(System.IO.Path.GetFileNameWithoutExtension(assetName)));
            }
            else
            {
                asset = ScriptableObject.CreateInstance<IAPProducts>();
                AssetDatabase.CreateAsset(asset, assetPath + "/" + assetName);
                AssetDatabase.SaveAssets();
            }

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    	#endregion

        [InitializeOnLoad]
        public class AutorunNew
        {
            static AutorunNew() {
                EditorApplication.update += RunOnce;
                AssetDatabase.importPackageCompleted += ImportPackageStartedCallback;	
            }

            static void RunOnce() {
                EditorApplication.update -= RunOnce;
            }

            public static void ImportPackageStartedCallback(string str) {  
        		CheckAllRequiredSDKs(str);
	        }

            public static void CheckAllRequiredSDKs(string packageName) 
		    {
			    SDKInfo thisSDKInfo = null;
			    bool thisSdkExists = false;
			    bool thisDefineSymbolExists = false;
			
			    #region UnityAds
			    thisSDKInfo = new SDKInfo("UnityIAP", "UnityEngine.Purchasing.Default", "HB_UNITYIAP");
			    thisSdkExists = NamespaceUtils.CheckNamespacesExists(thisSDKInfo.sdkNameSpace);
			    thisDefineSymbolExists = ScriptingDefineSymbolEditor.HasDefineSymbol(thisSDKInfo.sdkScriptingDefineSymbol);
			    VerifySDKImportInfo(thisSDKInfo, thisSdkExists, thisDefineSymbolExists);
			    #endregion
            }

            static void VerifySDKImportInfo(SDKInfo currentSdkInfo, bool sdkExists, bool defineSymbolExists) {
                if(sdkExists) {
                    if(!defineSymbolExists) {
                        AddScriptingDefineSymbol(currentSdkInfo.sdkName, currentSdkInfo.sdkScriptingDefineSymbol);
                    }
                } else {
                    RemoveScriptingDefineSymbol(currentSdkInfo.sdkScriptingDefineSymbol);
                }
		    }

            static void AddScriptingDefineSymbol( string sdkName, string symbol) {
                if(!EditorPrefs.HasKey("userRemoved_"+sdkName)) {
                    ScriptingDefineSymbolEditor.AddScriptingDefineSymbol(symbol);
                } 
            }

            static void RemoveScriptingDefineSymbol(string symbol) {
                if(ScriptingDefineSymbolEditor.HasDefineSymbol(symbol)) {
                    ScriptingDefineSymbolEditor.RemoveScriptingDefineSymbol(symbol);
                }
            }   
        }
    }
}
