using System;
using Assets.Wordis.BlockPuzzle.GameCore.Words;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels
{
    /// <summary>
    /// 3 letters match. Basic palindromes.
    /// </summary>
    public class Level1BasicPalindromes : IWordisGameLevel
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

        private Level1BasicPalindromes(WordisGame game)
        {
            Game = game;
        }

        public Level1BasicPalindromes() : this(
            new WordisGame(
                LevelSettings,
                BasicPalindromes.AsLetterSource()))
        {
        }

        public WordisGame Game { get; }

        public WordisSettings Settings => Game.Settings;

        public string Title => "Basic palindromes";

        public string Goal => $"Match {NeededMatches} words";

        public bool IsCompleted => Game.Matches.Count >= NeededMatches;

        public bool IsFailed => Game.IsGameOver;

        public IWordisGameLevel Handle(GameEvent gameEvent)
        {
            var updatedGame = Game.Handle(gameEvent);

            return new Level1BasicPalindromes(updatedGame);
        }

        public IWordisGameLevel Reset() => new Level1BasicPalindromes();

        public IWordisGameLevel WithOutput(Action<string> outFunc) => this;
    }
}
