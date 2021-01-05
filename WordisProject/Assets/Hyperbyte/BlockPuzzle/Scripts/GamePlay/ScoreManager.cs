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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hyperbyte.UITween;

namespace Hyperbyte
{
    /// <summary>
    /// Handled the game score.
    /// </summary>
	public class ScoreManager : MonoBehaviour
    {
        #pragma warning disable 0649
        // Text displays score.
        [SerializeField] private Text txtScore;

        // Text displays best score for selected mode.
        [SerializeField] private Text txtBestScore;
        #pragma warning restore 0649

        int Score = 0;
        int blockScore = 0;
        int singleLineBreakScore = 0;
        int multiLineScoreMultiplier = 0;

        // Yield instruction for the score countet iterations.
        WaitForSeconds scoreIterationWait = new WaitForSeconds(0.02F);

        #pragma warning disable 0649
        [SerializeField] private ScoreAnimator scoreAnimator;
        #pragma warning restore 0649
        

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        void OnEnable()
        {
            /// Registers game status callbacks.
            GamePlayUI.OnGameStartedEvent += GamePlayUI_OnGameStartedEvent;
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable()
        {
            /// Unregisters game status callbacks.
            GamePlayUI.OnGameStartedEvent -= GamePlayUI_OnGameStartedEvent;
        }

        /// <summary>
        /// Set best score onn game start. 
        /// </summary>
        private void GamePlayUI_OnGameStartedEvent(GameMode currentGameMode)
        {
            #region score data to local members
            blockScore = GamePlayUI.Instance.blockScore;
            singleLineBreakScore = GamePlayUI.Instance.singleLineBreakScore;
            multiLineScoreMultiplier = GamePlayUI.Instance.multiLineScoreMultiplier;
            #endregion

            if (GamePlayUI.Instance.progressData != null)
            {
                Score += GamePlayUI.Instance.progressData.score;
            }
            txtScore.text = Score.ToString("N0");
            txtBestScore.text = ProfileManager.Instance.GetBestScore(GamePlayUI.Instance.currentGameMode).ToString("N0");
        }

        /// <summary>
        /// Adds score based on calculation and bonus.
        /// </summary>
        public void AddScore(int linesCleared, int clearedBlocks)
        {
            int scorePerLine = singleLineBreakScore + ((linesCleared - 1) * multiLineScoreMultiplier);
            int scoreToAdd = ((linesCleared * scorePerLine) + (clearedBlocks * blockScore));

            int oldScore = Score;
            Score += scoreToAdd;

            StartCoroutine(SetScore(oldScore, Score));
            scoreAnimator.Animate(scoreToAdd);
        }


        void ShowScoreAnimation() {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
            mousePos.z = 0;
            scoreAnimator.transform.position = mousePos;
            // txtAnimatedText.text = "+" + 100.ToString ();
            
        }

        /// <summary>
        /// Returns score for the current game mode.
        /// </summary>
        public int GetScore()
        {
            return Score;
        }

        /// <summary>
        /// Set score with countetr animatio effect.
        /// </summary>
        IEnumerator SetScore(int lastScore, int currentScore)
        {
            int IterationSize = (currentScore - lastScore) / 10;
            txtScore.transform.LocalScale(Vector3.one * 1.2F, 0.2F).OnComplete(() =>
            {
                txtScore.transform.LocalScale(Vector3.one, 0.2F);
            });

            for (int index = 1; index < 10; index++)
            {
                lastScore += IterationSize;
                txtScore.text = lastScore.ToString("N0");
                AudioController.Instance.PlayClipLow(AudioController.Instance.addScoreSoundChord, 0.15F);
                yield return scoreIterationWait;
            }
            txtScore.text = currentScore.ToString("N0");
        }

        /// <summary>
        /// Resets score on game over, game quit.
        /// </summary>
        public void ResetGame()
        {
            txtScore.text = "0";
            txtBestScore.text = "0";
            Score = 0;
        }
    }
}