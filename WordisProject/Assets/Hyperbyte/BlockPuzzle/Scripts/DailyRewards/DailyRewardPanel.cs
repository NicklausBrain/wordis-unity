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
using Hyperbyte.Utils;
using Hyperbyte.Localization;

namespace Hyperbyte
{
    /// <summary>
    /// This script component is attached to all buttons daily reward poppup. It handles, displays rewards and giveaway.
    /// daily reward for the current day.
    /// </summary>
    public class DailyRewardPanel : MonoBehaviour
    {
        int rewardAmount = 0;

        #pragma warning disable 0649
        [SerializeField] Text txtDay;
        [SerializeField] Text txtReward;
        [SerializeField] Image imgCheckmark;
        [SerializeField] Image imgCollectedBorder;
        [SerializeField] RectTransform GemsRewardPlacement;
        #pragma warning restore 0649

        /// <summary>
        /// Prepares daily reward for the day.
        /// </summary>
        public void SetReward(int day, int currentRewardDay, int reward)
        {
            txtDay.SetFormattedTextWithTag("txtDay_FR", day.ToString(), "");
            txtReward.text = reward.ToString();

            if (day <= currentRewardDay)
            {
                imgCheckmark.gameObject.SetActive(true);
                imgCollectedBorder.gameObject.SetActive(true);

                if (day < currentRewardDay)
                {
                    imgCheckmark.SetColorWithThemeId("DCReward");
                    imgCollectedBorder.SetColorWithThemeId("DCCollectBorder");

                    GetComponent<CanvasGroup>().alpha = 0.8F;
                }
                else
                {
                    imgCheckmark.SetColorWithThemeId("DCReward");
                    imgCollectedBorder.SetColorWithThemeId("DCCollectBorder");
                    GetComponent<CanvasGroup>().alpha = 1F;
                    Invoke("AnimateAndProcessRewards", 1F);
                }
            }
            else
            {
                imgCheckmark.gameObject.SetActive(false);
                imgCollectedBorder.gameObject.SetActive(false);
                GetComponent<CanvasGroup>().alpha = 1F;
            }
            rewardAmount = reward;
        }

        /// <summary>
        /// Gives reward with animation.
        /// </summary>
        void AnimateAndProcessRewards()
        {
            UIController.Instance.PlayAddGemsAnimationAtPosition(GemsRewardPlacement.position, 0);
            CurrencyManager.Instance.AddGems(rewardAmount);
            DailyRewardManager.Instance.SaveCollectRewardInfo();
        }
    }
}
