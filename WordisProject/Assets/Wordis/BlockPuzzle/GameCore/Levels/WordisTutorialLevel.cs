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
                speed: 1.5f,
                width: 5,
                height: 5,
                minWordMatch: 4);

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
                    new WordLetters(new TutorialSequence().Word));
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
        public string Goal => "See what's going on here";

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
                        if (Game.GameEvents.Count == 0)
                        {
                            _displayMessage(Goal);
                        }

                        if (Game.GameEvents.Count == 1)
                        {
                            _displayMessage("Your goal is to match the words");
                        }

                        if (Game.GameEvents.Count == 2)
                        {
                            _displayMessage("Move the letter right");
                            updatedGame = Game.Handle(GameEvent.Right);
                        }

                        if (Game.GameEvents.Count == 3)
                        {
                            _displayMessage("Move the letter left");
                            updatedGame = Game.Handle(GameEvent.Left);
                        }

                        if (Game.GameEvents.Count == 4 ||
                            Game.GameEvents.Count == 5 ||
                            Game.GameEvents.Count == 6 ||
                            Game.GameEvents.Count == 7)
                        {
                            updatedGame = Game.Handle(GameEvent.Left);
                        }

                        if (Game.GameEvents.Count == 8)
                        {
                            updatedGame = Game.Handle(GameEvent.Left);
                        }

                        return With(updatedGame ?? Game.Handle(gameEvent));
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

        private class TutorialSequence : WordsSequence
        {
            public static readonly IReadOnlyList<string> Words = new[]
            {
                "hello",
                //"world"
            };

            public TutorialSequence() : base(Words)
            {
            }
        }
    }
}
