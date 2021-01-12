namespace Assets.Wordis.BlockPuzzle.GameCore
{
    public class WordisSettings
    {
        public WordisSettings(
            int width,
            int height,
            int minWordMatch = 3)
        {
            Width = width;
            Height = height;
            MinWordMatch = minWordMatch;
        }

        /// <summary>
        /// Game board columns count.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Max possible X-axis index;
        /// </summary>
        public int MaxX => Width - 1;

        /// <summary>
        /// Game board rows count.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Max possible Y-axis index;
        /// </summary>
        public int MaxY => Height - 1;

        /// <summary>
        /// Minimum word to be counted as match.
        /// </summary>
        public int MinWordMatch { get; }

        /// <summary>
        /// Creates a new instance with the given properties.
        /// </summary>
        public WordisSettings With(
            int? width,
            int? height) =>
            new WordisSettings(
                width ?? Width,
                height ?? Height);
    }
}
