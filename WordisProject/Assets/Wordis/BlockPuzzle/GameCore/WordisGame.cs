using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Functions;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    public class WordisGame
    {
        private readonly GetLetterFunc _getLetterFunc;
        private readonly Lazy<WordMatch[]> _matches;
        private readonly Lazy<GameEvent[]> _events;

        /// <summary>
        ///  x:y
        /// [0:0][1:0][2:0]
        /// [0:1][1:1][2:1]
        /// [0:2][1:2][2:2]
        /// </summary>
        private readonly Lazy<WordisObj[]> _gameObjects;

        public WordisGame(
            WordisSettings settings,
            GetLetterFunc getLetterFunc = null,
            IEnumerable<WordisObj> gameObjects = null,
            IEnumerable<GameEvent> events = null,
            IEnumerable<WordMatch> matches = null)
        {
            Settings = settings;
            _getLetterFunc = getLetterFunc ?? new GetEngLetterFunc();
            _matches = new Lazy<WordMatch[]>(
                () => matches?.ToArray() ?? Array.Empty<WordMatch>());
            _gameObjects = new Lazy<WordisObj[]>(
                () => gameObjects?.ToArray() ?? Array.Empty<WordisObj>());
            _events = new Lazy<GameEvent[]>(
                () => events?.ToArray() ?? Array.Empty<GameEvent>());
        }

        public WordisSettings Settings { get; }

        /// <summary>
        /// The current step of the game
        /// </summary>
        public int Step => GameEvents.Count(e => e == GameEvent.Step);

        /// <summary>
        /// State of the game.
        /// </summary>
        public IReadOnlyList<WordisObj> GameObjects => _gameObjects.Value;

        /// <summary>
        /// Log of the game events.
        /// </summary>
        public IReadOnlyList<GameEvent> GameEvents => _events.Value;

        /// <summary>
        /// Words matched.
        /// </summary>
        public IReadOnlyList<WordMatch> Matches => _matches.Value;

        /// <summary>
        /// Transforms the game into the next state.
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <returns></returns>
        public WordisGame Handle(GameEvent gameEvent)
        {
            var updatedGameObjects = GameObjects
                .Select(gameObject => gameObject.Handle(this, gameEvent))
                .ToArray();

            var updatedEvents = GameEvents.Append(gameEvent);

            switch (gameEvent)
            {
                case GameEvent.Step:
                    {
                        // todo: determine matches

                        var hasActiveObjects = updatedGameObjects.Any(o => o is ActiveChar);
                        var updatedGame = With(
                            gameObjects: updatedGameObjects,
                            gameEvents: updatedEvents,
                            matches: Matches);

                        // todo: make testable and test
                        return hasActiveObjects
                            ? updatedGame
                            : updatedGame.With(GenerateActiveChar());
                    }
                default:
                    return With(
                        gameObjects: updatedGameObjects,
                        gameEvents: updatedEvents);
            }
        }

        /// <summary>
        /// Updates a game with the refreshed properties.
        /// </summary>
        /// <returns>An updated game instance.</returns>
        public WordisGame With(
            IEnumerable<WordisObj> gameObjects,
            IEnumerable<GameEvent> gameEvents = null,
            IEnumerable<WordMatch> matches = null) =>
            new WordisGame(
                Settings,
                _getLetterFunc,
                gameObjects,
                gameEvents ?? GameEvents,
                matches ?? Matches);

        /// <summary>
        /// Spawns a new game object inside a game.
        /// </summary>
        /// <param name="gameObj">Game object.</param>
        /// <returns>An updated game instance.</returns>
        public WordisGame With(WordisObj gameObj) =>
            With(GameObjects.Append(gameObj));

        private ActiveChar GenerateActiveChar()
        {

            var randomChar = _getLetterFunc.Invoke();

            return new ActiveChar(Settings.Width / 2, 0, randomChar);
        }
    }
}
