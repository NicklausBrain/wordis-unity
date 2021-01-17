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
                        if (game.Settings.HasWater)
                        {
                            var staticCharsAbove = 0;
                            var sparePlaceAbove = 0;

                            for (int y = Y - 1; y >= 0; y--)
                            {
                                if (game.Matrix[X, y] is StaticChar)
                                {
                                    staticCharsAbove++;
                                }
                                else if (game.Matrix[X, y] == null)
                                {
                                    sparePlaceAbove++;
                                }
                            }

                            var destination = (x: X, y: game.Settings.AboveWaterY + staticCharsAbove);

                            if (destination.y > Y && destination.y < game.Settings.Height)
                            {
                                // go down by pressure
                                return With(y: Y + 1);
                            }

                            if (destination.y < Y && sparePlaceAbove > 0)
                            {
                                // emerge
                                return With(y: Y - 1);
                            }

                            return this;
                        }

                        var placeBelow = (x: X, y: Y + 1);
                        // fall down if any spare space below
                        for (int y = placeBelow.y; y < game.Settings.Height; y++)
                        {
                            if (game.Matrix[placeBelow.x, y] == null)
                            {
                                return With(y: placeBelow.y);
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
