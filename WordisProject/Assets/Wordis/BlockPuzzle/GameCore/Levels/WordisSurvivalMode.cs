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
            minWordMatch: 3,
            waterLevel: 0);

        private WordisSurvivalMode(WordisGame game) : base(game)
        {
        }

        /// <summary>
        /// Creates a level in default state.
        /// </summary>
        public WordisSurvivalMode() : this(
            new WordisGame(DefaultSettings))
        {
        }

        /// <inheritdoc cref="IWordisGameLevel" />
        public override string Title => "Survival mode";

        /// <inheritdoc cref="IWordisGameLevel" />
        public override string Goal => "How long could you persist?";

        /// <inheritdoc cref="WordisGameLevelBase{T}" />
        public override WordisSurvivalMode WithUpdatedGame(WordisGame updatedGame) =>
            new WordisSurvivalMode(updatedGame);
    }
}
