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
using Hyperbyte.Ads;

namespace Hyperbyte
{
    /// <summary>
    /// This script controlls and manages the ingame currecy, its balace, addition or subtraction of balance.
    /// </summary>
    public class CurrencyManager : Singleton<CurrencyManager>
    {
        public static event Action<int> OnCurrencyUpdated;

        int currentBalance;
        bool hasInitialised = false;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private void Start()
        {
            Initialise();
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            AdManager.OnRewardedAdRewardedEvent += OnRewardedAdRewarded;
        }

        /// <summary>
        /// This function is called when the object becomes disabled and inactive.
        /// </summary>
        private void OnDisable()
        {
            AdManager.OnRewardedAdRewardedEvent -= OnRewardedAdRewarded;
        }

        /// <summary>
        /// Initialize and restores the balance  and displays it.
        /// </summary>
        void Initialise()
        {

            if (PlayerPrefs.HasKey("currentBalance"))
            {
                currentBalance = PlayerPrefs.GetInt("currentBalance");
            }
            else
            {
                currentBalance = ProfileManager.Instance.GetAppSettings().defaultGemsAmount;

            }
            hasInitialised = true;
        }

        /// <summary>
        /// Returns current balance.
        /// </summary>
        public int GetCurrentGemsBalance()
        {
            if (!hasInitialised)
            {
                Initialise();
            }
            return currentBalance;
        }


        /// <summary>
        /// Add gems to current balance.
        /// </summary>
        public void AddGems(int gemsAmount)
        {
            if (gemsAmount > 0) {
                currentBalance += gemsAmount;
            }
            SaveCurrencyBalance();
            if (OnCurrencyUpdated != null)
            {
                OnCurrencyUpdated.Invoke(currentBalance);
            }
            AudioController.Instance.PlayClip(AudioController.Instance.addGemsSound);
        }

        /// <summary>
        /// Will deduct given amount from balance if enough balance is available.
        /// </summary>
        public bool DeductGems(int gemsAmount)
        {
            if (currentBalance >= gemsAmount)
            {
                currentBalance -= gemsAmount;
                SaveCurrencyBalance();

                if (OnCurrencyUpdated != null) {
                    OnCurrencyUpdated.Invoke(currentBalance);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Save currency balance.
        /// </summary>
        void SaveCurrencyBalance()
        {
            PlayerPrefs.SetInt("currentBalance", currentBalance);
        }

        /// <summary>
        /// Adds currecy balance from the rewarded ad.
        /// </summary>
        void OnRewardedAdRewarded(string tag)
        {
            switch(tag) {
                case "FreeGems":
                AddGems(ProfileManager.Instance.GetAppSettings().watchVideoRewardAmount);
                UIController.Instance.purchaseSuccessScreen.Activate();
                break;
            }
        }
    }
}
