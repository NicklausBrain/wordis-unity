using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels
{
    /// <summary>
    /// Default game mode, aka endless mode.
    /// </summary>
    public class WordisSurvivalMode : WordisGameLevelBase<WordisSurvivalMode>, IWordisGameLevel
    {
        private static readonly WordisSettings DefaultSettings = new WordisSettings(
            width: 9,
            height: 9,
            minWordLength: 3,
            waterLevel: 0);

        private WordisSurvivalMode(WordisGame game) : base(game)
        {
        }

        /// <summary>
        /// Creates a level in its default state.
        /// </summary>
        public WordisSurvivalMode() : this(
            new WordisGame(DefaultSettings))
        {
        }

        /// <inheritdoc cref="IWordisGameLevel.Title" />
        public override string Title => "Survival mode";

        /// <inheritdoc cref="IWordisGameLevel.Goal" />
        public override string Goal => "How long could you persist?";

        public override string Progress => $"{Game.Matches.Count} words matched";

        /// <inheritdoc cref="WordisGameLevelBase{T}.WithUpdatedGame(WordisGame)" />
        public override WordisSurvivalMode WithUpdatedGame(WordisGame updatedGame) =>
            new WordisSurvivalMode(updatedGame);
    }
}
