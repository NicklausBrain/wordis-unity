﻿using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
using Assets.Wordis.BlockPuzzle.GameCore.Words;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels.Campaign
{
    /// <summary>
    /// 3 letters match. Basic palindromes.
    /// </summary>
    public class Letter3Palindromes : WordisGameLevelBase<Letter3Palindromes>, IWordisGameLevel
    {
        public const int NeededMatches = 10;

        public static readonly WordisSettings LevelSettings = new WordisSettings(
            width: 5,
            height: 6,
            minWordMatch: 3,
            waterLevel: 0);

        /// <summary>
        /// https://en.wiktionary.org/wiki/Appendix:English_palindromes#Three_letters
        /// </summary>
        public static WordsSequence ThreeLetterPalindromes => WordsSequence.FromCsv(
            "lol,wow,eve,mom,dad,gig,pop,pup,ewe,did,gig,bob");

        private Letter3Palindromes(WordisGame game) : base(game)
        {
        }

        /// <summary>
        /// Creates a level in its default state.
        /// </summary>
        public Letter3Palindromes() : this(
            new WordisGame(
                LevelSettings,
                ThreeLetterPalindromes
                    .Shuffle()
                    .AsLetterSource()))
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
        public override Letter3Palindromes WithUpdatedGame(WordisGame updatedGame) =>
            new Letter3Palindromes(updatedGame);
    }
}