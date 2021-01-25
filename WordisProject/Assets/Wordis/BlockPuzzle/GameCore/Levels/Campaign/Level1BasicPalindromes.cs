using System;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
using Assets.Wordis.BlockPuzzle.GameCore.Words;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels.Campaign
{
    /// <summary>
    /// 3 letters match. Basic palindromes.
    /// </summary>
    public class Level1BasicPalindromes : WordisGameLevelBase<Level1BasicPalindromes>, IWordisGameLevel
    {
        private const int NeededMatches = 5;

        /// <summary>
        /// https://en.wiktionary.org/wiki/Appendix:English_palindromes#Three_letters
        /// </summary>
        private static WordsSequence BasicPalindromes => WordsSequence.FromCsv(
            "lol,wow,eve,mom,dad,gig,pop,pup,ewe,did,gig,bob");

        private static readonly WordisSettings LevelSettings = new WordisSettings(
            width: 5,
            height: 6,
            minWordMatch: 3,
            waterLevel: 0);

        private Level1BasicPalindromes(WordisGame game) : base(game)
        {
        }

        /// <summary>
        /// Creates a level in its default state.
        /// </summary>
        public Level1BasicPalindromes() : this(
            new WordisGame(
                LevelSettings,
                BasicPalindromes.AsLetterSource()))
        {
        }

        /// <inheritdoc cref="IWordisGameLevel" />
        public override string Title => "Basic palindromes";

        /// <inheritdoc cref="IWordisGameLevel" />
        public override string Goal => $"Match {NeededMatches} words";

        /// <inheritdoc cref="IWordisGameLevel" />
        public override bool IsCompleted =>
            Game.Matches.Count >= NeededMatches;

        /// <inheritdoc />
        public override Level1BasicPalindromes WithUpdatedGame(WordisGame updatedGame) =>
            new Level1BasicPalindromes(updatedGame);
    }
}
