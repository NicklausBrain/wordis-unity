using System;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;
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
                minWordLength: 3);

        private static WordsSequence TutorialSequence => WordsSequence.FromCsv("CAT");

        private readonly Action<string> _displayMessage;

        /// <summary>
        /// Steps counter to handle the tips scenario.
        /// </summary>
        private readonly int _steps;

        private WordisTutorialLevel(
            Action<string> displayMessage,
            WordisGame game,
            int steps) : base(game)
        {
            _steps = steps;
            _displayMessage =
                displayMessage ??
                Console.WriteLine;
        }

        private WordisTutorialLevel(Action<string> displayMessage = null) : this(
            displayMessage,
            new WordisGame(
                TutorialLevelSettings,
                new WordLetters(TutorialSequence.Word)),
            0)
        {
        }

        /// <summary>
        /// Creates a level in its default state.
        /// </summary>
        public WordisTutorialLevel() : this(null)
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
        public override IWordisGameLevel Handle(GameEvent gameEvent)
        {
            var centralObject = Game.Matrix[Game.StartPoint.x, Game.StartPoint.y + 1];
            var activeLetter = (centralObject as ActiveChar)?.Value;

            if (activeLetter == 'C')
            {
                if (gameEvent == GameEvent.Left)
                    return With(Game.Handle(gameEvent));
                else if (_steps % 3 == 0)
                    _displayMessage("Swipe left!"); // todo: localize
                return IncSteps();
            }

            if (activeLetter == 'A')
            {
                if (gameEvent == GameEvent.Down)
                    return With(Game.Handle(gameEvent));
                else if (_steps % 3 == 0)
                    _displayMessage("Swipe down!"); // todo: localize
                return IncSteps();
            }

            if (activeLetter == 'T')
            {
                if (gameEvent == GameEvent.Right)
                    return With(Game.Handle(gameEvent));
                else if (_steps % 3 == 0)
                    _displayMessage("Swipe right!"); // todo: localize
                return IncSteps();
            }

            if (gameEvent == GameEvent.Step)
                return With(updatedGame: Game.Handle(gameEvent), steps: _steps + 1);
            else
                return this;
        }

        /// <inheritdoc cref="IWordisGameLevel" />
        public override IWordisGameLevel WithOutput(Action<string> outFunc) =>
            With(outFunc: outFunc);

        /// <inheritdoc cref="IWordisGameLevel" />
        public override WordisTutorialLevel WithUpdatedGame(WordisGame updatedGame) =>
            With(updatedGame);

        /// <inheritdoc cref="IWordisGameLevel" />
        public override IWordisGameLevel Reset() =>
            new WordisTutorialLevel(_displayMessage);

        private WordisTutorialLevel With(
            WordisGame updatedGame = null,
            Action<string> outFunc = null,
            int? steps = 0) =>
            new WordisTutorialLevel(
                outFunc ?? _displayMessage,
                updatedGame ?? Game,
                steps ?? _steps);

        private WordisTutorialLevel IncSteps() => With(steps: _steps + 1);
    }
}
