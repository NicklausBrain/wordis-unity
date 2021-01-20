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
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;
using Assets.Wordis.BlockPuzzle.Scripts.Controller;
using Assets.Wordis.BlockPuzzle.Scripts.UI;
using Assets.Wordis.BlockPuzzle.Scripts.UI.Extensions;
using Assets.Wordis.Frameworks.InputManager.Scripts;
using Assets.Wordis.Frameworks.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Wordis.BlockPuzzle.Scripts.GamePlay
{
    public class GamePlayUI : Singleton<GamePlayUI>
    {
        private readonly object _gameLock = new object();

        private readonly WordisSettings _wordisSettings = new WordisSettings(
            width: 9,
            height: 9,
            minWordMatch: 3,
            waterLevel: 4);

        private WordisGame _wordisGame;

        private void GameStep() => HandleGameEvent(GameEvent.Step);

        /// <summary>
        /// Starts / resumes the game.
        /// </summary>
        public void ResumeGame()
        {
            PauseGame(); // to prevent double callback
            InvokeRepeating(nameof(GameStep), 1f, _gamePlaySettings.gameSpeed);
        }

        /// <summary>
        /// Stops / pauses the game.
        /// </summary>
        public void PauseGame() => CancelInvoke(nameof(GameStep));

        public void HandleGameEvent(GameEvent gameEvent)
        {
            lock (_gameLock)
            {
                PauseGame(); // prevent premature UI refresh

                if (_wordisGame.IsGameOver)
                {
                    // stop the game cycle
                    PauseGame();
                    GameOver();
                    return;
                }

                var lastGame = _wordisGame;
                var newGame = _wordisGame.Handle(gameEvent);
                _wordisGame = newGame;
                RefreshPresentation(lastGame, newGame);
                ResumeGame();
            }
        }

        /// <summary>
        /// Starts game with selected game mode.
        /// </summary>
        public void RestartGame()
        {
            ClearGame();

            // Enables gameplay screen if not active.
            if (!gameBoard.gameObject.activeSelf)
            {
                gameBoard.gameObject.SetActive(true);
            }

            // Generated gameplay grid.
            gameBoard.boardGenerator.GenerateBoard(_wordisSettings);
            scoreManager.Init();

            ResumeGame();
        }

        /// <summary>
        /// Pauses the game on pressing pause button.
        /// </summary>
        public void OnPauseButtonPressed()
        {
            if (InputManager.Instance.canInput())
            {
                UIFeedback.Instance.PlayButtonPressEffect();
                UIController.Instance.pauseGameScreen.Activate();
            }
        }

        [Tooltip("GamingButtonsController Script Reference")]
        public GamingButtonsController gamingButtonsController;

        [Tooltip("GamingSwipesController Script Reference")]
        public GamingSwipesController gamingSwipesController;

        [Header("Public Class Members")]
        [Tooltip("GamePlay Script Reference")]
        public GameBoard gameBoard;

        [Tooltip("ScoreManager Script Reference")]
        public ScoreManager scoreManager;

        [Tooltip("InGameMessage Script Reference To Show Message")]
        public InGameMessage inGameMessage;

        // GamePlay Setting Scriptable Instance. Initializes on awake.
        [NonSerialized] private GamePlaySettings _gamePlaySettings;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Initializes the GamePlay Settings Scriptable.
            if (_gamePlaySettings == null)
            {
                _gamePlaySettings = (GamePlaySettings)Resources.Load(nameof(GamePlaySettings));
            }
        }

        /// <summary>
        /// This function is called when the behaviour becomes disabled or inactive.
        /// </summary>
        private void OnDisable() => ClearGame();

        /// <summary>
        /// Will be called on game over. 
        /// </summary>
        private void GameOver()
        {
            UIController.Instance.gameOverScreen
                .GetComponent<GameOver>()
                .SetGameData(
                    scoreManager.GetScore(),
                    _wordisGame.Matches.Count);

            UIController.Instance.gameOverScreen.Activate();
        }

        private void ClearGame()
        {
            PauseGame();
            gameBoard.Clear();
            scoreManager.Clear();
            GameProgressTracker.Instance.ClearProgressData();
            _wordisGame = new WordisGame(_wordisSettings);
        }

        private static string lastLog;

        private void RefreshPresentation(
            WordisGame lastGameState, WordisGame newGameState)
        {
            var newMatches = newGameState.Matches.Last;

            if (newMatches.Any()) // on word matches
            {
                // 1. display matched words
                foreach (var match in newMatches)
                {
                    var log = "MATCH! '" + match.Word + "' Step: " + newGameState.GameEvents.Count;
                    Debug.LogWarning(log);

                    if (log == lastLog)
                    {
                        Debug.LogError("LOL it is here");
                    }

                    lastLog = log;

                    inGameMessage.ShowMessage(match.Word);
                }

                var blocksToClear =
                    newMatches
                        .SelectMany(match => match.MatchedChars)
                        .Select(c => gameBoard.allColumns[c.X][c.Y])
                        .ToArray();

                // 2. display score (todo: check calculation logic)
                scoreManager.AddScore(newMatches.Count, blocksToClear.Length);

                // 3. animate blocks destruction
                StartCoroutine(GameBoard.ClearAllBlocks(_wordisSettings, blocksToClear));

                // 4. play break sound
                AudioController.Instance.PlayLineBreakSound(blocksToClear.Length);
            }

            for (int x = 0; x < newGameState.Matrix.Width; x++)
            {
                for (int y = 0; y < newGameState.Matrix.Height; y++)
                {
                    var wordisObject = newGameState.Matrix[x, y];

                    RefreshVisualBlock(x, y, wordisObject);
                }
            }
        }

        private void RefreshVisualBlock(int x, int y, WordisObj wordisObj)
        {
            var block = gameBoard.allColumns[x][y];

            if (wordisObj == null)
            {
                block.PlaceBlock(_wordisSettings.IsWaterZone(block.RowId)
                    ? Block.WaterTag
                    : block.defaultSpriteTag);
                block.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
            }
            else
            {
                block.PlaceBlock(Block.DefaultCharTag);

                if (wordisObj is WordisChar wordisChar)
                {
                    block.GetComponentInChildren<TextMeshProUGUI>().text =
                        $"{wordisChar.Value}";
                }
            }
        }
    }
}