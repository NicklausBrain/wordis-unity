using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Words;
using NUnit.Framework.Constraints;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels
{
    /// <summary>
    /// Game tutorial.
    /// </summary>
    public class WordisTutorialLevel : IWordisGameLevel
    {
        private readonly Action<string> _displayMessage;

        private static readonly WordisSettings TutorialLevelSettings =
            new WordisSettings(
                speed: 1f,
                width: 3,
                height: 5,
                minWordMatch: 3);

        private WordisTutorialLevel(
            Action<string> displayMessage = null,
            WordisGame gameState = null,
            string message = null)
        {
            Message = message;
            _displayMessage =
                displayMessage ??
                Console.WriteLine;
            Game =
                gameState ??
                new WordisGame(
                    TutorialLevelSettings,
                    new WordLetters(new TutorialSequence().Word));
        }

        public WordisTutorialLevel() : this(null, null)
        {
        }

        public string Message { get; }

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
            Game.IsGameOver ||
            Game.Matches.All
                .Select(m => m.Word)
                .Intersect(TutorialSequence.Words)
                .Count() == TutorialSequence.Words.Count;

        /// <inheritdoc />
        public IWordisGameLevel Handle(GameEvent gameEvent)
        {
            switch (gameEvent)
            {
                case GameEvent.Step:
                    {
                        WordisGame updatedGame = null;
                        string message = null;

                        if (Game.GameEvents.Count == 4)
                        {
                            message = "Swipe left";
                            updatedGame = Game.Handle(GameEvent.Left);
                        }

                        if (Game.GameEvents.Count == 7)
                        {
                            message = "Swipe down";
                            updatedGame = Game.Handle(GameEvent.Down);
                        }

                        if (Game.GameEvents.Count == 10)
                        {
                            message = "Swipe right";
                            updatedGame = Game.Handle(GameEvent.Right);
                        }

                        return With(
                            updatedGame: updatedGame ?? Game.Handle(gameEvent),
                            message: message);
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
            WordisGame updatedGame,
            string message = null) =>
            new WordisTutorialLevel(
                _displayMessage,
                updatedGame,
                message);

        private class TutorialSequence : WordsSequence
        {
            public static readonly IReadOnlyList<string> Words = new[]
            {
                "CAT",
            };

            public TutorialSequence() : base(Words)
            {
            }
        }
    }
}
