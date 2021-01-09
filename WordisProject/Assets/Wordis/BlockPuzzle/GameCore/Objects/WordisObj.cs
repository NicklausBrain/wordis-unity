namespace Assets.Wordis.BlockPuzzle.GameCore.Objects
{
    /// <summary>
    /// Represents abstract Wordis game object
    /// </summary>
    public abstract class WordisObj
    {
        protected WordisObj(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// zero based column index.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// zero based row index.
        /// </summary>
        public int Y { get; }
    }
}
