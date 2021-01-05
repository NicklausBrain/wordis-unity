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
	/// <summary>
	/// Add this script component to any UI Button element to animate on button click event. 
	/// This script will use Animator attached to the button.
	/// </summary>
	[RequireComponent(typeof(Button))]
	public class ButtonAnimation : MonoBehaviour 
	{
		[SerializeField] bool doAnimate = true;
		Button thisButton;

		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		void Awake()
		{
			thisButton = GetComponent<Button>();
			if(GetComponent<Animator>() == null) {
				doAnimate = false;
			}
		}

		/// <summary>
		/// Start is called on the frame when a script is enabled just before
		/// any of the Update methods is called the first time.
		/// </summary>
		void Start()
		{
			thisButton.onClick.AddListener(()=> {
				if(doAnimate) {
					thisButton.GetComponent<Animator>().SetTrigger("Press");
				}
				UIFeedback.Instance.PlayButtonPressEffect();
			});
		}
	}
}