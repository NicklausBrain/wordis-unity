using System;
using System.Text;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    /// <summary>
    /// Provides 2-dimensional access to game objects.
    /// </summary>
    public class WordisMatrix
    {
        private readonly WordisGame _game;
        private readonly Lazy<WordisObj[,]> _matrix;

        public WordisMatrix(WordisGame game)
        {
            _game = game;
            _matrix = new Lazy<WordisObj[,]>(InitMatrix);
        }

        /// <summary>
        /// Gets Game object by its coordinates.
        /// </summary>
        /// <param name="x">Zero-based column.</param>
        /// <param name="y">Zero-based row.</param>
        /// <returns><see cref="WordisObj "/></returns>
        public WordisObj this[int x, int y] =>
            AreInBounds(x, y)
                ? _matrix.Value[y, x]
                : null;

        /// <summary>
        /// Gets Game object by its point.
        /// </summary>
        /// <param name="p">Point value (x, y).</param>
        /// <returns><see cref="WordisObj "/></returns>
        public WordisObj this[(int x, int y) p] => this[p.y, p.x];

        /// <inheritdoc cref="WordisSettings.Width"/>
        public int Width => _game.Settings.Width;

        /// <inheritdoc cref="WordisSettings.Height"/>
        public int Height => _game.Settings.Height;

        /// <summary>
        /// String representation for troubleshooting purpose.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    sb.Append($"[{this[x, y]?.ToString() ?? "-"}]");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private WordisObj[,] InitMatrix()
        {
            var height = _game.Settings.Height;
            var width = _game.Settings.Width;

            // that's right we have X and Y different to default in C#
            var matrix = new WordisObj[height, width];

            foreach (WordisObj gameObject in _game.GameObjects)
            {
                // defensive programming to cover potential exception of unknown origin.
                if (AreInBounds(gameObject.X, gameObject.Y))
                {
                    matrix[gameObject.Y, gameObject.X] = gameObject;
                }
            }

            return matrix;
        }

        private bool AreInBounds(int x, int y)
        {
            var height = _game.Settings.Height;
            var width = _game.Settings.Width;

            return y < height &&
                   y >= 0 &&
                   x < width &&
                   x >= 0;
        }
    }
}
