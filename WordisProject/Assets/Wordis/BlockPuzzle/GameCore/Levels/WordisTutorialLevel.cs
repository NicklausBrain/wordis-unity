using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Words;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels
{
    /// <summary>
    /// Game tutorial.
    /// </summary>
    public class WordisTutorialLevel : IWordisGameLevel
    {
        private static WordsSequence TutorialSequence => WordsSequence.FromCsv("CAT");

        private static readonly WordisSettings TutorialLevelSettings =
            new WordisSettings(
                speed: 1f,
                width: 3,
                height: 5,
                minWordMatch: 3);

        private readonly Action<string> _displayMessage;

        private WordisTutorialLevel(
            Action<string> displayMessage = null,
            WordisGame gameState = null)
        {
            _displayMessage =
                displayMessage ??
                Console.WriteLine;
            Game =
                gameState ??
                new WordisGame(
                    TutorialLevelSettings,
                    new WordLetters(TutorialSequence.Word));
        }

        public WordisTutorialLevel() : this(null, null)
        {
        }

        /// <inheritdoc />
        public WordisGame Game { get; }

        /// <inheritdoc />
        public WordisSettings Settings => Game.Settings;

        /// <inheritdoc />
        public string Title => "Tutorial";

        /// <inheritdoc />
        public string Goal => "Match the words";

        /// <inheritdoc />
        public bool IsCompleted =>
            Game.Matches.All
                .Select(m => m.Word)
                .Intersect(TutorialSequence.Words)
                .Count() == TutorialSequence.Words.Count;

        /// <inheritdoc />
        public bool IsFailed => Game.IsGameOver;

        /// <inheritdoc />
        public IWordisGameLevel Handle(GameEvent gameEvent)
        {
            switch (gameEvent)
            {
                case GameEvent.Step:
                    {
                        WordisGame updatedGame = null;

                        if (Game.GameEvents.Count == 4)
                        {
                            _displayMessage("Swipe left"); // todo: localize
                            updatedGame = Game.Handle(GameEvent.Left);
                        }

                        if (Game.GameEvents.Count == 7)
                        {
                            _displayMessage("Swipe down"); // todo: localize
                            updatedGame = Game.Handle(GameEvent.Down);
                        }

                        if (Game.GameEvents.Count == 10)
                        {
                            _displayMessage("Swipe right"); // todo: localize
                            updatedGame = Game.Handle(GameEvent.Right);
                        }

                        return With(
                            updatedGame: updatedGame ?? Game.Handle(gameEvent));
                    }
                default:
                    return this;
            }
        }

        public IWordisGameLevel Reset() =>
            new WordisTutorialLevel(displayMessage: _displayMessage);

        public IWordisGameLevel WithOutput(Action<string> outFunc) =>
            new WordisTutorialLevel(displayMessage: outFunc);

        private WordisTutorialLevel With(
            WordisGame updatedGame) =>
            new WordisTutorialLevel(
                _displayMessage,
                updatedGame);
    }
}
