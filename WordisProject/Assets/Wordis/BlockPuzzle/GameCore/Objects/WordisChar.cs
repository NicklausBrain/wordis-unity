namespace Assets.Wordis.BlockPuzzle.GameCore.Objects
{
    /// <summary>
    /// Represents a single character.
    /// </summary>
    public abstract class WordisChar : WordisObj
    {
        protected WordisChar(
            WordisGame wordisGame,
            int x,
            int y,
            char value) : base(
            wordisGame,
            x: x,
            y: y)
        {
            Value = value;
        }

        public char Value { get; }
    }
}
