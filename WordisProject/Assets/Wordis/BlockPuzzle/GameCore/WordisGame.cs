using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Functions;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;
using Assets.Wordis.BlockPuzzle.GameCore.Words;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    public class WordisGame
    {
        //private readonly GetLetterFunc _getLetterFunc;
        private readonly LetterSource _letterSource;
        private readonly FindWordMatchesFunc _findWordMatchesFunc;
        private readonly ImmutableList<WordMatchEx> _wordMatches;
        private readonly ImmutableList<WordMatchEx> _lastMatches;
        private readonly ImmutableList<GameEvent> _gameEvents;

        /// <summary>
        ///  x:y
        /// [0:0][1:0][2:0]
        /// [0:1][1:1][2:1]
        /// [0:2][1:2][2:2]
        /// </summary>
        private readonly ImmutableList<WordisObj> _gameObjects;

        private WordisGame(
            WordisSettings settings,
            LetterSource letterSource,
            //GetLetterFunc getLetterFunc,
            FindWordMatchesFunc findWordMatchesFunc,
            ImmutableList<WordisObj> gameObjects,
            ImmutableList<WordMatchEx> wordMatches,
            ImmutableList<WordMatchEx> lastMatches,
            ImmutableList<GameEvent> gameEvents)
        {
            Settings = settings;
            Matrix = new WordisMatrix(this);
            _letterSource = letterSource;
            //_getLetterFunc = getLetterFunc;
            _findWordMatchesFunc = findWordMatchesFunc;
            _gameObjects = gameObjects;
            _wordMatches = wordMatches;
            _gameEvents = gameEvents;
            _lastMatches = lastMatches;
        }

        public WordisGame(
            WordisSettings settings,
            LetterSource letterSource = null,
            //GetLetterFunc getLetterFunc = null,
            FindWordMatchesFunc findWordMatchesFunc = null)
        {
            Settings = settings;
            Matrix = new WordisMatrix(this);
            _gameObjects = ImmutableList<WordisObj>.Empty;
            _wordMatches = ImmutableList<WordMatchEx>.Empty;
            _gameEvents = ImmutableList<GameEvent>.Empty;

            _letterSource =
                letterSource ??
                new RandomEngLetterSource();
            //_getLetterFunc =
            //    getLetterFunc ??
            //    new GetEngLetterFunc();
            _findWordMatchesFunc =
                findWordMatchesFunc ??
                new FindWordMatchesFunc();
        }

        public WordisSettings Settings { get; }

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
        public virtual WordisGame Handle(GameEvent gameEvent)
        {
            if (IsGameOver) // do nothing
            {
                return this;
            }

            var updatedGameObjects = GameObjects
                .Select(gameObject => gameObject.Handle(this, gameEvent))
                .ToImmutableList();

            var updatedEvents = _gameEvents.Add(gameEvent);

            switch (gameEvent)
            {
                case GameEvent.Step:
                    {
                        var updatedGame = With(
                            gameObjects: updatedGameObjects,
                            gameEvents: updatedEvents);

                        var matches = FindWordMatches(updatedGame.Matrix);

                        updatedGame = matches.Any()
                            ? updatedGame.With(
                                gameObjects: updatedGameObjects
                                    .Except(matches.SelectMany(m => m.MatchedChars))
                                    .ToImmutableList(),
                                wordMatches: updatedGame._wordMatches.AddRange(matches),
                                lastMatches: matches)
                            : updatedGame;

                        var hasActiveObjects = updatedGame.GameObjects.Any(o => o is ActiveChar);

                        return hasActiveObjects || updatedGame.IsGameOver
                            ? updatedGame
                            : updatedGame
                                .With(GenerateActiveChar(_letterSource.Char))
                                .With(letterSource: _letterSource.Next);
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
            LetterSource letterSource = null,
            ImmutableList<WordisObj> gameObjects = null,
            ImmutableList<WordMatchEx> wordMatches = null,
            ImmutableList<WordMatchEx> lastMatches = null,
            ImmutableList<GameEvent> gameEvents = null) =>
            new WordisGame(
                settings: Settings,
                //getLetterFunc: _getLetterFunc,
                letterSource: letterSource ?? _letterSource,
                findWordMatchesFunc: _findWordMatchesFunc,
                gameObjects: gameObjects ?? _gameObjects,
                gameEvents: gameEvents ?? _gameEvents,
                wordMatches: wordMatches ?? _wordMatches,
                lastMatches: lastMatches ?? _lastMatches);

        /// <summary>
        /// Spawns a new game object inside a game.
        /// </summary>
        /// <param name="gameObj">Game object.</param>
        /// <returns>An updated game instance.</returns>
        public WordisGame With(WordisObj gameObj) =>
            With(gameObjects: _gameObjects.Add(gameObj));

        /// <summary>
        /// The point where a new active object is generated.
        /// </summary>
        public (int x, int y) StartPoint => (x: Settings.Width / 2, y: 0);

        private ActiveChar GenerateActiveChar(char newChar)
        {
            //var randomChar = _letterSource.Char;

            return new ActiveChar(
                x: StartPoint.x,
                y: StartPoint.y,
                newChar);
        }

        private ImmutableList<WordMatchEx> FindWordMatches(
            WordisMatrix matrix)
        {
            var activeObj = GameObjects.FirstOrDefault(o => o is ActiveChar); // todo: what for?

            var foundMatches = activeObj == null
                ? ImmutableList<WordMatchEx>.Empty
                : _findWordMatchesFunc.Invoke(matrix, Settings.MinWordLength)
                .Select(m => new WordMatchEx(m, _gameEvents.Count, DateTimeOffset.UtcNow))
                .ToImmutableList();

            return foundMatches;
        }

        /// <summary>
        /// Represent collections of word matches made through the game.
        /// </summary>
        public class WordMatches
        {
            private readonly WordisGame _game;

            public WordMatches(WordisGame game)
            {
                _game = game;
            }

            /// <summary>
            /// All the matches made.
            /// </summary>
            public IReadOnlyList<WordMatchEx> All => _game._wordMatches;

            /// <summary>
            /// Matches on this game event.
            /// </summary>
            public IReadOnlyList<WordMatchEx> Last =>
                _game._lastMatches ?? ImmutableList<WordMatchEx>.Empty;

            /// <summary>
            /// Total matches.
            /// </summary>
            public int Count => _game._wordMatches.Count;
        }
    }
}
