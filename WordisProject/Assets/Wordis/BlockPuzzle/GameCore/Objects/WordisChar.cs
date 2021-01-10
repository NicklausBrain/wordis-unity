namespace Assets.Wordis.BlockPuzzle.GameCore.Objects
{
    /// <summary>
    /// Represents a single character.
    /// </summary>
    public class WordisChar : WordisObj
    {
        public WordisChar(
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

        public override WordisObj Handle(GameEvent gameEvent)
        {
            switch (gameEvent)
            {
                default:
                    return this;
            }
        }
    }
}
