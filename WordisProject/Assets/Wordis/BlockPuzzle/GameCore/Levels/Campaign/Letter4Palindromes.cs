using System;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Words;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels.Campaign
{
    /// <summary>
    /// 3 letters match. Basic palindromes.
    /// </summary>
    public class Letter4Palindromes : IWordisGameLevel // try abstract 1 more
    {
        private const int NeededMatches = 4;

        /// <summary>
        /// https://en.wiktionary.org/wiki/Appendix:English_palindromes#Three_letters
        /// </summary>
        private static WordsSequence FourLetterPalindromes => WordsSequence.FromCsv(
            "peep,esse,anna,deed,poop,kook,noon");

        private static readonly WordisSettings LevelSettings = new WordisSettings(
            width: 5,
            height: 6,
            minWordMatch: 4,
            waterLevel: 0);

        private Letter4Palindromes(WordisGame game)
        {
            Game = game;
        }

        public Letter4Palindromes() : this(
            new WordisGame(
                LevelSettings,
                FourLetterPalindromes.AsLetterSource()))
        {
        }

        public WordisGame Game { get; }

        public WordisSettings Settings => Game.Settings;

        public string Title => "4-letter palindromes";

        public string Goal => $"Match {NeededMatches} palindromes";

        public bool IsCompleted =>
            Game.Matches.All
                .Select(m => m.Word)
                .Intersect(FourLetterPalindromes.Words, StringComparer.OrdinalIgnoreCase)
                .Count() >= NeededMatches;

        public bool IsFailed => Game.IsGameOver;

        public IWordisGameLevel Handle(GameEvent gameEvent)
        {
            var updatedGame = Game.Handle(gameEvent);

            return new Letter4Palindromes(updatedGame);
        }

        public IWordisGameLevel Reset() => new Letter4Palindromes();

        public IWordisGameLevel WithOutput(Action<string> outFunc) => this;
    }
}
