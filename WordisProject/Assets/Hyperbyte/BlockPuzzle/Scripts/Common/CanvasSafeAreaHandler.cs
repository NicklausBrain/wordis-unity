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

namespace Hyperbyte
{
	/// <summary>
	/// This script component can be added to any canvas to handle safe area or notch.
	/// </summary>
	public class CanvasSafeAreaHandler : MonoBehaviour 
	{
		[SerializeField] Vector2 offsetMin = Vector2.zero;
		[SerializeField] Vector2 offsetMax = Vector2.zero;

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>		
		private void Awake() {
			float bottomSafeArea = Screen.safeArea.y;
			float topSafeArea = Screen.height - ( bottomSafeArea + Screen.safeArea.height);

			if(topSafeArea > 0) {
				GetComponent<RectTransform>().offsetMin = offsetMin;
				GetComponent<RectTransform>().offsetMax = offsetMax;
			}
		}
	}
}
