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
using UnityEngine.UI;

namespace Hyperbyte
{
	[CustomEditor(typeof(UnityIAPButton))]
	public class UnityIAPButtonEditor : CustomInspectorHelper 
	{
		private bool cache = false;
		UnityIAPButton unityIAPButton;
		IAPProducts iapProducts;

		string[] allProducts;

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (!cache)
			{
				cache = true;
				unityIAPButton = (UnityIAPButton)target;
				iapProducts = (IAPProducts)Resources.Load("IAPProducts");
				allProducts = new string[iapProducts.productInfos.Length];

				int index = 0;
				foreach(ProductInfo product in iapProducts.productInfos) {
					allProducts[index] = product.productName;
					index++;
				}
			}

			EditorUtility.SetDirty(unityIAPButton);

			GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
			labelStyle.fontStyle = FontStyle.Bold;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Button Type : ",  labelStyle,GUILayout.MaxWidth(120));
			unityIAPButton.buttonType = EditorGUILayout.Popup(unityIAPButton.buttonType, new string[] {"RESTORE","PURCHASE"});
			EditorGUILayout.EndHorizontal();

			if(unityIAPButton.buttonType == 1) 
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Product Name : ",  labelStyle,GUILayout.MaxWidth(120));
				unityIAPButton.inAppProductIndex = EditorGUILayout.Popup(unityIAPButton.inAppProductIndex, allProducts);
				EditorGUILayout.EndHorizontal();

				labelStyle.fontStyle = FontStyle.Normal;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Title Text : ",  labelStyle,GUILayout.MaxWidth(120));
				unityIAPButton.txtTitle = (Text)EditorGUILayout.ObjectField(unityIAPButton.txtTitle, typeof(Text),true);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Description Text : ",  labelStyle,GUILayout.MaxWidth(120));
				unityIAPButton.txtDescription = (Text)EditorGUILayout.ObjectField(unityIAPButton.txtDescription, typeof(Text),true);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Price Text : ",  labelStyle,GUILayout.MaxWidth(120));
				unityIAPButton.txtPrice = (Text)EditorGUILayout.ObjectField(unityIAPButton.txtPrice, typeof(Text),true);
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.HelpBox("You don't need to add On Click() event explicitly. It will be handled automatically. Purchase completion responce and rewards will be handled from IAPManagert.cs",MessageType.Info);
		}
	}
}