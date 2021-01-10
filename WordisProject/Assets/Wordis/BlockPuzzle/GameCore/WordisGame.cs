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
            IEnumerable<WordMatch> matches = null,
            int step = 0)
        {
            Settings = settings;
            Step = step;
            _matches = new Lazy<WordMatch[]>(
                () => matches?.ToArray() ?? Array.Empty<WordMatch>());
            _gameObjects = new Lazy<WordisObj[]>(
                () => gameObjects?.ToArray() ?? Array.Empty<WordisObj>());
            _events = new Lazy<GameEvent[]>(
                () => inputs?.ToArray() ?? Array.Empty<GameEvent>());
        }

        public WordisSettings Settings { get; }

        public int Step { get; }

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
            switch (gameEvent)
            {
                case GameEvent.Step:
                    return new WordisGame(
                        Settings,
                        GameObjects.Select(gameObject => gameObject.Handle(gameEvent)),
                        GameEvents.Append(gameEvent),
                        Matches,
                        Step + 1);
                case GameEvent.Down:
                    return this;
                case GameEvent.Left:
                    return this;
                case GameEvent.Right:
                    return this;
                default:
                    return this;
            }
        }
    }
}
