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
using System.Linq;
using System.Threading;
using Assets.Wordis.BlockPuzzle.GameCore;
using Assets.Wordis.BlockPuzzle.GameCore.Levels;
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
    /// <summary>
    /// Encapsulates UI logic for the gameplay.
    /// Includes game events handling and UI refresh cycle.
    /// </summary>
    public class GamePlayUI : Singleton<GamePlayUI>
    {
        private static readonly object GameLock = new object();

        private IWordisGameLevel _wordisGameLevel = new WordisSurvivalMode();

        private void GameStep() => HandleGameEvent(GameEvent.Step);

        /// <summary>
        /// Starts / resumes the game.
        /// </summary>
        public void ResumeGame()
        {
            PauseGame(); // to prevent double callback
            InvokeRepeating(
                nameof(GameStep),
                _wordisGameLevel.Settings.Speed,
                _wordisGameLevel.Settings.Speed);
        }

        /// <summary>
        /// Stops / pauses the game.
        /// </summary>
        public void PauseGame() => CancelInvoke(nameof(GameStep));

        public void HandleGameEvent(GameEvent gameEvent)
        {
            lock (GameLock)
            {
                PauseGame(); // prevent premature UI refresh

                if (_wordisGameLevel.IsCompleted)
                {
                    // stop the game cycle
                    PauseGame();
                    GameOver();
                    return;
                }

                var updatedLevel = _wordisGameLevel.Handle(gameEvent);

                if (!string.IsNullOrWhiteSpace(updatedLevel.Message))
                {
                    inGameMessage.ShowMessage(updatedLevel.Message);
                }

                if (updatedLevel.Game.GameEvents.Count >
                    _wordisGameLevel.Game.GameEvents.Count) // avoid extra refresh on game over.
                {
                    RefreshPresentation(updatedLevel.Game);
                }

                _wordisGameLevel = updatedLevel;

                ResumeGame();
            }
        }

        /// <summary>
        /// Sets the level to be played.
        /// </summary>
        public GamePlayUI SetLevel(IWordisGameLevel gameLevel = null)
        {
            _wordisGameLevel =
                gameLevel?.WithOutput(message =>
                {
                    inGameMessage.ShowMessage(message);
                    Debug.LogWarning(message);
                }) ??
                new WordisSurvivalMode();

            return this;
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
            gameBoard.boardGenerator.GenerateBoard(_wordisGameLevel.Settings);
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
                    _wordisGameLevel.Game.Score.Value,
                    _wordisGameLevel.Game.Matches.Count);

            UIController.Instance.gameOverScreen.Activate();
        }

        private void ClearGame()
        {
            PauseGame();
            gameBoard.Clear();
            scoreManager.Clear();
            GameProgressTracker.Instance.ClearProgressData();
            _wordisGameLevel = _wordisGameLevel.Reset();
        }

        private void RefreshPresentation(WordisGame gameState)
        {
            if (gameState.Matches.Last.Any()) // on word matches
            {
                DisplayMatches(gameState);
            }

            for (int x = 0; x < gameState.Matrix.Width; x++)
            {
                for (int y = 0; y < gameState.Matrix.Height; y++)
                {
                    var wordisObject = gameState.Matrix[x, y];

                    RefreshVisualBlock(x, y, wordisObject);
                }
            }
        }

        private void DisplayMatches(WordisGame gameState)
        {
            var newMatches = gameState.Matches.Last;

            // 1. display matched words
            foreach (var match in newMatches)
            {
                inGameMessage.ShowMessage(match.Word);
            }

            var blocksToClear =
                newMatches
                    .SelectMany(match => match.MatchedChars)
                    .Select(c => gameBoard.allColumns[c.X][c.Y])
                    .ToArray();

            // 2. display score
            scoreManager.ShowScore(gameState.Score.Value);

            // 3. animate blocks destruction
            StartCoroutine(GameBoard.ClearAllBlocks(_wordisGameLevel.Settings, blocksToClear));

            // 4. play break sound
            AudioController.Instance.PlayLineBreakSound(blocksToClear.Length);
        }

        private void RefreshVisualBlock(int x, int y, WordisObj wordisObj)
        {
            var block = gameBoard.allColumns[x][y];

            if (wordisObj == null)
            {
                block.PlaceBlock(_wordisGameLevel.Settings.IsWaterZone(block.RowId)
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

        private IEnumerator ShowMessage(string message)
        {
            inGameMessage.ShowMessage(message);
            yield return new WaitForSeconds(1.5F);
        }
    }
}