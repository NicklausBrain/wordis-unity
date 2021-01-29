using System.Linq;

namespace Assets.Wordis.BlockPuzzle.GameCore.Objects
{
    /// <summary>
    /// Character controlled by a player.
    /// </summary>
    public class ActiveChar : WordisChar
    {
        public ActiveChar(
            int x,
            int y,
            char value) : base(
            x,
            y,
            value)
        {
        }

        public override WordisObj Handle(
            WordisGame game,
            GameEvent gameEvent)
        {
            switch (gameEvent)
            {
                case GameEvent.Step:
                    return HandleStep(game);
                case GameEvent.Down:
                    return HandleDown(game);
                case GameEvent.Left:
                    return HandleLeft(game);
                case GameEvent.Right:
                    return HandleRight(game);
                default:
                    return this;
            }
        }

        /// <summary>
        /// Moves the char down. Converts it to static on obstacle.
        /// </summary>
        private WordisObj HandleStep(WordisGame game)
        {
            // handle collision:
            if (game.Matrix[X, Y] != null && !game.Matrix[X, Y].Equals(this))
            {
                return new StaticChar(X, Y - 1, Value);
            }

            // on reaching water:
            if (game.Settings.HasWater && Y == game.Settings.AboveWaterY)
            {
                return new StaticChar(X, Y, Value);
            }

            // on reaching the bottom:
            if (Y == game.Settings.MaxY)
            {
                return new StaticChar(X, Y, Value);
            }

            var objectBelow = game.GameObjects
                .FirstOrDefault(o =>
                    o.X == X &&
                    o.Y == Y + 1);

            // on obstacle:
            if (objectBelow != null)
            {
                return new StaticChar(X, Y, Value);
            }

            return With(y: Y + 1);
        }

        /// <summary>
        /// Moves the char down to the limit. Stops on obstacle.
        /// </summary>
        private WordisObj HandleDown(WordisGame game)
        {
            var firstObjBelow = game.GameObjects
                .Where(o =>
                    o.X == X &&
                    o.Y > Y)
                .OrderBy(o => o.Y)
                .FirstOrDefault();

            if (firstObjBelow == null)
            {
                if (game.Settings.HasWater)
                {
                    return With(y: game.Settings.AboveWaterY);
                }

                return With(y: game.Settings.MaxY);
            }

            // stop before obstacle
            return With(y: firstObjBelow.Y - 1);
        }

        /// <summary>
        /// Moves this char left.
        /// </summary>
        private WordisObj HandleLeft(WordisGame game)
        {
            var firstObjLeft = game.GameObjects
                .Where(o => o.Y == Y && o.X < X)
                .OrderByDescending(o => o.X)
                .FirstOrDefault();

            if (firstObjLeft?.X == X - 1 || X == 0)
            {
                // stay in current place
                return this;
            }

            return With(x: X - 1);
        }

        /// <summary>
        /// Moves this char right.
        /// </summary>
        private WordisObj HandleRight(WordisGame game)
        {
            var firstObjRight = game.GameObjects
                .Where(o => o.Y == Y && o.X > X)
                .OrderBy(o => o.X)
                .FirstOrDefault();

            if (firstObjRight?.X == X + 1 || X == game.Settings.MaxX)
            {
                // stay in current place
                return this;
            }

            return With(x: X + 1);
        }

        private ActiveChar With(
            int? x = null,
            int? y = null) =>
            new ActiveChar(
                x ?? X,
                y ?? Y,
                Value);
    }
}
