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
                    return HandleLeft();
                case GameEvent.Right:
                    return HandleRight(game.Settings.Width);
                default:
                    return this;
            }
        }

        /// <summary>
        /// Moves the char down. Converts it to static on obstacle.
        /// </summary>
        private WordisObj HandleStep(WordisGame game)
        {
            // on reaching the bottom:
            if (Y == game.Settings.Height - 1)
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
                return With(y: game.Settings.Height - 1);
            }

            return With(y: firstObjBelow.Y - 1);
        }

        /// <summary>
        /// Moves this char left.
        /// </summary>
        private WordisObj HandleLeft() => With(
            // todo: has a bug, does not count an obstacle
            x: X == 0
                ? 0
                : X - 1);

        /// <summary>
        /// Moves this char right.
        /// </summary>
        private WordisObj HandleRight(int width) => With(
            // todo: has a bug, does not count an obstacle
            x: X == width - 1
                ? X
                : X + 1);

        private ActiveChar With(
            int? x = null,
            int? y = null) =>
            new ActiveChar(
                x ?? X,
                y ?? Y,
                Value);
    }
}
