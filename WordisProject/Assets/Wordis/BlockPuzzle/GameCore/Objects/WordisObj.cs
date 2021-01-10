namespace Assets.Wordis.BlockPuzzle.GameCore.Objects
{
    /// <summary>
    /// Represents abstract Wordis game object
    /// </summary>
    public abstract class WordisObj
    {
        protected WordisObj(
            WordisGame wordisGame,
            int x,
            int y)
        {
            WordisGame = wordisGame;
            X = x;
            Y = y;
        }

        /// <summary>
        /// Reference to a game this game object belongs to.
        /// </summary>
        protected WordisGame WordisGame { get; }

        /// <summary>
        /// zero based column index.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// zero based row index.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Handles game event, transforming the object if necessary.
        /// </summary>
        public abstract WordisObj Handle(GameEvent gameEvent);
    }
}
