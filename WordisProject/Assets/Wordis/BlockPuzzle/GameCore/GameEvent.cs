using Assets.Wordis.BlockPuzzle.GameCore.Objects;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    /// <summary>
    /// Represents possible in-game input made by the player.
    /// </summary>
    public class GameEvent
    {
        /// <summary>
        /// No input.
        /// </summary>
        public static readonly GameEvent None = new GameEvent();

        /// <summary>
        /// This is a primary event commanding the game to proceed to the next step.
        /// Evaluates success and failure conditions.
        /// </summary>
        public static readonly GameEvent Step = new GameEvent();

        /// <summary>
        /// Speed up the current <see cref="WordisObj"/> to be processed.
        /// </summary>
        public static readonly GameEvent Down = new GameEvent();

        /// <summary>
        /// Move the current <see cref="WordisObj"/> to the left.
        /// </summary>
        public static readonly GameEvent Left = new GameEvent();

        /// <summary>
        /// Move the current <see cref="WordisObj"/> to the right.
        /// </summary>
        public static readonly GameEvent Right = new GameEvent();

        /// <summary>
        /// User approves highlighted words to match.
        /// </summary>
        public static readonly GameEvent Match = new GameEvent();
    }
}
