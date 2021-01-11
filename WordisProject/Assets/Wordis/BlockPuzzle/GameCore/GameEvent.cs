using Assets.Wordis.BlockPuzzle.GameCore.Objects;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    /// <summary>
    /// Represents possible in-game input made by the player.
    /// </summary>
    public enum GameEvent
    {
        /// <summary>
        /// No input.
        /// </summary>
        None,

        /// <summary>
        /// This is a primary event commanding the game to proceed to the next step.
        /// Evaluates success and failure conditions.
        /// </summary>
        Step,

        /// <summary>
        /// Speed up the current <see cref="WordisObj"/> to be processed.
        /// </summary>
        Down,

        /// <summary>
        /// Move the current <see cref="WordisObj"/> to the left.
        /// </summary>
        Left,

        /// <summary>
        /// Move the current <see cref="WordisObj"/> to the right.
        /// </summary>
        Right,
    }
}
