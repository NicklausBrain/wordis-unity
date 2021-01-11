namespace Assets.Wordis.BlockPuzzle.GameCore.Objects
{
    /// <summary>
    /// Represents abstract Wordis game object
    /// </summary>
    public abstract class WordisObj
    {
        protected WordisObj(
            int x,
            int y)
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

        /// <summary>
        /// Handles game event, transforming the object if necessary.
        /// </summary>
        /// <param name="wordisGame">Reference to a game this game object belongs to.</param>
        /// <param name="gameEvent">Event to be handled.</param>
        /// <returns></returns>
        public abstract WordisObj Handle(
            WordisGame wordisGame,
            GameEvent gameEvent);

        #region Equality members

        protected bool Equals(WordisObj other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WordisObj)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        #endregion
    }
}
