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
using Assets.Wordis.BlockPuzzle.GameCore.Functions;
using Assets.Wordis.BlockPuzzle.GameCore.Levels;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
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
        private static IWordisGameLevel DefaultLevel => new WordisSurvivalMode();

        private static readonly object GameLock = new object();

        private static readonly DefineEngWordFunc DefineWordFunc = new DefineEngWordFunc();

        private IWordisGameLevel _wordisGameLevel = DefaultLevel;

        private void GameStep() => HandleGameEvent(GameEvent.Step);

        /// <summary>
        /// Starts / resumes the game.
        /// </summary>
        public void ResumeGame()
        {
            PauseGame(); // to prevent double callback
            Invoke(nameof(GameStep), _wordisGameLevel.Settings.Speed);
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

                if (_wordisGameLevel.IsCompleted || _wordisGameLevel.Game.IsGameOver)
                {
                    // stop the game cycle
                    PauseGame();
                    GameOver();
                    return;
                }

                var updatedLevel = _wordisGameLevel.Handle(gameEvent);

                if (updatedLevel.Game.GameEvents.Count >
                    _wordisGameLevel.Game.GameEvents.Count) // avoid extra refresh on game over.
                {
                    RefreshPresentation(updatedLevel.Game);
                    ShowProgress(updatedLevel.Progress);
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
            _wordisGameLevel = gameLevel ?? DefaultLevel;

            return this;
        }

        /// <summary>
        /// Gets active level.
        /// </summary>
        public IWordisGameLevel CurrentLevel => _wordisGameLevel;

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
            scoreManager.Init(_wordisGameLevel.Title);

            ShowMessage(_wordisGameLevel.Title); // move to TIP area? move to level?

            ShowMessage(_wordisGameLevel.Goal); // move to level?

            DefineWordFunc.WarmUp(); // preload word definitions

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

                GameProgressTracker.Instance.SaveSession();
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
                .SetGameData( // TODO: remove extra args
                    _wordisGameLevel.Game.Score.Value,
                    _wordisGameLevel.Game.Matches.Count,
                    _wordisGameLevel);

            UIController.Instance.gameOverScreen.Activate();
        }

        private void ClearGame()
        {
            PauseGame();
            gameBoard.Clear();
            scoreManager.Clear();
            GameProgressTracker.Instance.ClearProgressData();
            _wordisGameLevel = _wordisGameLevel
                .Reset()
                .WithOutput(message =>
                {
                    ShowMessage(message);
                    Debug.LogWarning(message);
                });

            UIController.Instance.HideTips();
        }

        private void RefreshPresentation(WordisGame gameState)
        {
            // check last game event to avoid extra animations on user input
            if (gameState.LastEvent == GameEvent.Step &&
                gameState.Matches.Last.Any()) // on word match
            {
                DisplayMatches(gameState);
            }

            var activeChar = gameState.ActiveChar;

            for (int x = 0; x < gameState.Matrix.Width; x++)
            {
                for (int y = 0; y < gameState.Matrix.Height; y++)
                {
                    var wordisObject = gameState.Matrix[x, y];

                    RefreshVisualBlock(x, y, wordisObject, activeChar);
                }
            }
        }

        private void DisplayMatches(WordisGame gameState)
        {
            var newMatches = gameState.Matches.Last;

            // 1. display matched words
            foreach (var match in newMatches)
            {
                ShowMessage(match.Word);
                ShowWordDefinition(match.Word);
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

        private void RefreshVisualBlock(
            int x,
            int y,
            WordisObj wordisObj,
            ActiveChar activeChar)
        {
            var block = gameBoard.allColumns[x][y];

            if (wordisObj == null)
            {
                if (activeChar != null && y > activeChar.Y && x == activeChar.X)
                {
                    // Highlight the trajectory
                    block.Highlight(Block.ActiveCharSprite.Value);
                }
                else
                {
                    block.PlaceBlock(_wordisGameLevel.Settings.IsWaterZone(block.RowId)
                        ? Block.WaterTag
                        : block.defaultSpriteTag);
                    block.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
                }
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

        private void ShowMessage(string message)
        {
            inGameMessage.ShowMessage(message);
        }

        private void ShowWordDefinition(string word)
        {
            var definitions = DefineWordFunc.Invoke(word);

            if (definitions.Any())
            {
                UIController.Instance.ShowTopTipAtPosition(
                    tipPosition: new Vector2(0, -250F), // todo: make default, dont specify in code
                    anchor: new Vector2(0.5F, 1), // todo: make default, dont specify in code
                    tipText: definitions[0].Definition,
                    duration: 7F);
            }
        }

        /// <summary>
        /// Try to display level progression.
        /// </summary>
        private void ShowProgress(string levelProgress)
        {
            if (!string.IsNullOrWhiteSpace(levelProgress))
            {
                UIController.Instance.ShowDownTipAtPosition(
                    tipPosition: new Vector2(0, 400F), // todo: make default, dont specify in code
                    anchor: new Vector2(0.5F, 0), // todo: make default, dont specify in code
                    tipText: levelProgress,
                    duration: 7F);
            }
        }
    }
}