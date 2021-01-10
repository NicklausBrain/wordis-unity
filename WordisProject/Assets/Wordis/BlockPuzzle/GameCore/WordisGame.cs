using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    public class WordisGame
    {
        private readonly Lazy<WordMatch[]> _matches;
        private readonly Lazy<GameEvent[]> _events;
        private readonly Lazy<WordisObj[]> _gameObjects;

        public WordisGame(
            WordisSettings settings,
            IEnumerable<WordisObj> gameObjects = null,
            IEnumerable<GameEvent> inputs = null,
            IEnumerable<WordMatch> matches = null)
        {
            Settings = settings;
            _matches = new Lazy<WordMatch[]>(
                () => matches?.ToArray() ?? Array.Empty<WordMatch>());
            _gameObjects = new Lazy<WordisObj[]>(
                () => gameObjects?.ToArray() ?? Array.Empty<WordisObj>());
            _events = new Lazy<GameEvent[]>(
                () => inputs?.ToArray() ?? Array.Empty<GameEvent>());
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
            var updatedGameObjects = GameObjects.Select(gameObject => gameObject.Handle(gameEvent));
            var updatedEvents = GameEvents.Append(gameEvent);

            switch (gameEvent)
            {
                case GameEvent.Step:
                    // todo: determine matches
                    return With(
                        gameObjects: updatedGameObjects,
                        gameEvents: updatedEvents,
                        matches: Matches);
                default:
                    return With(
                        gameObjects: updatedGameObjects,
                        gameEvents: updatedEvents);
            }
        }

        private WordisGame With(
            IEnumerable<WordisObj> gameObjects,
            IEnumerable<GameEvent> gameEvents,
            IEnumerable<WordMatch> matches = null) =>
            new WordisGame(
                Settings,
                gameObjects,
                gameEvents,
                matches ?? Matches);
    }
}
