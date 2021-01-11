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
        /// Game board rows count.
        /// </summary>
        public int Height { get; }

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
