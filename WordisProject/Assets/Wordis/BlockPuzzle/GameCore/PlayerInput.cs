using Assets.Wordis.BlockPuzzle.GameCore.Objects;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    /// <summary>
    /// Represents possible in-game input made by the player.
    /// </summary>
    public enum PlayerInput
    {
        /// <summary>
        /// Player has no input.
        /// </summary>
        None,

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
