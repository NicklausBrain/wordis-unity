using System;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
using Assets.Wordis.BlockPuzzle.GameCore.Words;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels
{
    /// <summary>
    /// Game tutorial.
    /// </summary>
    public class WordisTutorialLevel : WordisGameLevelBase<WordisTutorialLevel>, IWordisGameLevel
    {
        private static readonly WordisSettings TutorialLevelSettings =
            new WordisSettings(
                speed: 1f,
                width: 3,
                height: 5,
                minWordMatch: 3);

        private static WordsSequence TutorialSequence => WordsSequence.FromCsv("CAT");

        private readonly Action<string> _displayMessage;

        private WordisTutorialLevel(
            Action<string> displayMessage,
            WordisGame game) : base(game)
        {
            _displayMessage =
                displayMessage ??
                Console.WriteLine;
        }

        /// <summary>
        /// Creates a level in its default state.
        /// </summary>
        public WordisTutorialLevel() : this(
            null,
            new WordisGame(
                TutorialLevelSettings,
                new WordLetters(TutorialSequence.Word)))
        {
        }

        /// <inheritdoc cref="IWordisGameLevel" />
        public override string Title => "Tutorial";

        /// <inheritdoc cref="IWordisGameLevel" />
        public override string Goal => "Match the words";

        /// <inheritdoc cref="IWordisGameLevel" />
        public override bool IsCompleted =>
            Game.Matches.All
                .Select(m => m.Word)
                .Intersect(TutorialSequence.Words)
                .Count() == TutorialSequence.Words.Count;

        /// <inheritdoc cref="IWordisGameLevel" />
        public override bool IsFailed => Game.IsGameOver;

        /// <inheritdoc cref="IWordisGameLevel" />
        public override IWordisGameLevel Handle(GameEvent gameEvent)
        {
            switch (gameEvent)
            {
                case GameEvent.Step:
                    {
                        WordisGame updatedGame = null;

                        if (Game.GameEvents.Count == 3)
                            _displayMessage("Swipe left"); // todo: localize
                        if (Game.GameEvents.Count == 4)
                            updatedGame = Game.Handle(GameEvent.Left);
                        if (Game.GameEvents.Count == 6)
                            _displayMessage("Swipe down"); // todo: localize
                        if (Game.GameEvents.Count == 7)
                            updatedGame = Game.Handle(GameEvent.Down);
                        if (Game.GameEvents.Count == 9)
                            _displayMessage("Swipe right"); // todo: localize
                        if (Game.GameEvents.Count == 10)
                            updatedGame = Game.Handle(GameEvent.Right);

                        return With(
                            updatedGame: updatedGame ?? Game.Handle(gameEvent));
                    }
                default:
                    return this;
            }
        }

        /// <inheritdoc cref="IWordisGameLevel" />
        public override IWordisGameLevel WithOutput(Action<string> outFunc) =>
            With(outFunc: outFunc);

        /// <inheritdoc cref="IWordisGameLevel" />
        public override WordisTutorialLevel WithUpdatedGame(WordisGame updatedGame) =>
            With(updatedGame);

        private WordisTutorialLevel With(
            WordisGame updatedGame = null,
            Action<string> outFunc = null) =>
            new WordisTutorialLevel(
                outFunc ?? _displayMessage,
                updatedGame ?? Game);
    }
}
