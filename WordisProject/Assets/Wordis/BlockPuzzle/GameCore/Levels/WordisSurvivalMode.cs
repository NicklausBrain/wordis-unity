using System;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels
{
    /// <summary>
    /// Default game mode, aka endless mode.
    /// </summary>
    public class WordisSurvivalMode : IWordisGameLevel
    {
        private static readonly WordisSettings DefaultSettings = new WordisSettings(
            width: 9,
            height: 9,
            minWordMatch: 3,
            waterLevel: 0);

        private WordisSurvivalMode(WordisGame game)
        {
            Game = game;
        }

        public WordisSurvivalMode() : this(
            new WordisGame(DefaultSettings))
        {
        }

        /// <inheritdoc />
        public WordisGame Game { get; }

        /// <inheritdoc />
        public WordisSettings Settings => Game.Settings;

        /// <inheritdoc />
        public string Title => "Survival mode";

        /// <inheritdoc />
        public string Goal => "How long could you persist?";

        /// <inheritdoc />
        public bool IsCompleted => false;

        /// <inheritdoc />
        public bool IsFailed => Game.IsGameOver;

        /// <inheritdoc />
        public IWordisGameLevel Handle(GameEvent gameEvent)
        {
            var updatedGame = Game.Handle(gameEvent);

            return new WordisSurvivalMode(updatedGame);
        }

        public IWordisGameLevel Reset() => new WordisSurvivalMode();

        public IWordisGameLevel WithOutput(Action<string> outFunc) => this;
    }
}
