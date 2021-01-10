namespace Assets.Wordis.BlockPuzzle.GameCore.Objects
{
    /// <summary>
    /// Represents a single character.
    /// </summary>
    public abstract class WordisChar : WordisObj
    {
        protected WordisChar(
            int x,
            int y,
            char value) : base(
            x: x,
            y: y)
        {
            Value = value;
        }

        #region Equality members

        public char Value { get; }

        protected bool Equals(WordisChar other) =>
            base.Equals(other) &&
            Value == other.Value;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WordisChar)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ Value.GetHashCode();
            }
        }

        #endregion
    }
}
