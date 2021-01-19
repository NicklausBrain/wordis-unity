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
using Assets.Wordis.BlockPuzzle.Scripts.Controller;
using Assets.Wordis.BlockPuzzle.Scripts.GamePlay;
using Assets.Wordis.BlockPuzzle.Scripts.UI.Extensions;
using Assets.Wordis.Frameworks.InputManager.Scripts;
using Assets.Wordis.Frameworks.Localization.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Wordis.BlockPuzzle.Scripts.UI
{
    public class GameOver : MonoBehaviour
    {
#pragma warning disable 0649

        [Tooltip("Game Over reason text")]
        [SerializeField]
        Text txtGameOveTitle;

        [Tooltip("Score text from game over screen")]
        [SerializeField]
        Text txtScore;

        [Tooltip("BestScore text from game over screen")]
        [SerializeField]
        Text txtBestScore;

        [Tooltip("Reward Penel")]
        [SerializeField]
        GameObject rewardPanel;

        [Tooltip("Reward text from game over screen")]
        [SerializeField]
        Text txtReward;

        [SerializeField] GameObject gemImage;
        [SerializeField] GameObject rewardAnimation;
        [SerializeField] GameObject highScoreParticle;
#pragma warning restore 0649


        int _rewardAmount = 0;
        int _totalLinesCompleted = 0;
        int _gameOverId = 0;

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
        private void TryShowingInterstitial()
        {
        }

        /// <summary>
        /// Sets game data and score on game over.
        /// </summary>
        public void SetGameData(
            int score,
            int totalLinesCompleted,
            GameOverReason reason = GameOverReason.GridFilled,
            GameMode gameMode = GameMode.Classic)
        {
            switch (reason)
            {
                case GameOverReason.GridFilled:
                    txtGameOveTitle.SetTextWithTag("txtGameOver_gridfull");
                    break;

                case GameOverReason.TimeOver:
                    txtGameOveTitle.SetTextWithTag("txtGameOver_timeover");
                    break;
            }

            _totalLinesCompleted = totalLinesCompleted;
            txtScore.text = score.ToString("N0");

            int bestScore = ProfileManager.Instance.GetBestScore(gameMode);
            if (score > bestScore)
            {
                bestScore = score;
                ProfileManager.Instance.SetBestScore(bestScore, gameMode);
            }

            txtBestScore.text = bestScore.ToString("N0");

            // Number of time game over shown. Also total game play counts.
            _gameOverId = PlayerPrefs.GetInt("gameOverId", 0);
            _gameOverId += 1;
            PlayerPrefs.SetInt("gameOverId", _gameOverId);

            if (ProfileManager.Instance.gameOverReviewSessions.Contains(_gameOverId))
            {
                InputManager.Instance.DisableTouchForDelay(2F);
                Invoke("CheckForReview", 2F);
            }
        }

        private void CheckForReview()
        {
            UIController.Instance.CheckForReviewAppPopupOnGameOver(_gameOverId);
        }

        private void ShowRewardAnimation()
        {
            CurrencyManager.Instance.AddGems(_rewardAmount);
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
        /// Replay button click listener.
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
        private IEnumerator RestartGame()
        {
            GamePlayUI.Instance.ResetGame();
            yield return new WaitForSeconds(0.1f);
            GamePlayUI.Instance.StartGamePlay();
            gameObject.Deactivate();
        }

        public enum GameOverReason
        {
            GridFilled, // If there is no enough space to place existing blocks. Applies to all game mode.
            TimeOver, // If timer finishing. Applied only to time mode.
        }
    }
}