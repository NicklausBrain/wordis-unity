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
using UnityEngine.UI;

namespace Hyperbyte
{   
    /// <summary>
    /// This script component is attached to all buttons daily reward poppup. It handles, displays rewards and giveaway.
    /// daily reward.static You can configure daily rewards from Hyperbyte -> App Settings option from menu item.
    /// </summary>
    public class DailyRewards : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] List<DailyRewardPanel> allDayRewards;
        [SerializeField] Button btnContinue;
        #pragma warning restore 0649

        /// <summary>
        /// Closes popup.
        /// </summary>
        public void OnCloseButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {   
                UIFeedback.Instance.PlayButtonPressEffect();
                gameObject.Deactivate();
            }
        }


        /// <summary>
        /// Continue will close screen after processing daily rewards.
        /// </summary>
        public void OnContinueButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                gameObject.Deactivate();
            }
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            btnContinue.interactable = false;
            UIController.Instance.EnableCurrencyBalanceButton();
            int currentRewardDay = DailyRewardManager.Instance.currentRewardDay;
            PrepareDailyRewardScreen(currentRewardDay);

            Invoke("EnableContinueButton",3F);
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()  {
            UIController.Instance.Invoke("DisableCurrencyBalanceButton",0.1F);
        }


        /// <summary>
        /// Prepares daily reward from this method.
        /// </summary>
        private void PrepareDailyRewardScreen(int currentRewardDay)
        {
            List<int> displayRewardDays = new List<int>();
            List<int> displayRewards = new List<int>();

            if (currentRewardDay < 3)
            {
                for (int index = 1; index <= allDayRewards.Count; index++) {
                    displayRewardDays.Add(index);
                }
            }
            else
            {
                int startIndex = (currentRewardDay - 2);
                for (int index = startIndex; index <= (allDayRewards.Count + (startIndex-1) ); index++) {

                    displayRewardDays.Add(index);
                }
            }

            foreach (int rewardDay in displayRewardDays) {
                displayRewards.Add(GetRewardForDay(rewardDay));
            }

            for (int index = 0; index < allDayRewards.Count; index++) {
                allDayRewards[index].SetReward(displayRewardDays[index], currentRewardDay, displayRewards[index]);
            }
        }   

        /// <summary>
        /// Returns reward for amount for the given day.
        /// </summary>
        int GetRewardForDay(int day)
        {
            if(day < ProfileManager.Instance.GetAppSettings().dailyRewardsSettings.allDayRewards.Length) {
                return ProfileManager.Instance.GetAppSettings().dailyRewardsSettings.allDayRewards[day-1];
            } else {
                day = ProfileManager.Instance.GetAppSettings().dailyRewardsSettings.allDayRewards.Length - 1;
                return ProfileManager.Instance.GetAppSettings().dailyRewardsSettings.allDayRewards[day];
            }
        }

        void EnableContinueButton() {
            btnContinue.interactable = true;
        }
    }
}
