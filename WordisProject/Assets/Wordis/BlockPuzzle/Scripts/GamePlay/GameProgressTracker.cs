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
using Assets.Wordis.BlockPuzzle.GameCore.Levels;
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
        public const string BestScoreKey = "BestScore";

        private ConcurrentDictionary<string, int> _wordsStats;

        private void Awake()
        {
            Debug.LogWarning($"{nameof(GameProgressTracker)}.Awake");

            var emptyDict = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            _wordsStats = _wordsStats ?? (PlayerPrefs.HasKey(WordsStatsKey)
                ? JsonConvert.DeserializeObject<ConcurrentDictionary<string, int>>(
                      PlayerPrefs.GetString(WordsStatsKey)) ?? emptyDict
                : emptyDict);
        }

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

        private void OnApplicationQuit()
        {
            Debug.LogWarning($"{nameof(GameProgressTracker)}.OnApplicationQuit");
            Debug.LogWarning($"{nameof(_wordsStats)}={_wordsStats.Count}");
            PlayerPrefs.SetString(WordsStatsKey, JsonConvert.SerializeObject(_wordsStats));
        }

        public void AddWordsStats(IReadOnlyList<WordMatchEx> matches)
        {
            Debug.LogWarning($"{nameof(GameProgressTracker)}.AddWordsStats");
            foreach (WordMatchEx match in matches)
            {
                _wordsStats.AddOrUpdate(match.Word, 1, (_, c) => c + 1);
            }
        }

        public IReadOnlyDictionary<string, int> GetWordStats()
        {
            return _wordsStats;
        }

        public bool HasSession(IWordisGameLevel gameLevel)
        {
            return PlayerPrefs.HasKey($"{GameSessionKey}_{gameLevel.Id}");
        }

        public void DropSession(IWordisGameLevel gameLevel)
        {
            PlayerPrefs.DeleteKey($"{GameSessionKey}_{gameLevel.Id}");
        }

        public void SaveSession(IWordisGameLevel gameLevel)
        {
            if (gameLevel.Id == nameof(WordisSurvivalMode))
            {
                var gameAsJson = WordisGame.ToJson(gameLevel.Game);

                PlayerPrefs.SetString($"{GameSessionKey}_{gameLevel.Id}", gameAsJson);
            }
        }

        public WordisGame RestoreSession(IWordisGameLevel gameLevel)
        {
            if (gameLevel.Id == nameof(WordisSurvivalMode))
            {
                var gameAsJson = PlayerPrefs.GetString($"{GameSessionKey}_{gameLevel.Id}");

                var restoredGame = WordisGame.FromJson(gameAsJson);

                return restoredGame;
            }

            return null;
        }

        /// <summary>
        /// Returns best score for the given level.
        /// </summary>
        public int GetBestScore(string gameLevel)
        {
            return PlayerPrefs.HasKey($"{BestScoreKey}_{gameLevel}")
                ? PlayerPrefs.GetInt($"{BestScoreKey}_{gameLevel}", 0)
                : 0;
        }

        /// <summary>
        /// Saves best for the give mode if it bigger than existing.
        /// </summary>
        public void TrySetBestScore(int score, string gameLevel)
        {
            int lastBestScore = GetBestScore(gameLevel);

            if (lastBestScore < score)
            {
                PlayerPrefs.SetInt($"{BestScoreKey}_{gameLevel}", score);
            }
        }
    }
}
