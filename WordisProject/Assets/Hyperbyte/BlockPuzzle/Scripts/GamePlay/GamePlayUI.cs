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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hyperbyte
{
    public enum GameOverReason 
    {
        GRID_FILLED,    // If there is no enough space to place existing blocks. Applies to all game mode.
        TIME_OVER,      // If timer finishing. Applied only to time mode.
        BOMB_BLAST      // If Counter on placed bomb reaches to 0. Applies only to blast mode.
    }

    public class GamePlayUI : Singleton<GamePlayUI>
    {

        [Header("Public Class Members")]
        [Tooltip("GamePlay Script Reference")]
        public GamePlay gamePlay;

        [Tooltip("ScoreManager Script Reference")]
        public ScoreManager scoreManager;

        [Tooltip("ProgressData Script Reference")]
        public ProgressData progressData;

        [Tooltip("TimeModeProgresssBar Script Reference")]
        public TimeModeProgresssBar timeModeProgresssBar;

        [Tooltip("InGameMessage Script Reference To Show Message")]
        public InGameMessage inGameMessage;
        
        [System.NonSerialized] public GameModeSettings currentModeSettings;

        // Stores current playing mode.
        [System.NonSerialized] public GameMode currentGameMode;

        // GamePlay Setting Scriptable Instance. Initializes on awake.
        [System.NonSerialized] GamePlaySettings gamePlaySettings;

        #region  Game Status event callbacks.
        //Event action for game start callback.
        public static event Action<GameMode> OnGameStartedEvent;

        //Event action for shape place callback.
        public static event Action OnShapePlacedEvent;
        
        //Event action for game finish callback.
        public static event Action<GameMode> OnGameOverEvent;

        //Event action for game pause callback.
        public static event Action<GameMode, bool> OnGamePausedEvent;
        #endregion 

        // Total lines clear during gameplay.
        [HideInInspector] public int totalLinesCompleted = 0;

        // Resuce used for the game or not.
        [HideInInspector] public bool rescueDone = false;

        // Reason for game over. Will Initialize at game over or rescue.
        [HideInInspector] public GameOverReason currentGameOverReason;

        /// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
        private void Awake() {
            // Initializes the GamePlay Settings Scriptable.
            if (gamePlaySettings == null)  {
                gamePlaySettings = (GamePlaySettings)Resources.Load("GamePlaySettings");
            }
        }

        /// <summary>
        /// Starts game with selected game mode.
        /// </summary>
        public void StartGamePlay(GameMode gameMode)
        {
            currentGameMode = gameMode;
            currentModeSettings = GetCurrentModeSettings();

            // Checks if the there is user progerss from previos session.
            bool hasPreviosSessionProgress = GameProgressTracker.Instance.HasGameProgress(currentGameMode);
            if (hasPreviosSessionProgress) {
                progressData = GameProgressTracker.Instance.GetGameProgress(GamePlayUI.Instance.currentGameMode);
            }

            // Enables gameplay screen if not active.
            if (!gamePlay.gameObject.activeSelf) {
                gamePlay.gameObject.SetActive(true);
            }
            
            // Generated gameplay grid.
            gamePlay.boardGenerator.GenerateBoard(progressData);

            // Board Generator will create and initialize board with progress data if available.
            gamePlay.blockShapeController.PrepareShapeContainer(progressData);

            #region Time Mode Specific
            // Will enable timer start seeking it. If there is previos session data then timer will start from remaining duration.
            if(gameMode == GameMode.Timed) {
                timeModeProgresssBar.gameObject.SetActive(true);
                timeModeProgresssBar.SetTimer((progressData != null) ? progressData.remainingTimer : timeModeInitialTimer);
                timeModeProgresssBar.StartTimer();
            } else {
                if (timeModeProgresssBar.gameObject.activeSelf) {
                    timeModeProgresssBar.gameObject.SetActive(false);
                }
            }

            if(progressData != null) {
                totalLinesCompleted = progressData.totalLinesCompleted;
                rescueDone = progressData.rescueDone;
            }
            #endregion


            // Invokes Game Start Event Callback.
            if(OnGameStartedEvent != null) {
                OnGameStartedEvent.Invoke(currentGameMode);
            }

            ShowInitialTip();

        }

        void ShowInitialTip() {
             
             switch(currentGameMode) {
                 case GameMode.Timed:
                    UIController.Instance.ShowTimeModeTip();
                 break;
             }
        }

        

        /// <summary>
        /// Returns size of the current grid.
        /// </summary>
        /// <returns></returns>
        public BoardSize GetBoardSize() {
            return currentModeSettings.boardSize;
        }

        /// <summary>
        /// Returns game settings for the current game mode.
        /// </summary>
        GameModeSettings GetCurrentModeSettings()
        {
            switch (currentGameMode)
            {
                case GameMode.Classic:
                    return gamePlaySettings.classicModeSettings;
                case GameMode.Timed:
                    return gamePlaySettings.timeModeSettings;
                case GameMode.Blast:
                    return gamePlaySettings.blastModeSettings;
                case GameMode.Advance:
                    return gamePlaySettings.advancedModeSettings;
            }
            return gamePlaySettings.classicModeSettings;
        }

        // Returns of list of all standard block shapes.
        public List<BlockShapeInfo> GetStandardBlockShapesInfo() {
            return gamePlaySettings.standardBlockShapesInfo.ToList();
        }

        // Returns of list of all advanced block shapes.
        public List<BlockShapeInfo> GetAdvancedBlockShapesInfo() {
            return gamePlaySettings.advancedBlockShapesInfo.ToList();
        }

        // Returns score to be added on for each block cleared.
        public int blockScore {
            get {
                return gamePlaySettings.blockScore;
            }
        }

        // Returns score to be added on for each line cleared.
        public int singleLineBreakScore
        {
            get  {
                return gamePlaySettings.singleLineBreakScore;
            }
        }

        // Returns score multiplier to be added on for each line cleared when more than 1 lines cleared together.
        public int multiLineScoreMultiplier {
            get {
                return gamePlaySettings.multiLineScoreMultiplier;
            }
        }

        #region Time Mode Specific
        // Returns Intial timer for time mode.
        public float timeModeInitialTimer {
            get {
                return gamePlaySettings.initialTime;
            }
        }

        // Returns seconds to be added 
        public float timeModeAddSecondsOnLineBreak {
            get {
                return gamePlaySettings.addSecondsOnLineBreak;
            }
        }
        #endregion

        #region Blast Mode Specific
        // Returns initial counter when on bomb.
        public int blastModeCounter {
            get {
                return gamePlaySettings.blastModeCounter;
            }
        }

        //Retuens after how many block shape place new bomb should be placed.
        public int addBombAfterMoves {
            get {
                return gamePlaySettings.addBombAfterMoves;
            }
        }
        #endregion

        public bool rewardOnGameOver {
            get {
                return gamePlaySettings.rewardOnGameOver;
            }
        }

        public bool giveFixedReward { 
            get {
                return gamePlaySettings.giveFixedReward;
            }
        }

        public int fixedRewardAmount {
            get {
                return gamePlaySettings.fixedRewardAmount;
            }
        }

        public float rewardPerLine {
            get {
                return gamePlaySettings.rewardPerLineCompleted;
            }
        }


        // Invokes callback for OnShapePlaced Event.
        public void OnShapePlaced() {
            if(OnShapePlacedEvent != null) {
                OnShapePlacedEvent.Invoke();
            }
        }

        public bool CanRescueGame() {
            if(!rescueDone && currentModeSettings.allowRescueGame) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if game can be rescued.
        /// </summary>
        public void TryRescueGame(GameOverReason reason) {
            currentGameOverReason = reason;
            StartCoroutine(TryRescueGameEnumerator(reason));
           
        }


        IEnumerator TryRescueGameEnumerator(GameOverReason reason) 
        {
            inGameMessage.ShowMessage(reason);
            yield return new WaitForSeconds(1.5F);
            GameProgressTracker.Instance.ClearProgressData();
            
            if(CanRescueGame()) {
                UIController.Instance.rescueGameScreen.Activate();
                UIController.Instance.rescueGameScreen.GetComponent<RescueGame>().SetRescueReason(reason);
            } else {
                OnGameOver();
            }
        }

        public void OnRescueCancelled() {
             OnGameOver();
        }

        /// <summary>
        /// Resume Game With Rescue Done
        /// </summary>
        public void OnRescueSuccessful() {
            gamePlay.PerfromRescueAction(currentGameOverReason);
			rescueDone = true;

            GameProgressTracker.Instance.SaveProgressExplicitly();
            gamePlay.blockShapeController.UpdateShapeContainers();
		}

        /// <summary>
        /// Pauses the game on pressing pause button.
        /// </summary>
        public void OnPauseButtonPressed() {
            if (InputManager.Instance.canInput()) {
                UIFeedback.Instance.PlayButtonPressEffect();
                #region Time Mode Specific
                if(currentGameMode == GameMode.Timed && timeModeProgresssBar.GetRemainingTimer() < 5F) {
                    return;
                }
                #endregion
                UIController.Instance.pauseGameScreen.Activate();
            }
        }

        /// <summary>
        /// Will be called on game over. 
        /// </summary>
        public void OnGameOver() {
            if(OnGameOverEvent != null) {
                OnGameOverEvent.Invoke(currentGameMode);
            }
            UIController.Instance.gameOverScreen.GetComponent<GameOver>().SetGameData(currentGameOverReason, scoreManager.GetScore(), totalLinesCompleted, currentGameMode);
            UIController.Instance.gameOverScreen.Activate();
        }

        /// <summary>
        /// Resets game.
        /// </summary>
        public void ResetGame() {
            progressData = null;
            totalLinesCompleted = 0;
            rescueDone = false;
            gamePlay.ResetGame();
            scoreManager.ResetGame();
        }

         #region Time Mode Specific
        /// <summary>
        /// Returns Remaining Timer.
        /// </summary>
        public int GetRemainingTimer() {
            return (currentGameMode == GameMode.Timed) ? timeModeProgresssBar.GetRemainingTimer() : 0;
        }
        #endregion  

        /// <summary>
        /// Pauses game.
        /// </summary>
        public void PauseGame() {
            if(OnGamePausedEvent != null) {
                OnGamePausedEvent.Invoke(currentGameMode, true);
            }
        }
        
        /// <summary>
        /// Resumes game.
        /// </summary>
        public void ResumeGame() {
            if (OnGamePausedEvent != null) {
                OnGamePausedEvent.Invoke(currentGameMode, false);
            }
        }

        /// <summary>
        /// Will rest game to empty state and start new game with same selected mode.
        /// </summary>
        public void RestartGame() {
            GameProgressTracker.Instance.ClearProgressData();
            ResetGame();
            StartGamePlay(currentGameMode);
        }
    }
}
