using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Functions;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;
using Assets.Wordis.BlockPuzzle.GameCore.Words;
using Newtonsoft.Json;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    /// <summary>
    /// Encapsulates essential game logic.
    /// </summary>
    public class WordisGame
    {
        // todo: find a way to serialize this dependency
        // [JsonProperty("letterSource")]
        private readonly LetterSource _letterSource;
        // todo: find a way to serialize this dependency
        // [JsonProperty("findWordMatchesFunc")]
        private readonly FindWordMatchesFunc _findWordMatchesFunc;
        [JsonProperty("availableMatches")]
        private readonly ImmutableList<WordMatchEx> _availableMatches;
        [JsonProperty("wordMatches")]
        private readonly ImmutableList<WordMatchEx> _wordMatches;
        [JsonProperty("lastMatches")]
        private readonly ImmutableList<WordMatchEx> _lastMatches;
        [JsonProperty("gameEvents")]
        private readonly ImmutableList<GameEvent> _gameEvents;

        private readonly Lazy<ActiveChar> _activeChar;

        /// <summary>
        ///  x:y
        /// [0:0][1:0][2:0]
        /// [0:1][1:1][2:1]
        /// [0:2][1:2][2:2]
        /// </summary>
        private readonly ImmutableList<WordisObj> _gameObjects;

        [JsonConstructor]
        private WordisGame(
            WordisSettings settings,
            LetterSource letterSource,
            FindWordMatchesFunc findWordMatchesFunc,
            ImmutableList<WordisObj> gameObjects,
            ImmutableList<WordMatchEx> wordMatches,
            ImmutableList<WordMatchEx> lastMatches,
            ImmutableList<WordMatchEx> availableMatches,
            ImmutableList<GameEvent> gameEvents)
        {
            Settings = settings;
            Matrix = new WordisMatrix(this);
            _letterSource =
                letterSource ??
                new RandomEngLetterSource();
            _findWordMatchesFunc =
                findWordMatchesFunc ??
                new FindWordMatchesFunc();
            _gameObjects = gameObjects ?? ImmutableList<WordisObj>.Empty;
            _wordMatches = wordMatches ?? ImmutableList<WordMatchEx>.Empty;
            _lastMatches = lastMatches ?? ImmutableList<WordMatchEx>.Empty;
            _availableMatches = availableMatches ?? ImmutableList<WordMatchEx>.Empty;
            _gameEvents = gameEvents ?? ImmutableList<GameEvent>.Empty;
            _activeChar = new Lazy<ActiveChar>(() =>
                GameObjects.FirstOrDefault(o => o is ActiveChar) as ActiveChar);
        }

        public WordisGame(
            WordisSettings settings,
            LetterSource letterSource = null,
            FindWordMatchesFunc findWordMatchesFunc = null) : this(
                settings: settings,
                letterSource: letterSource,
                findWordMatchesFunc: findWordMatchesFunc,
                gameObjects: null,
                wordMatches: null,
                lastMatches: null,
                availableMatches: null,
                gameEvents: null)
        {
        }

        public WordisSettings Settings { get; }

        /// <summary>
        /// State of the game.
        /// </summary>
        public IReadOnlyList<WordisObj> GameObjects => _gameObjects;

        /// <summary>
        /// Current active char. Can be NULL.
        /// </summary>
        [JsonIgnore]
        public ActiveChar ActiveChar => _activeChar.Value;

        /// <inheritdoc cref="WordisMatrix"/>
        [JsonIgnore]
        public WordisMatrix Matrix { get; }

        /// <summary>
        /// Log of the game events.
        /// </summary>
        public IReadOnlyList<GameEvent> GameEvents => _gameEvents;

        /// <summary>
        /// Returns the last event occured.
        /// </summary>
        [JsonIgnore]
        public GameEvent LastEvent =>
            GameEvents.Any()
                ? GameEvents[GameEvents.Count - 1]
                : GameEvent.None;

        /// <summary>
        /// All the words matched during the game.
        /// </summary>
        [JsonIgnore]
        public WordMatches Matches => new WordMatches(this);

        /// <summary>
        /// Accumulated score.
        /// </summary>
        [JsonIgnore]
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

            var updatedEvents = _gameEvents.Add(gameEvent);

            // handle static first for collision handling
            var staticObjects = _gameObjects.Remove(ActiveChar);
            var staticHandled = staticObjects
                .Select(o => o.Handle(this, gameEvent))
                .ToImmutableList();
            var updatedGame = With(
                gameObjects: staticHandled,
                gameEvents: updatedEvents,
                lastMatches: ImmutableList<WordMatchEx>.Empty);

            // handle active char if any
            if (ActiveChar != null)
            {
                updatedGame = With(
                    gameObjects: staticHandled.Add(ActiveChar.Handle(updatedGame, gameEvent)),
                    gameEvents: updatedEvents,
                    lastMatches: ImmutableList<WordMatchEx>.Empty);
            }

            switch (gameEvent)
            {
                case GameEvent.Step:
                    {
                        var availableMatches = FindWordMatches(updatedGame.Matrix);

                        updatedGame = availableMatches.Any()
                            ? updatedGame.With(
                                availableMatches: availableMatches)
                            : updatedGame;

                        // todo: consider generating active object 1 step later
                        // todo: https://github.com/NicklausBrain/wordis-unity/issues/38
                        return updatedGame.CanHaveActiveChar
                            ? updatedGame.WithNewActiveChar()
                            : updatedGame;
                    }
                case GameEvent.Match:
                    {
                        var availableMatches = updatedGame._availableMatches;

                        updatedGame = availableMatches.Any()
                            ? updatedGame.With(
                                gameObjects: updatedGame._gameObjects
                                    .Except(availableMatches.SelectMany(m => m.MatchedChars))
                                    .ToImmutableList(),
                                wordMatches: updatedGame._wordMatches.AddRange(availableMatches),
                                lastMatches: availableMatches,
                                availableMatches: ImmutableList<WordMatchEx>.Empty)
                            : updatedGame;

                        // todo: consider generating active object 1 step later
                        // todo: https://github.com/NicklausBrain/wordis-unity/issues/38
                        return updatedGame.CanHaveActiveChar
                            ? updatedGame.WithNewActiveChar()
                            : updatedGame;
                    }
                default:
                    return updatedGame;
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
            ImmutableList<WordMatchEx> availableMatches = null,
            ImmutableList<GameEvent> gameEvents = null) =>
            new WordisGame(
                settings: Settings,
                letterSource: letterSource ?? _letterSource,
                findWordMatchesFunc: _findWordMatchesFunc,
                gameObjects: gameObjects ?? _gameObjects,
                gameEvents: gameEvents ?? _gameEvents,
                wordMatches: wordMatches ?? _wordMatches,
                lastMatches: lastMatches ?? _lastMatches,
                availableMatches: availableMatches ?? _availableMatches);

        /// <summary>
        /// Spawns a new game object inside a game.
        /// </summary>
        /// <param name="gameObj">Game object.</param>
        /// <returns>An updated game instance.</returns>
        public WordisGame With(WordisObj gameObj) =>
            With(gameObjects: _gameObjects.Add(gameObj));

        /// <summary>
        /// For Unit Tests. Creates an instance with predefined word matches.
        /// </summary>
        /// <param name="wordMatches"></param>
        /// <returns></returns>
        public WordisGame WithWordMatches(params WordMatchEx[] wordMatches) =>
            With(wordMatches: wordMatches.ToImmutableList());

        /// <summary>
        /// The point where a new active object is generated.
        /// </summary>
        public (int x, int y) StartPoint => (x: Settings.Width / 2, y: 0);

        /// <summary>
        /// Letter that will come next.
        /// </summary>
        public char LetterToCome => _letterSource.Char;

        private ActiveChar GenerateActiveChar(char newChar)
        {
            return new ActiveChar(
                x: StartPoint.x,
                y: StartPoint.y,
                newChar);
        }

        private ImmutableList<WordMatchEx> FindWordMatches(
            WordisMatrix matrix)
        {
            var foundMatches = _findWordMatchesFunc.Invoke(
                    matrix,
                    Settings.MinWordLength)
                .Select(m => new WordMatchEx(m, _gameEvents.Count, DateTimeOffset.UtcNow))
                .ToImmutableList();

            return foundMatches;
        }

        private bool CanHaveActiveChar =>
            !IsGameOver && ActiveChar == null && Matrix[StartPoint.x, StartPoint.y] == null;

        private WordisGame WithNewActiveChar() =>
            With(GenerateActiveChar(_letterSource.Char))
                .With(letterSource: _letterSource.Next);

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
            /// Matches available to make at this step.
            /// </summary>
            public IReadOnlyList<WordMatchEx> Available => _game._availableMatches;

            /// <summary>
            /// Total matches.
            /// </summary>
            public int Count => _game._wordMatches.Count;
        }

        /// <summary>
        /// Converts game to JSON string.
        /// </summary>
        public static string ToJson(WordisGame game)
        {
            return JsonConvert.SerializeObject(game, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        /// <summary>
        /// Restores game state from JSON.
        /// </summary>
        public static WordisGame FromJson(string jsonStr)
        {
            return JsonConvert.DeserializeObject<WordisGame>(jsonStr, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}
