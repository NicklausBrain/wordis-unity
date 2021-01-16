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
    public enum GameOverReason
    {
        GridFilled, // If there is no enough space to place existing blocks. Applies to all game mode.
        TimeOver, // If timer finishing. Applied only to time mode.
    }

    public class GamePlayUI : Singleton<GamePlayUI>
    {
        #region Wordis

        private readonly object _gameLock = new object();
        public WordisSettings wordisSettings = new WordisSettings(9, 9, 3);
        public WordisGame wordisGame;

        private void GameStep()
        {
            HandleGameEvent(GameEvent.Step);
        }

        public void HandleGameEvent(GameEvent gameEvent)
        {
            lock (_gameLock)
            {
                if (wordisGame.IsGameOver)
                {
                    // stop the game cycle
                    CancelInvoke(nameof(GameStep));
                    OnGameOver();
                }

                var lastGame = wordisGame;
                var newGame = wordisGame.Handle(gameEvent);
                wordisGame = newGame;
                RefreshPresentation(lastGame, newGame);
            }
        }

        #endregion Wordis

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

        [NonSerialized] public GameModeSettings currentModeSettings;

        // Stores current playing mode.
        [NonSerialized] public GameMode currentGameMode;

        // GamePlay Setting Scriptable Instance. Initializes on awake.
        [NonSerialized] GamePlaySettings gamePlaySettings;

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

        // Rescue used for the game or not.
        [HideInInspector] public bool rescueDone = false;

        // Reason for game over. Will Initialize at game over or rescue.
        [HideInInspector] public GameOverReason currentGameOverReason;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Initializes the GamePlay Settings Scriptable.
            if (gamePlaySettings == null)
            {
                gamePlaySettings = (GamePlaySettings)Resources.Load("GamePlaySettings");
            }
        }

        /// <summary>
        /// Starts game with selected game mode.
        /// </summary>
        public void StartGamePlay(GameMode gameMode)
        {
            #region
            wordisGame = new WordisGame(wordisSettings);
            InvokeRepeating(nameof(GameStep), 1, 1);
            #endregion

            currentGameMode = gameMode;
            currentModeSettings = gamePlaySettings.classicModeSettings;

            // Checks if the there is user progress from previous session.
            bool hasPreviousSessionProgress = GameProgressTracker.Instance.HasGameProgress(currentGameMode);
            if (hasPreviousSessionProgress)
            {
                progressData = GameProgressTracker.Instance.GetGameProgress(Instance.currentGameMode);
            }

            // Enables gameplay screen if not active.
            if (!gamePlay.gameObject.activeSelf)
            {
                gamePlay.gameObject.SetActive(true);
            }

            // Generated gameplay grid.
            gamePlay.boardGenerator.GenerateBoard(progressData);

            // Board Generator will create and initialize board with progress data if available.

            OnGameStartedEvent?.Invoke(currentGameMode);
        }

        /// <summary>
        /// Returns size of the current grid.
        /// </summary>
        /// <returns></returns>
        public BoardSize GetBoardSize()
        {
            return currentModeSettings.boardSize;
        }

        // Returns of list of all standard block shapes.
        public List<BlockShapeInfo> GetStandardBlockShapesInfo()
        {
            return gamePlaySettings.standardBlockShapesInfo.ToList();
        }

        // Returns of list of all advanced block shapes.
        public List<BlockShapeInfo> GetAdvancedBlockShapesInfo()
        {
            return gamePlaySettings.advancedBlockShapesInfo.ToList();
        }

        // Returns score to be added on for each block cleared.
        public int blockScore => gamePlaySettings.blockScore;

        // Returns score to be added on for each line cleared.
        public int singleLineBreakScore => gamePlaySettings.singleLineBreakScore;

        // Returns score multiplier to be added on for each line cleared when more than 1 lines cleared together.
        public int multiLineScoreMultiplier => gamePlaySettings.multiLineScoreMultiplier;

        public bool rewardOnGameOver => gamePlaySettings.rewardOnGameOver;

        public bool giveFixedReward => gamePlaySettings.giveFixedReward;

        public int fixedRewardAmount => gamePlaySettings.fixedRewardAmount;

        public float rewardPerLine => gamePlaySettings.rewardPerLineCompleted;

        // Invokes callback for OnShapePlaced Event.
        public void OnShapePlaced()
        {
            OnShapePlacedEvent?.Invoke();
        }

        /// <summary>
        /// Will be called on game over. 
        /// </summary>
        public void OnGameOver()
        {
            OnGameOverEvent?.Invoke(currentGameMode);
            UIController.Instance.gameOverScreen
                .GetComponent<GameOver>()
                .SetGameData(
                    currentGameOverReason,
                    scoreManager.GetScore(),
                    totalLinesCompleted,
                    currentGameMode);

            UIController.Instance.gameOverScreen.Activate();
        }

        /// <summary>
        /// Resets game.
        /// </summary>
        public void ResetGame()
        {
            wordisGame = new WordisGame(wordisSettings);

            progressData = null;
            totalLinesCompleted = 0;
            rescueDone = false;
            gamePlay.ResetGame();
            scoreManager.ResetGame();
        }

        /// <summary>
        /// Pauses game.
        /// </summary>
        public void PauseGame()
        {
            CancelInvoke(nameof(GameStep));
            OnGamePausedEvent?.Invoke(currentGameMode, true);
        }

        /// <summary>
        /// Resumes game.
        /// </summary>
        public void ResumeGame()
        {
            OnGamePausedEvent?.Invoke(currentGameMode, false);
        }

        /// <summary>
        /// Will rest game to empty state and start new game with same selected mode.
        /// </summary>
        public void RestartGame()
        {
            GameProgressTracker.Instance.ClearProgressData();
            ResetGame();
            StartGamePlay(currentGameMode);
        }

        #region Wordis

        private BlockShape GetBasicBlockShape()
        {
            var basicBlockInfo = GamePlayUI.Instance.GetStandardBlockShapesInfo().First();
            var basicBlockShape = basicBlockInfo.blockShape.GetComponent<BlockShape>();
            basicBlockShape.SetSpriteTag(basicBlockInfo.blockSpriteTag);
            return basicBlockShape;
        }

        private void RefreshPresentation(
            WordisGame lastGameState, WordisGame newGameState)
        {
            var newObjects =
                newGameState.GameObjects
                    .Except(lastGameState.GameObjects)
                    .ToArray();
            var removedObjects =
                lastGameState.GameObjects
                    .Except(newGameState.GameObjects)
                    .ToArray();
            var newMatches =
                newGameState.Matches
                    .Except(lastGameState.Matches)
                    .ToArray();

            if (newMatches.Any()) // on word matches
            {
                // 1. display matched words
                foreach (var match in newMatches)
                {
                    inGameMessage.ShowMessage(match.Word);
                }

                var blocksToClear =
                    newMatches
                        .SelectMany(match => match.MatchedChars)
                        .Select(c => gamePlay.allColumns[c.X][c.Y])
                        .ToArray();

                // 2. display score (todo: check calculation logic)
                scoreManager.AddScore(newMatches.Length, blocksToClear.Length);

                // 3. animate blocks destruction
                StartCoroutine(GamePlay.ClearAllBlocks(blocksToClear));

                // 4. play break sound
                AudioController.Instance.PlayLineBreakSound(blocksToClear.Length);
            }

            if (removedObjects.Any())
            {
                var blocksToClear =
                    removedObjects
                        .Select(c => gamePlay.allColumns[c.X][c.Y])
                        .ToArray();

                foreach (Block block in blocksToClear)
                {
                    block.PlaceBlock(block.defaultSpriteTag);
                    block.GetComponentInChildren<TextMeshProUGUI>().text = string.Empty;
                }
                // todo: add animation instead like: block.Fade()
                //StartCoroutine(GamePlay.ClearAllBlocks(blocksToClear));
            }

            if (newObjects.Any())
            {
                var blocksToCreate =
                    newObjects
                        .Select(o => (wordisObj: o, block: gamePlay.allColumns[o.X][o.Y]))
                        .ToArray();

                foreach (var pair in blocksToCreate)
                {
                    var basicShape = GetBasicBlockShape();
                    pair.block.PlaceBlock(basicShape.spriteTag);

                    if (pair.wordisObj is WordisChar)
                    {
                        pair.block.GetComponentInChildren<TextMeshProUGUI>().text =
                            $"{((WordisChar)pair.wordisObj).Value}";
                    }
                }
            }
        }

        #endregion
    }
}