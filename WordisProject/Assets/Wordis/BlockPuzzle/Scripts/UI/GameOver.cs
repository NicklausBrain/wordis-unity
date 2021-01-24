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
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Levels;
using Assets.Wordis.BlockPuzzle.Scripts.Controller;
using Assets.Wordis.BlockPuzzle.Scripts.GamePlay;
using Assets.Wordis.BlockPuzzle.Scripts.UI.Extensions;
using Assets.Wordis.Frameworks.InputManager.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Wordis.BlockPuzzle.Scripts.UI
{
    /// <summary>
    /// Code behind for Game Over and Level Completion
    /// </summary>
    public class GameOver : MonoBehaviour
    {
#pragma warning disable 0649

        [Tooltip("Game Over reason text")]
        [SerializeField]
        Text _txtGameOverTitle;

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

        private int _rewardAmount = 0;
        private int _gameOverId = 0;
        private IWordisGameLevel _gameLevel;

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
            int totalWordsMatched,
            IWordisGameLevel gameLevel)
        {
            _gameLevel = gameLevel;

            if (gameLevel.IsCompleted) // level is finished successfully
            {
                _txtGameOverTitle.text = "LEVEL PASSED"; // todo: localize
            }

            txtScore.text = score.ToString("N0");

            // TODO: check all this logic
            int bestScore = ProfileManager.Instance.GetBestScore(gameLevel.Title);
            if (score > bestScore)
            {
                bestScore = score;
                ProfileManager.Instance.SetBestScore(bestScore, gameLevel.Title);
            }

            txtBestScore.text = bestScore.ToString("N0");

            // Number of time game over shown. Also total game play counts.
            _gameOverId = PlayerPrefs.GetInt("gameOverId", 0);
            _gameOverId += 1;
            PlayerPrefs.SetInt("gameOverId", _gameOverId);

            if (ProfileManager.Instance.gameOverReviewSessions.Contains(_gameOverId))
            {
                InputManager.Instance.DisableTouchForDelay(2F);
                Invoke(nameof(CheckForReview), 2F);
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
                UIFeedback.Instance.PlayButtonPressEffect();

                // select next level
                var nextLevels = SelectLevel.Levels
                    .SkipWhile(l => l.GetType() != _gameLevel.GetType())
                    .Skip(1) // this level
                    .ToArray();

                var nextLevel = nextLevels.Any()
                    ? nextLevels.First() // go next
                    : SelectLevel.Levels.First(); // start all over again

                // start next level
                StartCoroutine(RestartGame(nextLevel));
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
                StartCoroutine(RestartGame(_gameLevel));
            }
        }

        /// <summary>
        /// Restarts game.
        /// </summary>
        private IEnumerator RestartGame(IWordisGameLevel level)
        {
            yield return new WaitForSeconds(0.1f);
            GamePlayUI.Instance
                .SetLevel(level)
                .RestartGame();
            gameObject.Deactivate();
        }
    }
}