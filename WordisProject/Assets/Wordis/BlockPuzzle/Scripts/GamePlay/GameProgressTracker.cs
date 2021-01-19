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
using Assets.Wordis.Frameworks.Utils;
using UnityEngine;

namespace Assets.Wordis.BlockPuzzle.Scripts.GamePlay
{
    /// <summary>
    /// This script component typically tracks and saves the progress of game.
    /// This is used to start game previous progress when user leaves game without completing/finishing it.
    /// The info of game progress is saved in playerpref in json format.
    /// </summary>
    public class GameProgressTracker : Singleton<GameProgressTracker>
    {
        ProgressData _currentProgressData;

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnDisable()
        {
        }

        /// <summary>
        /// Creates a progress data instance on game start and will be maintained throughout game.
        /// </summary>
        private void GamePlayUI_OnGameStartedEvent(GameMode obj)
        {
            _currentProgressData = new ProgressData();
        }

        /// <summary>
        /// Save progress on calling manually. Typically will be called after rescue done.
        /// </summary>
        public void SaveProgressExplicitly()
        {
            //if (GamePlayUI.Instance.currentModeSettings.saveProgress)
            //{
            //    Invoke(nameof(SaveProgress), 2F);
            //}
        }

        /// <summary>
        /// Save progress after block shape places.
        /// </summary>
        private void GamePlayUI_OnShapePlacedEvent()
        {
            //if (GamePlayUI.Instance.currentModeSettings.saveProgress)
            //{
            //    Invoke(nameof(SaveProgress), 1F);
            //}
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
            _currentProgressData = null;
            DeleteProgress();
        }

        /// <summary>
        /// If you want to further optimize progress saving operation, then you can move save progress call to OnApplicationPause(true) instead of GamePlayUI_OnShapePlacedEvent. 
        /// </summary>
        /// <param name="pause"></param>
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                if (_currentProgressData != null)
                {
                }
            }
        }

        /// <summary>
        /// This method will be executed after each block shape being placed on board. This will get status of board, block shapes, timer, 
        /// score etc and will save to progress data class which in turn will be saved to playerprefs in json format.
        /// </summary>
        private void SaveProgress(GameMode gameMode = GameMode.Classic)
        {
            if (_currentProgressData != null)
            {
                string[] gridData = new string[GameBoard.Instance.allRows.Count];
                int rowIndex = 0;

                // Reads the status of all elements from the board grid.
                foreach (List<Block> blockRow in GameBoard.Instance.allRows)
                {
                    string row = string.Empty;
                    foreach (Block b in blockRow)
                    {
                        row = row == string.Empty
                            ? $"{b.isAvailable}-{b.assignedSpriteTag}"
                            : string.Concat(row, $",{b.isAvailable}-{b.assignedSpriteTag}");
                    }

                    gridData[rowIndex] = row;
                    rowIndex++;
                }

                _currentProgressData.gridData = gridData;

                // Attached all the fetched data to progress data class instance.
                _currentProgressData.score = GamePlayUI.Instance.scoreManager.GetScore();
                PlayerPrefs.SetString($"gameProgress_{gameMode}",
                    JsonUtility.ToJson(_currentProgressData));
            }
        }

        public bool HasGameProgress(GameMode gameMode = GameMode.Classic)
        {
            return PlayerPrefs.HasKey($"gameProgress_{gameMode}");
        }

        /// <summary>
        /// Returns game progress for the given mode if any.
        /// </summary>
        public ProgressData GetGameProgress(GameMode gameMode = GameMode.Classic)
        {
            if (HasGameProgress(gameMode))
            {
                ProgressData progressData = JsonUtility.FromJson<ProgressData>(
                    PlayerPrefs.GetString($"gameProgress_{gameMode}"));
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
        public void DeleteProgress(GameMode gameMode = GameMode.Classic)
        {
            PlayerPrefs.DeleteKey($"gameProgress_{gameMode}");
        }
    }

    /// <summary>
    /// Progress data class will be converted to json after preparing it to save game progress.
    /// </summary>
    public class ProgressData
    {
        public string[] gridData;

        public int score;
        public int totalLinesCompleted;
        public bool rescueDone;

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
        public ShapeInfo(
            bool isAdvanceShape,
            string shapeName,
            float shapeRotation)
        {
            this.isAdvanceShape = isAdvanceShape;
            this.shapeName = shapeName;
            this.shapeRotation = shapeRotation;
        }
    }
}