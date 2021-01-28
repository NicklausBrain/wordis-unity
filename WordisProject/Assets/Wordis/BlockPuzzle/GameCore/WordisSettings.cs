namespace Assets.Wordis.BlockPuzzle.GameCore
{
    public class WordisSettings
    {
        public WordisSettings(
            float speed = 1.0f,
            int width = 9,
            int height = 9,
            int minWordLength = 3,
            int waterLevel = 0)
        {
            Speed = speed;
            WaterLevel = waterLevel;
            Width = width;
            Height = height;
            MinWordLength = minWordLength;
        }

        /// <summary>
        /// Game step duration in seconds.
        /// Lover -> faster game.
        /// </summary>
        public float Speed { get; }

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
        /// The line before water (Y coordinate).
        /// </summary>
        public int AboveWaterY => MaxY - WaterLevel;

        /// <summary>
        /// The water like Y coordinate.
        /// </summary>
        public int WaterY => Height - WaterLevel;

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
            float? speed = null,
            int? width = null,
            int? height = null,
            int? minWordLength = null,
            int? waterLevel = null) =>
            new WordisSettings(
                speed: speed ?? Speed,
                width: width ?? Width,
                height: height ?? Height,
                minWordLength: minWordLength ?? MinWordLength,
                waterLevel: waterLevel ?? WaterLevel);
    }
}
