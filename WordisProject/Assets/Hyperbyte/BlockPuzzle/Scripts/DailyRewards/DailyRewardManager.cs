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

using System;   
using UnityEngine;

namespace Hyperbyte 
{
    /// <summary>
    /// This sigletion class will track and handles daily reward during game.
    /// </summary>
    public class DailyRewardManager : Singleton<DailyRewardManager>
    {
        bool hasInitialised = false;
        bool isDailyRewardAvailable = false;

        [HideInInspector] public int currentRewardDay = 1;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start() {
             Initialise();  
        }

        void Initialise() {
            if(!hasInitialised) {
                hasInitialised = true;
            }
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable() {
            SessionManager.OnSessionUpdatedEvent += OnSessionUpdated;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable() {
            SessionManager.OnSessionUpdatedEvent -= OnSessionUpdated;
        }

        private void OnSessionUpdated(SessionInfo info) 
        {
            if(ProfileManager.Instance.GetAppSettings().useDailyRewards) {
                if(info.currentSessionCount == 1) 
                {
                    currentRewardDay = 1;
                    isDailyRewardAvailable = true;
                    UIController.Instance.ShowDailyRewardsPopup();
                } else {
                    CheckForDailyReward();
                }
            }
        }

        /// <summary>
        /// Checks if day has changed and daily reward is available or not.
        /// </summary>
        void CheckForDailyReward() 
        {
            DateTime lastRewardCollectionDate = DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("lastRewardCollectionDate",DateTime.Now.ToBinary().ToString()))).Date;
            DateTime currentDate = DateTime.Now.Date;
            int totalDays = (int) (currentDate - lastRewardCollectionDate).TotalDays;

            if(totalDays < 0) {
                PlayerPrefs.DeleteKey("lastRewardDay");
                isDailyRewardAvailable = false;
                currentRewardDay = 0;
                return;
            }

            if(totalDays >= 1) {
                if(totalDays == 1) {
                    int lastRewardDay = PlayerPrefs.GetInt("lastRewardDay",0);
                    currentRewardDay = (lastRewardDay + 1);
                    isDailyRewardAvailable = true;
                } else {
                    PlayerPrefs.DeleteKey("lastRewardDay");
                    currentRewardDay = 1;
                    isDailyRewardAvailable = true;
                }

                if(isDailyRewardAvailable) {
                    UIController.Instance.ShowDailyRewardsPopup();
                }
            }
        }

        /// <summary>
        /// Callback sent to all game objects when the player pauses.
        /// </summary>
        /// <param name="pauseStatus">The pause state of the application.</param>
        private void OnApplicationPause(bool pauseStatus) 
        {
            if(pauseStatus) {
            } else {
                if(hasInitialised) {
                /// Checks if daily reward is available on app resume.
                    CheckForDailyReward();
                }
            }
        }

        /// <summary>
        /// Saves info of daily reward collection and curreny day.
        /// </summary>
        public void SaveCollectRewardInfo() 
        {
            PlayerPrefs.SetInt("lastRewardDay",currentRewardDay);
            PlayerPrefs.SetString("lastRewardCollectionDate",DateTime.Now.ToBinary().ToString());
        }
    }
}