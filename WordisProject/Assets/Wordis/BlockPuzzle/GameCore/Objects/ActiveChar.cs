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
                    return HandleDown(game.Settings.Height);
                case GameEvent.Left:
                    return HandleLeft();
                case GameEvent.Right:
                    return HandleRight();
                default:
                    return this;
            }
        }

        // todo: incomplete
        private WordisObj HandleStep() => With(y: Y + 1);

        // todo: incomplete
        private WordisObj HandleDown(int height) => With(y: height - 1);

        // todo: incomplete
        private WordisObj HandleLeft() => With(x: X - 1);

        // todo: incomplete
        private WordisObj HandleRight() => With(x: X + 1);

        private ActiveChar With(
            int? x = null,
            int? y = null) =>
            new ActiveChar(
                x ?? X,
                y ?? Y,
                Value);
    }
}
