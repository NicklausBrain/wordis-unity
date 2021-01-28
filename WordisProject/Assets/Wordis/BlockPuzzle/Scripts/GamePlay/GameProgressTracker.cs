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
using System.Collections.Concurrent;
using System.Collections.Generic;
using Assets.Wordis.BlockPuzzle.GameCore;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
using Assets.Wordis.Frameworks.Utils;
using Newtonsoft.Json;
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
        public const string GameSessionKey = "WordisSession";
        public const string WordsStatsKey = "WordisWordsStats";

        private ConcurrentDictionary<string, int> _wordsStats;

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnEnable()
        {
            var emptyDict = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            _wordsStats = PlayerPrefs.HasKey(WordsStatsKey)
                ? JsonConvert.DeserializeObject<ConcurrentDictionary<string, int>>(
                      PlayerPrefs.GetString(WordsStatsKey)) ?? emptyDict
                : emptyDict;
        }

        /// <summary>
        /// This function is called when the behaviour becomes enabled or active.
        /// </summary>
        private void OnDisable()
        {
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.SetString(WordsStatsKey, JsonConvert.SerializeObject(_wordsStats));
        }

        public void AddWordsStats(IReadOnlyList<WordMatchEx> matches)
        {
            foreach (WordMatchEx match in matches)
            {
                _wordsStats.AddOrUpdate(match.Word, 1, (_, c) => c + 1);
            }
        }

        public IReadOnlyDictionary<string, int> GetWordStats()
        {
            return _wordsStats;
        }

        /// <summary>
        /// Clears the progress of current gameplay.
        /// </summary>
        //public void ClearProgressData()
        //{
        //    DeleteProgress();
        //}

        /// <summary>
        /// OnApplicationPause is set to true or false.
        /// Normally, false is the value returned by the OnApplicationPause message.
        /// This means the game is running normally in the editor.
        /// If an editor window such as the Inspector is chosen the game is paused and OnApplicationPause returns true.
        /// When the game window is selected and active OnApplicationPause again returns false.
        /// True means that the game is not active.
        /// </summary>
        /// <param name="pause"></param>
        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {

            }
        }

        public void SaveSession()
        {
            //var gameState = JsonConvert.SerializeObject(GamePlayUI.Instance.CurrentLevel.Game);

            //Debug.LogWarning(gameState);

            ////var restoredGame = new WordisGame();
            //var restored = JsonConvert.DeserializeObject<WordisGame>(gameState);

            //Debug.LogWarning(restored);
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
        //public void DeleteProgress()
        //{
        //    //PlayerPrefs.DeleteKey($"gameProgress_{gameMode}");
        //}
    }
}
