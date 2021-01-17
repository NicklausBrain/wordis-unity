namespace Assets.Wordis.BlockPuzzle.GameCore
{
    public class WordisSettings
    {
        public WordisSettings(
            int width = 9,
            int height = 9,
            int minWordMatch = 3,
            int waterLevel = 0)
        {
            WaterLevel = waterLevel;
            Width = width;
            Height = height;
            MinWordLength = minWordMatch;
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
        /// Minimum word length to be counted as match.
        /// </summary>
        public int MinWordLength { get; }

        /// <summary>
        /// "Water" is an opposite force to the down direction of blocks.
        /// Block stops on water, and drowns if another block pushes it down.
        /// Water serves as a game-play enhancement.
        /// Water level counts from the bottom of the game board.
        /// </summary>
        public int WaterLevel { get; }

        /// <inheritdoc cref="WaterLevel"/>
        public bool HasWater => WaterLevel > 0;

        /// <summary>
        /// Determines if zer-based Y coordinate belong to the water field/zone.
        /// </summary>
        public bool IsWaterZone(int y)
        {
            if (HasWater)
            {
                return y >= Height - WaterLevel;
            }

            return false;
        }

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
