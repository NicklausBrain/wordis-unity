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
                case GameEvent.Step:
                    {
                        // fall down if any spare space
                        for (int y = Y + 1; y < game.Settings.Height; y++)
                        {
                            var matrix = game.GameObjectsMatrix;
                            if (matrix[y, X] == null)
                            {
                                return With(y: Y + 1);
                            }
                        }

                        return this;
                    }
                default:
                    return this;
            }
        }

        private StaticChar With(
            int? x = null,
            int? y = null) =>
            new StaticChar(
                x ?? X,
                y ?? Y,
                Value);
    }
}
