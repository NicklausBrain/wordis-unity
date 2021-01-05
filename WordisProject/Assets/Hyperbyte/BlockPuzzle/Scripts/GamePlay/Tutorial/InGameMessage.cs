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
using Hyperbyte.UITween;
using Hyperbyte.Localization;

namespace Hyperbyte
{
	public class InGameMessage : MonoBehaviour 
	{
		public AnimationCurve animationCurve;
		public GameObject messageView;
		public Text txtMessageText;

		public void ShowMessage(GameOverReason reason) 
		{
			messageView.transform.localScale = Vector3.zero;
			txtMessageText.text = GetRescueReason(reason);

			messageView.gameObject.SetActive(true);
			messageView.transform.LocalScale(Vector3.one, 0.2F).SetAnimation(animationCurve).OnComplete(()=> {
				messageView.transform.LocalScale(Vector3.zero, 0.2F).SetAnimation(animationCurve).SetDelay(1F);
			});
		}

		public string GetRescueReason(GameOverReason reason)
        {
            switch (reason)
            {
                case GameOverReason.GRID_FILLED:
                    return LocalizationManager.Instance.GetTextWithTag("txtGameOver_gridfull");

                case GameOverReason.BOMB_BLAST:
                    return LocalizationManager.Instance.GetTextWithTag("txtGameOver_bombexplode");

                case GameOverReason.TIME_OVER:
                    return LocalizationManager.Instance.GetTextWithTag("txtGameOver_timeover");

				default:
					return LocalizationManager.Instance.GetTextWithTag("txtGameOver_gridfull");
            }
        }
	}
}

