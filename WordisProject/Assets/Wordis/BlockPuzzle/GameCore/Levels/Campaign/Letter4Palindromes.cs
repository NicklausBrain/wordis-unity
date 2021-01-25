using System;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
using Assets.Wordis.BlockPuzzle.GameCore.Words;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels.Campaign
{
    /// <summary>
    /// 4-letters palindromes. 4-letters match.
    /// </summary>
    public class Letter4Palindromes : WordisGameLevelBase<Letter4Palindromes>, IWordisGameLevel
    {
        public const int NeededMatches = 4;

        public static readonly WordisSettings LevelSettings = new WordisSettings(
            width: 5,
            height: 6,
            minWordMatch: 4,
            waterLevel: 0);

        /// <summary>
        /// https://en.wiktionary.org/wiki/Appendix:English_palindromes#Four_letters
        /// </summary>
        public static WordsSequence FourLetterPalindromes => WordsSequence.FromCsv(
            "peep,esse,anna,deed,poop,kook,noon");

        private Letter4Palindromes(WordisGame game) : base(game)
        {
        }

        /// <summary>
        /// Creates a level in its default state.
        /// </summary>
        public Letter4Palindromes() : this(
            new WordisGame(
                LevelSettings,
                FourLetterPalindromes
                    .Shuffle()
                    .AsLetterSource()))
        {
        }

        /// <inheritdoc cref="IWordisGameLevel" />
        public override string Title => "4-letter palindromes";

        /// <inheritdoc cref="IWordisGameLevel" />
        public override string Goal => $"Match {NeededMatches} palindromes!";

        /// <inheritdoc cref="IWordisGameLevel" />
        public override bool IsCompleted =>
            Game.Matches.All
                .Select(m => m.Word)
                .Intersect(FourLetterPalindromes.Words, StringComparer.OrdinalIgnoreCase)
                .Count() >= NeededMatches;

        /// <inheritdoc />
        public override Letter4Palindromes WithUpdatedGame(WordisGame updatedGame) =>
            new Letter4Palindromes(updatedGame);
    }
}
