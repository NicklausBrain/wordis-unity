namespace Assets.Wordis.BlockPuzzle.GameCore.Objects
{
    /// <summary>
    /// Represents a character that is found its place and not controlled by a player.
    /// </summary>
    public class StaticChar : WordisChar
    {
        public StaticChar(
            int x,
            int y,
            char value) : base(
            x: x,
            y: y,
            value: value)
        {
        }

        public override WordisObj Handle(WordisGame game, GameEvent gameEvent)
        {
            switch (gameEvent)
            {
                default:
                    return this;
            }
        }
    }
}
