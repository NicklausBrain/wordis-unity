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
        public WordisObj this[int x, int y] => _matrix.Value[y, x];

        /// <summary>
        /// Gets Game object by its point.
        /// </summary>
        /// <param name="p.x">Zero-based column.</param>
        /// <param name="p.y">Zero-based row.</param>
        /// <returns><see cref="WordisObj "/></returns>
        public WordisObj this[(int x, int y) p] => _matrix.Value[p.y, p.x];

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
            // that's right we have X and Y different to default in C#
            var matrix = new WordisObj[_game.Settings.Height, _game.Settings.Width];

            foreach (WordisObj gameObject in _game.GameObjects)
            {
                matrix[gameObject.Y, gameObject.X] = gameObject;
            }

            return matrix;
        }
    }
}
