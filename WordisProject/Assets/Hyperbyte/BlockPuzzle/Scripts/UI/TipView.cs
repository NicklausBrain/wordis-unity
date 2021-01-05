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
using UnityEngine.UI;

namespace Hyperbyte
{
	public class TipView : MonoBehaviour 
	{
		#pragma warning disable 0649
		[SerializeField] RectTransform tipContent;
		[SerializeField] Text txtTip;
		#pragma warning restore 0649

		public void ShowTipAtPosition(Vector2 tipPosition, Vector2 anchor, string tipText) {
			if(IsInvoking("HideTip")) {
				CancelInvoke("HideTip");
			}
			tipContent.anchorMax = anchor;
			tipContent.anchorMin = anchor;
			tipContent.pivot = anchor;
			tipContent.anchoredPosition = tipPosition;
			txtTip.text = tipText;
		}

		public void ShowTipAtPosition(Vector2 tipPosition, Vector2 anchor, string tipText, float duration) {
			if(IsInvoking("HideTip")) {
				CancelInvoke("HideTip");
			}
			
			tipContent.anchorMax = anchor;
			tipContent.anchorMin = anchor;
			tipContent.pivot = anchor;
			tipContent.anchoredPosition = tipPosition;
			txtTip.text = tipText;

			if(!IsInvoking("HideTip")) {
				Invoke("HideTip", duration);
			}
		}


		void HideTip() {
			gameObject.Deactivate();
		}
	}
}

