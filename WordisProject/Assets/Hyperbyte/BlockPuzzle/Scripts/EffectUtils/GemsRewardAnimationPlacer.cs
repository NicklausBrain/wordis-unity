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
    public class GemsRewardAnimationPlacer : MonoBehaviour
    {
        RewardAddAnimation rewardAddAnimation;
        
        #pragma warning disable 0649
        [SerializeField] float animationDelay;
        #pragma warning restore 0649

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            InputManager.Instance.DisableTouchForDelay(1F);
            GameObject rewardAnim = (GameObject)Instantiate(Resources.Load("RewardAnimation")) as GameObject;
            rewardAnim.transform.SetParent(transform);
            rewardAnim.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            rewardAnim.transform.localScale = Vector3.one;
            rewardAddAnimation = rewardAnim.GetComponent<RewardAddAnimation>();

            Invoke("ShowAddRewardAnimation", (animationDelay +  0.2F));
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            if (rewardAddAnimation != null)
            {
                Destroy(rewardAddAnimation.gameObject);
            }
        }

        void ShowAddRewardAnimation()
        {
            rewardAddAnimation.PlayGemsBalanceUpdateAnimation(UIController.Instance.ShopButtonGemsIcon.position,0);
            
        }
    }
}
