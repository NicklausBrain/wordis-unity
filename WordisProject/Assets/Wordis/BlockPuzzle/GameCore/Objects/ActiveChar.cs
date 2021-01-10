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
                    return HandleStep();
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

        // todo: incomplete
        private WordisObj HandleStep() => With(y: Y + 1);

        // todo: incomplete
        /// <summary>
        /// Moves the char down. Stops on obstacle.
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        private WordisObj HandleDown(WordisGame game)
        {
            var firstObjBelow = game.GameObjects
                .FirstOrDefault(o =>
                    o.X == X &&
                    o.Y > Y);

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
            x: X == 0
                ? 0
                : X - 1);

        /// <summary>
        /// Moves this char right.
        /// </summary>
        private WordisObj HandleRight(int width) => With(
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
