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

using System.Collections;
using Hyperbyte.Localization;
using UnityEngine;
using UnityEngine.UI;
using Hyperbyte.Ads;

namespace Hyperbyte
{
    public class GameOver : MonoBehaviour
    {
        #pragma warning disable 0649

        [Tooltip("Game Over reason text")]
        [SerializeField] Text txtGameOveTitle;

        [Tooltip("Score text from game over screen")]
        [SerializeField] Text txtScore;

        [Tooltip("BestScore text from game over screen")]
        [SerializeField] Text txtBestScore;

        [Tooltip("Reward Penel")] 
        [SerializeField] GameObject rewardPanel;

        [Tooltip("Reward text from game over screen")]
        [SerializeField] Text txtReward;
        [SerializeField] GameObject gemImage;
        [SerializeField] GameObject rewardAnimation;
        [SerializeField] GameObject highScoreParticle;
        #pragma warning restore 0649


        int rewardAmount = 0;
        int totalLinesCompleted = 0;
        int gameOverId = 0;

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
		private void OnEnable() 
		{
            UIController.Instance.EnableCurrencyBalanceButton();
            TryShowingInterstitial();
		}

		/// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
		private void OnDisable() 
        {
            rewardAnimation.SetActive(false);
            UIController.Instance.DisableCurrencyBalanceButton();
        }


        /// <summary>
        /// Try to show Interstitial ad on game over if ad is available.
        /// </summary>
        void TryShowingInterstitial() 
        {
            if(AdManager.Instance.adSettings.showInterstitialOnGameOver) 
            {
                if(AdManager.Instance.IsInterstitialAvailable()) {
                    AdManager.Instance.ShowInterstitial();
                }
            }
        }

        /// <summary>
        /// Sets game data and score on game over.
        /// </summary>
        public void SetGameData(GameOverReason reason, int score, int _totalLinesCompleted, GameMode gameMode)
        {   
            switch (reason)
            {
                case GameOverReason.GRID_FILLED:
                    txtGameOveTitle.SetTextWithTag("txtGameOver_gridfull");
                    break;

                case GameOverReason.BOMB_BLAST:
                    txtGameOveTitle.SetTextWithTag("txtGameOver_bombexplode");
                    break;

                case GameOverReason.TIME_OVER:
                    txtGameOveTitle.SetTextWithTag("txtGameOver_timeover");
                    break;
            }

            totalLinesCompleted = _totalLinesCompleted;
            txtScore.text = score.ToString("N0");

            int bestScore = ProfileManager.Instance.GetBestScore(gameMode);
            if (score > bestScore)
            {
                bestScore = score;
                ProfileManager.Instance.SetBestScore(bestScore, gameMode);
            }
            txtBestScore.text = bestScore.ToString("N0");
            ProgressGameReward();


            // Number of time game over shown. Also total game play counts.
            gameOverId = PlayerPrefs.GetInt("gameOverId",0);
            gameOverId += 1;
            PlayerPrefs.SetInt("gameOverId", gameOverId);

            if(ProfileManager.Instance.gameOverReviewSessions.Contains(gameOverId)) {
                InputManager.Instance.DisableTouchForDelay(2F);
                Invoke("CheckForReview",2F);
            }
        }


        public void ProgressGameReward() {
            
            if(GamePlayUI.Instance.rewardOnGameOver) 
            {
                if(!rewardPanel.activeSelf) {
                    rewardPanel.SetActive(true);
                    gemImage.SetActive(true);
                }

                if(GamePlayUI.Instance.giveFixedReward) {
                    rewardAmount = GamePlayUI.Instance.fixedRewardAmount;
                } else {
                    rewardAmount =  ((int) (GamePlayUI.Instance.rewardPerLine * totalLinesCompleted));
                }
                txtReward.text = rewardAmount.ToString();

                if(rewardAmount > 0) {
                    Invoke("ShowRewardAnimation",1F);
                }
            } else {
                rewardPanel.SetActive(false);
            }
        }


        void CheckForReview() {
             UIController.Instance.CheckForReviewAppPopupOnGameOver(gameOverId);
        }

        void ShowRewardAnimation() 
        {
            CurrencyManager.Instance.AddGems(rewardAmount);
            rewardAnimation.SetActive(true);
        }

        /// <summary>
        /// Continue button click listener.
        /// </summary>
        public void OnContinueButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                InputManager.Instance.DisableTouchForDelay(1F);
                UIFeedback.Instance.PlayButtonPressEffect();
            }
        }

        /// <summary>
        /// Home button click listener.
        /// </summary>
        public void OnHomeButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.OpenHomeScreenFromGameOver();
            }
        }

        /// <summary>
        /// Replay button click listner.
        /// </summary>
        public void OnReplayButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                StartCoroutine(RestartGame());
            }
        }

        /// <summary>
        /// Restarts game.
        /// </summary>
        IEnumerator RestartGame()
        {
            GamePlayUI.Instance.ResetGame();
            yield return new WaitForSeconds(0.1f);
            GamePlayUI.Instance.StartGamePlay(GamePlayUI.Instance.currentGameMode);
            gameObject.Deactivate();
        }
    }
}


