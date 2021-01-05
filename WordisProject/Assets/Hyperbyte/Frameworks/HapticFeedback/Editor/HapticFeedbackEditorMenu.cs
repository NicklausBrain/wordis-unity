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

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml.Linq;
using System.Linq;

public class HapticFeedbackEditorMenu  
{
	[InitializeOnLoad]
	public class AutorunNew
	{
		static AutorunNew() {
			EditorApplication.update += RunOnce;
		}

		static void RunOnce() {
			EditorApplication.update -= RunOnce;
			#if UNITY_ANDROID
			VerifyAndroidManifest();
			#endif
		}	

		/// <summary>
		/// Generates default AndroidManifest.xml if not available at Plugins -> Android path.
		/// </summary>
		static void VerifyAndroidManifest() 
		{
			string androidManifestPath = Application.dataPath + "/Plugins/Android/";
			
			if(File.Exists(androidManifestPath + "AndroidManifest.xml")) {
				CheckPermissionsInAndroidManifest(androidManifestPath + "AndroidManifest.xml", "android.permission.VIBRATE");
			}
			else {
				if(!Directory.Exists(androidManifestPath)) {
					Directory.CreateDirectory(androidManifestPath);
					AssetDatabase.Refresh();
				}
				string defaultManifestPath = Application.dataPath + "/Hyperbyte/Frameworks/HapticFeedback/Plugins/Android/DefaultAndroidManifest.xml";
				if(File.Exists(defaultManifestPath)) {
					File.Copy(defaultManifestPath, androidManifestPath + "AndroidManifest.xml");
					AssetDatabase.Refresh();
				}
			}
		}	

		/// <summary>
		/// Verifies if android vibration permission is added or not.
		/// </summary>
		static void CheckPermissionsInAndroidManifest(string manifestPath, string permission) 
		{
			XDocument xDocument = XDocument.Parse(File.ReadAllText(manifestPath));
			List<XElement> result = xDocument.Root.Elements("uses-permission").ToList();
			bool vibrationPermissionExists = false;

			foreach(XElement element in result) {
				string permissionValue = element.Attribute("{http://schemas.android.com/apk/res/android}name").Value;

				if(permissionValue.ToLower().Equals(permission.ToLower())) {
					vibrationPermissionExists = true;
				}
			}

			if(!vibrationPermissionExists) {
				XElement ele = new XElement("uses-permission");
				ele.Add(new XAttribute("{http://schemas.android.com/apk/res/android}name",permission));
				xDocument.Root.Add(ele);

				xDocument.Save(manifestPath);
				AssetDatabase.Refresh();
			}
		}
	}
}
