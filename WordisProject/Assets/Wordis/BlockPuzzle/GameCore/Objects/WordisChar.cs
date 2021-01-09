namespace Assets.Wordis.BlockPuzzle.GameCore.Objects
{
    /// <summary>
    /// Represents a single character.
    /// </summary>
    public class WordisChar : WordisObj
    {
        public WordisChar(
            char value,
            int x,
            int y) : base(
            x: x,
            y: y)
        {
            Value = value;
        }

        public char Value { get; }
    }
}
