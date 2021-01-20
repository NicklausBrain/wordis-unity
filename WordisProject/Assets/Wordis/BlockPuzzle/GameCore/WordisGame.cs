using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Functions;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    public class WordisGame
    {
        private readonly GetLetterFunc _getLetterFunc;
        private readonly FindWordMatchesFunc _findWordMatchesFunc;
        private readonly IImmutableDictionary<int, IReadOnlyList<WordMatchEx>> _wordMatches;
        private readonly IImmutableList<GameEvent> _gameEvents;

        /// <summary>
        ///  x:y
        /// [0:0][1:0][2:0]
        /// [0:1][1:1][2:1]
        /// [0:2][1:2][2:2]
        /// </summary>
        private readonly IImmutableList<WordisObj> _gameObjects;

        private WordisGame(
            WordisSettings settings,
            GetLetterFunc getLetterFunc,
            FindWordMatchesFunc findWordMatchesFunc,
            IImmutableList<WordisObj> gameObjects,
            IImmutableDictionary<int, IReadOnlyList<WordMatchEx>> wordMatches,
            IImmutableList<GameEvent> gameEvents)
        {
            Settings = settings;
            Matrix = new WordisMatrix(this);
            _getLetterFunc = getLetterFunc;
            _findWordMatchesFunc = findWordMatchesFunc;
            _gameObjects = gameObjects;
            _wordMatches = wordMatches;
            _gameEvents = gameEvents;
        }

        public WordisGame(
            WordisSettings settings,
            GetLetterFunc getLetterFunc = null,
            FindWordMatchesFunc findWordMatchesFunc = null)
        {
            Settings = settings;
            Matrix = new WordisMatrix(this);
            _gameObjects = ImmutableArray<WordisObj>.Empty;
            _wordMatches = ImmutableDictionary<int, IReadOnlyList<WordMatchEx>>.Empty;
            _gameEvents = ImmutableArray<GameEvent>.Empty;

            _getLetterFunc =
                getLetterFunc ??
                new GetEngLetterFunc();
            _findWordMatchesFunc =
                findWordMatchesFunc ??
                new FindWordMatchesFunc(new IsLegitEngWordFunc(), settings.MinWordLength);
        }

        public WordisSettings Settings { get; }

        ///// <summary>
        ///// The current step of the game
        ///// </summary>
        //public int Step => GameEvents.Count(e => e == GameEvent.Step);

        /// <summary>
        /// State of the game.
        /// </summary>
        public IReadOnlyList<WordisObj> GameObjects => _gameObjects;

        /// <inheritdoc cref="WordisMatrix"/>
        public WordisMatrix Matrix { get; }

        /// <summary>
        /// Log of the game events.
        /// </summary>
        public IReadOnlyList<GameEvent> GameEvents => _gameEvents;

        /// <summary>
        /// All the words matched during the game.
        /// </summary>
        public WordMatches Matches => new WordMatches(this);

        /// <summary>
        /// Accumulated score.
        /// </summary>
        public WordisScore Score => new WordisScore(this);

        /// <summary>
        /// Determines if game is over.
        /// </summary>
        public bool IsGameOver
        {
            get
            {
                for (int y = 0; y < Settings.Height; y++)
                {
                    if (Matrix[StartPoint.x, y] == null ||
                        Matrix[StartPoint.x, y] is ActiveChar)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Transforms the game into the next state.
        /// </summary>
        /// <param name="gameEvent"></param>
        /// <returns></returns>
        public WordisGame Handle(GameEvent gameEvent)
        {
            if (IsGameOver) // do nothing
            {
                return this;
            }

            var updatedGameObjects = GameObjects
                .Select(gameObject => gameObject.Handle(this, gameEvent))
                .ToImmutableArray();

            var updatedEvents = _gameEvents.Add(gameEvent);

            switch (gameEvent)
            {
                case GameEvent.Step:
                    {
                        var matches = FindWordMatches(updatedState: updatedGameObjects);

                        var updatedGame = With(
                            gameObjects: matches.Any()
                                ? updatedGameObjects
                                    .Except(matches.SelectMany(m => m.MatchedChars))
                                    .ToImmutableArray()
                                : updatedGameObjects,
                            gameEvents: updatedEvents,
                            matches: _wordMatches.Add(_gameEvents.Count, matches));

                        var hasActiveObjects = updatedGame.GameObjects.Any(o => o is ActiveChar);

                        return hasActiveObjects || updatedGame.IsGameOver
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
        private WordisGame With(
            IImmutableList<WordisObj> gameObjects = null,
            IImmutableDictionary<int, IReadOnlyList<WordMatchEx>> matches = null,
            IImmutableList<GameEvent> gameEvents = null) =>
            new WordisGame(
                settings: Settings,
                getLetterFunc: _getLetterFunc,
                findWordMatchesFunc: _findWordMatchesFunc,
                gameObjects: gameObjects ?? _gameObjects,
                gameEvents: gameEvents ?? _gameEvents,
                wordMatches: matches ?? _wordMatches);

        /// <summary>
        /// Spawns a new game object inside a game.
        /// </summary>
        /// <param name="gameObj">Game object.</param>
        /// <returns>An updated game instance.</returns>
        public WordisGame With(WordisObj gameObj) =>
            With(_gameObjects.Add(gameObj));

        /// <summary>
        /// The point where a new active object is generated.
        /// </summary>
        public (int x, int y) StartPoint => (x: Settings.Width / 2, y: 0);

        private ActiveChar GenerateActiveChar()
        {
            var randomChar = _getLetterFunc.Invoke();

            return new ActiveChar(
                x: StartPoint.x,
                y: StartPoint.y,
                randomChar);
        }

        private WordMatchEx[] FindWordMatches(IReadOnlyList<WordisObj> updatedState)
        {
            var activeObj = GameObjects.FirstOrDefault(o => o is ActiveChar);

            var foundMatches = activeObj == null
                ? Array.Empty<WordMatchEx>()
                : _findWordMatchesFunc.Invoke(
                    updatedState
                        .Where(o => o is StaticChar)
                        .Cast<StaticChar>())
                .Select(m => new WordMatchEx(m, _gameEvents.Count, DateTimeOffset.UtcNow))
                .ToArray();

            return foundMatches;
        }

        /// <summary>
        /// Represent collections of word matches made through the game.
        /// </summary>
        public class WordMatches : IReadOnlyCollection<WordMatchEx>
        {
            private readonly WordisGame _game;

            public WordMatches(WordisGame game)
            {
                _game = game;
            }

            /// <summary>
            /// Matches on this game event.
            /// </summary>
            public IReadOnlyList<WordMatchEx> Last =>
                _game._wordMatches.ContainsKey(_game._gameEvents.Count - 1)
                    ? _game._wordMatches[_game._gameEvents.Count - 1]
                    : Array.Empty<WordMatchEx>();

            /// <summary>
            /// All matches.
            /// </summary>
            public IReadOnlyDictionary<int, IReadOnlyList<WordMatchEx>> All =>
                _game._wordMatches;

            public IEnumerator<WordMatchEx> GetEnumerator() =>
                _game._wordMatches.Values
                    .SelectMany(m => m)
                    .GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() =>
                GetEnumerator();

            /// <summary>
            /// Total matches.
            /// </summary>
            public int Count => _game._wordMatches.Sum(m => m.Value.Count);
        }
    }
}
