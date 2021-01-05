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
using Hyperbyte.Localization;
using Hyperbyte.Ads;

namespace Hyperbyte
{
    /// <summary>
    /// This script is used to rescue game using coins or watching video.
    /// </summary>
    public class RescueGame : MonoBehaviour
    {
        #pragma warning disable 0649
        [SerializeField] Text txtTitle;
        [SerializeField] RectTransform gemsIcon;
        [SerializeField] Text txtRescueGemAmount;
        [SerializeField] Button BtnRescueWithAds;
        #pragma warning restore 0649

        bool attemptedRescueWithGems = false;
        string rescueVideoTag = "RescueGame";

        bool isRescueDone = false;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            txtRescueGemAmount.text = ProfileManager.Instance.GetAppSettings().rescueGameGemsAmount.ToString();
        }

        public void SetRescueReason(GameOverReason reason)
        {
            switch (reason)
            {
                case GameOverReason.GRID_FILLED:
                    txtTitle.SetTextWithTag("txtGameOver_gridfull");
                    break;

                case GameOverReason.BOMB_BLAST:
                    txtTitle.SetTextWithTag("txtGameOver_bombexplode");
                    break;

                case GameOverReason.TIME_OVER:
                    txtTitle.SetTextWithTag("txtGameOver_timeover");
                    break;
            }
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            /// Pauses the game when it gets enabled.
            GamePlayUI.Instance.PauseGame();
            AdManager.OnRewardedAdRewardedEvent += OnRewardedAdRewarded;
            UIController.Instance.EnableCurrencyBalanceButton();

            if(AdManager.Instance.IsRewardedAvailable()) {
                BtnRescueWithAds.GetComponent<CanvasGroup>().alpha = 1.0F;
                BtnRescueWithAds.interactable = true;
            } else {
                BtnRescueWithAds.GetComponent<CanvasGroup>().alpha = 0.2F;
                BtnRescueWithAds.interactable = false;
            }
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            /// Resumes the game when it gets enabled.
            AdManager.OnRewardedAdRewardedEvent -= OnRewardedAdRewarded;
            attemptedRescueWithGems = false;
            UIController.Instance.DisableCurrencyBalanceButton();

            if(isRescueDone) {
                GamePlayUI.Instance.ResumeGame();
            }
            isRescueDone = false;
        }


        /// <summary>
        /// Will rescue game after showing rewarded video ad.
        /// </summary>
        public void OnContinueWithWatchVideoButtonPressed()
        {
            if (InputManager.Instance.canInput()) {
                UIFeedback.Instance.PlayButtonPressEffect();
                ShowRewardedToRescue();
            }
        }

        /// <summary>
        /// Will rescue game with gems.
        /// </summary>
        public void OnContinueWithGemsButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                if (CurrencyManager.Instance.DeductGems(ProfileManager.Instance.GetAppSettings().rescueGameGemsAmount))
                {
                    UIController.Instance.PlayDeductGemsAnimation(gemsIcon.position, 0.1F);
                    Invoke("ResumeGameWithRescue", 1.5F);
                }
                else
                {
                    attemptedRescueWithGems = true;
                    //Will open shop if not having enough gems.
                    UIController.Instance.shopScreen.Activate();
                }
            }
        }

        /// <summary>
        /// Will start/continnue game with rescue successful.
        /// </summary>
        void ResumeGameWithRescue()
        {
            isRescueDone = true;
            InputManager.Instance.DisableTouchForDelay(1F);
            GamePlayUI.Instance.OnRescueSuccessful();
            gameObject.Deactivate();
        }

        /// <summary>
        /// Closes pause screen and resumes gameplay.
        /// </summary>
        public void OnCloseButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                isRescueDone = false;
                UIFeedback.Instance.PlayButtonPressEffect();
                GamePlayUI.Instance.OnRescueCancelled();
                gameObject.Deactivate();
            }
        }

        void ShowRewardedToRescue()
        {
            if (AdManager.Instance.IsRewardedAvailable()) {
                AdManager.Instance.ShowRewardedWithTag(rescueVideoTag);
            }
        }

        /// <summary>
        ///  Rewarded Ad Successful.see
        /// </summary>
        void OnRewardedAdRewarded(string watchVidoTag)
        {
            if (watchVidoTag == rescueVideoTag)
            {
                isRescueDone = true;
                GamePlayUI.Instance.OnRescueSuccessful();
                gameObject.Deactivate();
            }
        }

        /// <summary>
        ///  Not in use. THis method can be called if rescue should be executed on sufficient balance received.
        /// </summary>
        public void ReattemptRescueWithGems()
        {
            if (attemptedRescueWithGems)
            {
                if (CurrencyManager.Instance.DeductGems(ProfileManager.Instance.GetAppSettings().rescueGameGemsAmount))
                {
                    UIController.Instance.PlayDeductGemsAnimation(gemsIcon.position, 0.1F);
                    Invoke("ResumeGameWithRescue", 1.5F);
                }
            }
        }
    }
}

