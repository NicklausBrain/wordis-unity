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
        private void GamePlayUI_OnGameStartedEvent()
        {
            //_currentProgressData = new ProgressData();
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
        /// Clears the progress of current gameplay on game over.
        /// </summary>
        private void GamePlayUI_OnGameOverEvent()
        {
            ClearProgressData();
        }

        /// <summary>
        /// Clears the progress of current gameplay.
        /// </summary>
        public void ClearProgressData()
        {
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
            }
        }

        /// <summary>
        /// This method will be executed after each block shape being placed on board. This will get status of board, block shapes, timer, 
        /// score etc and will save to progress data class which in turn will be saved to playerprefs in json format.
        /// </summary>
        private void SaveProgress()
        {
            //PlayerPrefs.SetString($"gameProgress_{gameMode}",
            //    JsonUtility.ToJson(_currentProgressData));
        }

        public bool HasGameProgress()
        {
            //return PlayerPrefs.HasKey($"gameProgress_{gameMode}");
            return false;
        }

        /// <summary>
        /// Returns game progress for the given mode if any.
        /// </summary>
        public object GetGameProgress()
        {
            //if (HasGameProgress(gameMode))
            //{
            //    ProgressData progressData = JsonUtility.FromJson<ProgressData>(
            //        PlayerPrefs.GetString($"gameProgress_{gameMode}"));
            //    if (progressData != null)
            //    {
            //        return progressData;
            //    }
            //}

            return null;
        }

        /// <summary>
        /// Clears game progress if any for the given game mode.
        /// </summary>
        public void DeleteProgress()
        {
            //PlayerPrefs.DeleteKey($"gameProgress_{gameMode}");
        }
    }
}
