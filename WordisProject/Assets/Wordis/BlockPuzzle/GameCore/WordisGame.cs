using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    public class WordisGame
    {
        private readonly Lazy<WordMatch[]> _matches;
        private readonly Lazy<PlayerInput[]> _inputs;
        private readonly Lazy<WordisObj[]> _gameObjects;

        public WordisGame(
            WordisSettings settings,
            IEnumerable<WordisObj> gameObjects = null,
            IEnumerable<PlayerInput> inputs = null,
            IEnumerable<WordMatch> matches = null,
            int step = 0)
        {
            Settings = settings;
            Step = step;
            _matches = new Lazy<WordMatch[]>(
                () => matches?.ToArray() ?? Array.Empty<WordMatch>());
            _gameObjects = new Lazy<WordisObj[]>(
                () => gameObjects?.ToArray() ?? Array.Empty<WordisObj>());
            _inputs = new Lazy<PlayerInput[]>(
                () => inputs?.ToArray() ?? Array.Empty<PlayerInput>());
        }

        public WordisSettings Settings { get; }

        public int Step { get; }

        /// <summary>
        /// State of the game.
        /// </summary>
        public IReadOnlyList<WordisObj> GameObjects => _gameObjects.Value;

        /// <summary>
        /// Log of the player inputs.
        /// </summary>
        public IReadOnlyList<PlayerInput> PlayerInputs => _inputs.Value;

        /// <summary>
        /// Words matched.
        /// </summary>
        public IReadOnlyList<WordMatch> Matches => _matches.Value;

        /// <summary>
        /// Transforms the game into the next state.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public WordisGame Next(PlayerInput input)
        {
            switch (input)
            {
                case PlayerInput.None:
                    break;
                case PlayerInput.Down:
                    break;
                case PlayerInput.Left:
                    break;
                case PlayerInput.Right:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), input, null);
            }

            return new WordisGame(
                Settings,
                GameObjects,
                PlayerInputs.Append(input),
                Matches,
                Step + 1);
        }
    }
}
