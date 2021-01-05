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

namespace Hyperbyte
{
    /// <summary>
    /// This script component typically tracks and saves the progress of game. This is used to start game previos progress 
    /// when user leaves game without completing/finishing it. The info of game progress is saved in playerpref in json format.
    /// </summary>
    public class GameProgressTracker : Singleton<GameProgressTracker>
    {
        ProgressData currentProgressData;

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            ///  Registers game status callbacks.
            GamePlayUI.OnGameStartedEvent += GamePlayUI_OnGameStartedEvent;
            GamePlayUI.OnGameOverEvent += GamePlayUI_OnGameOverEvent;
            GamePlayUI.OnShapePlacedEvent += GamePlayUI_OnShapePlacedEvent;
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnDisable()
        {
            ///  Registers game status callbacks.
            GamePlayUI.OnGameStartedEvent -= GamePlayUI_OnGameStartedEvent;
            GamePlayUI.OnShapePlacedEvent -= GamePlayUI_OnShapePlacedEvent;
            GamePlayUI.OnGameOverEvent -= GamePlayUI_OnGameOverEvent;
        }

        /// <summary>
        /// Creates a progress data instance on game start and will be maintained throughout game.
        /// </summary>
        private void GamePlayUI_OnGameStartedEvent(GameMode obj)
        {
            currentProgressData = new ProgressData();
        }

        /// <summary>
        /// Save progres on calling manually. Typically will be called after rescue done.
        /// </summary>
        public void SaveProgressExplicitly() {
            if(GamePlayUI.Instance.currentModeSettings.saveProgress) {
                Invoke("SaveProgress", 2F);
            }
        }

        /// <summary>
        /// Save progres after block shape places.
        /// </summary>
        private void GamePlayUI_OnShapePlacedEvent()
        {
            if (GamePlayUI.Instance.currentModeSettings.saveProgress) {
                Invoke("SaveProgress", 1F);
            }
        }

        /// <summary>
        /// Clears the progress of current gameplay on game over.
        /// </summary>
        private void GamePlayUI_OnGameOverEvent(GameMode currentMode)
        {
            ClearProgressData();
        }

         /// <summary>
        /// Clears the progress of current gameplay.
        /// </summary>
        public void ClearProgressData()
        {
            currentProgressData = null;
            DeleteProgress(GamePlayUI.Instance.currentGameMode);
        }

        /// <summary>
        /// If you want to further optimize progress saving operation, then you can move save progress call to OnApplicationPause(true) instead of GamePlayUI_OnShapePlacedEvent. 
        /// </summary>
        /// <param name="pause"></param>
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                if (currentProgressData != null)
                {

                }
            }
        }

        /// <summary>
        /// This method will be executed after each block shape being placed on board. This will get status of board, block shapes, timer, bombs, 
        /// score etc and will save to progress data class which inturn will be saved to playerprefs in json format.
        /// </summary>
        private void SaveProgress()
        {
            if (currentProgressData != null)
            {
                string[] gridData = new string[GamePlay.Instance.allRows.Count];
                int rowIndex = 0;

                // Reads the status of all elements from the board grid.
                foreach (List<Block> blockRow in GamePlay.Instance.allRows)
                {
                    string row = "";
                    foreach (Block b in blockRow)
                    {
                        row = (row == string.Empty) ? (row = (b.isAvailable + "-" + b.assignedSpriteTag)) : (string.Concat(row, "," + (b.isAvailable + "-" + b.assignedSpriteTag)));
                    }
                    gridData[rowIndex] = row;
                    rowIndex++;
                }

                currentProgressData.gridData = gridData;
                currentProgressData.currentShapesInfo = GamePlay.Instance.blockShapeController.GetCurrentShapesInfo();
                
                #region Blast Mode Specific
                // Gets all the placed bombs and its counter.
                if (GamePlayUI.Instance.currentGameMode == GameMode.Blast)
                {
                    currentProgressData.allBombInfo = GamePlay.Instance.GetAllBombInfo();
                }
                #endregion

                // Attached all the fetched data to progress data class instance.
                currentProgressData.score = GamePlayUI.Instance.scoreManager.GetScore();
                currentProgressData.totalLinesCompleted = GamePlayUI.Instance.totalLinesCompleted;
                currentProgressData.rescueDone = GamePlayUI.Instance.rescueDone;
                currentProgressData.remainingTimer = GamePlayUI.Instance.GetRemainingTimer();
                currentProgressData.totalShapesPlaced = GamePlay.Instance.blockShapeController.GetTotalShapesPlaced();
                PlayerPrefs.SetString("gameProgress_" + GamePlayUI.Instance.currentGameMode, JsonUtility.ToJson(currentProgressData));
            }
        }

        public bool HasGameProgress(GameMode gameMode)
        {
            return PlayerPrefs.HasKey("gameProgress_" + gameMode);
        }

        /// <summary>
        /// Returns game progress for the given mode if any.
        /// </summary>
        public ProgressData GetGameProgress(GameMode gameMode)
        {
            if (HasGameProgress(gameMode))
            {
                ProgressData progressData = JsonUtility.FromJson<ProgressData>(PlayerPrefs.GetString("gameProgress_" + gameMode));
                if (progressData != null)
                {
                    return progressData;
                }
            }
            return null;
        }

        /// <summary>
        /// Clears game progress if any for the given game mode.
        /// </summary>
        public void DeleteProgress(GameMode gameMode)
        {
            PlayerPrefs.DeleteKey("gameProgress_" + gameMode);
        }
    }

    /// <summary>
    /// Progress data class will be converted to json after preparing it to save game progress.
    /// </summary>
    public class ProgressData
    {
        public string[] gridData;
        public ShapeInfo[] currentShapesInfo;
        
        #region Blast Mode Specific
        public BombInfo[] allBombInfo;
        #endregion
        
        public int score;
        public int totalLinesCompleted;
        public bool rescueDone;

        #region Time Mode Specific
        public int remainingTimer = 0;
        #endregion

        public int totalShapesPlaced = 0;

        public ProgressData()
        {
        }
    }
    /// <summary>
    /// Class that contains info of block shape.
    /// </summary>
    [System.Serializable]
    public class ShapeInfo
    {
        public bool isAdvanceShape = false;
        public string shapeName;
        public float shapeRotation;

        // Class constructor with required parameters.
        public ShapeInfo(bool _isAdvanceShape, string _shapeName, float _shapeRotation)
        {
            isAdvanceShape = _isAdvanceShape;
            shapeName = _shapeName;
            shapeRotation = _shapeRotation;
        }
    }

    #region Blast Mode Specific
    /// <summary>
    /// Class that contains info on bomb.
    /// </summary>
    [System.Serializable]
    public class BombInfo
    {
        public int rowId;
        public int columnId;
        public int remainCounter;

        // Class constructor with required parameters.
        public BombInfo(int _rowId, int _columnId, int _remainCounter)
        {
            this.rowId = _rowId;
            this.columnId = _columnId;
            this.remainCounter = _remainCounter;
        }
    }
    #endregion
}

