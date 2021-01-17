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
        private readonly FindWordMatchesFunc _findWordMatchesFunc;
        private readonly Lazy<WordMatchEx[]> _allMatches;
        private readonly Lazy<WordMatchEx[]> _lastStepMatches;
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
            FindWordMatchesFunc findWordMatchesFunc = null,
            IEnumerable<WordisObj> gameObjects = null,
            IEnumerable<GameEvent> events = null,
            IEnumerable<WordMatchEx> allMatches = null,
            IEnumerable<WordMatchEx> lastStepMatches = null)
        {
            Settings = settings;
            _getLetterFunc =
                getLetterFunc ??
                new GetEngLetterFunc();
            _findWordMatchesFunc =
                findWordMatchesFunc ??
                new FindWordMatchesFunc(
                    new IsLegitEngWordFunc(),
                    settings.MinWordLength);
            _allMatches = new Lazy<WordMatchEx[]>(
                () => allMatches?.ToArray() ?? Array.Empty<WordMatchEx>());
            _lastStepMatches = new Lazy<WordMatchEx[]>(
                () => lastStepMatches?.ToArray() ?? Array.Empty<WordMatchEx>());
            _gameObjects = new Lazy<WordisObj[]>(
                () => gameObjects?.ToArray() ?? Array.Empty<WordisObj>());
            _events = new Lazy<GameEvent[]>(
                () => events?.ToArray() ?? Array.Empty<GameEvent>());
            Matrix = new WordisMatrix(this);
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

        /// <inheritdoc cref="WordisMatrix"/>
        public WordisMatrix Matrix { get; }

        /// <summary>
        /// Log of the game events.
        /// </summary>
        public IReadOnlyList<GameEvent> GameEvents => _events.Value;

        /// <summary>
        /// All the words matched during the game.
        /// </summary>
        public IReadOnlyList<WordMatchEx> AllMatches => _allMatches.Value;

        /// <summary>
        /// Matches made on last step.
        /// </summary>
        public IReadOnlyList<WordMatchEx> LastStepMatches => _lastStepMatches.Value;

        /// <summary>
        /// Determines if game is over.
        /// </summary>
        public bool IsGameOver
        {
            get
            {
                for (int y = 0; y < Settings.Height; y++)
                {
                    if (Matrix[StartPoint.x, y] == null || Matrix[StartPoint.x, y] is ActiveChar)
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
                .ToArray();

            var updatedEvents = GameEvents.Append(gameEvent);

            switch (gameEvent)
            {
                case GameEvent.Step:
                    {
                        var matches = FindWordMatches(updatedState: updatedGameObjects);

                        var updatedGame = With(
                            gameObjects: matches.Any()
                                ? updatedGameObjects.Except(matches.SelectMany(m => m.MatchedChars))
                                : updatedGameObjects,
                            gameEvents: updatedEvents,
                            allMatches: AllMatches.Concat(matches),
                            lastStepMatches: matches);

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
        public WordisGame With(
            IEnumerable<WordisObj> gameObjects,
            IEnumerable<GameEvent> gameEvents = null,
            IEnumerable<WordMatchEx> allMatches = null,
            IEnumerable<WordMatchEx> lastStepMatches = null) =>
            new WordisGame(
                settings: Settings,
                getLetterFunc: _getLetterFunc,
                findWordMatchesFunc: _findWordMatchesFunc,
                gameObjects: gameObjects,
                events: gameEvents ?? GameEvents,
                allMatches: allMatches ?? AllMatches,
                lastStepMatches: lastStepMatches ?? LastStepMatches);

        /// <summary>
        /// Spawns a new game object inside a game.
        /// </summary>
        /// <param name="gameObj">Game object.</param>
        /// <returns>An updated game instance.</returns>
        public WordisGame With(WordisObj gameObj) =>
            With(GameObjects.Append(gameObj));

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

        private WordMatchEx[] FindWordMatches(WordisObj[] updatedState)
        {
            var activeObj = GameObjects.FirstOrDefault(o => o is ActiveChar);

            var foundMatches = activeObj == null
                ? Array.Empty<WordMatchEx>()
                : _findWordMatchesFunc.Invoke(
                    updatedState // this is optimization to calculate the match only for the active axes
                        .Where(o => o is StaticChar && (o.X == activeObj.X || o.Y == activeObj.Y))
                        .Cast<StaticChar>())
                .Select(m => new WordMatchEx(m, Step, DateTimeOffset.UtcNow))
                .ToArray();

            return foundMatches;
        }
    }
}
