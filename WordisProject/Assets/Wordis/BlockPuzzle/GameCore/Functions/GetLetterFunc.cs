namespace Assets.Wordis.BlockPuzzle.GameCore.Functions
{
    /// <summary>
    /// Generates a letter to be used in game.
    /// </summary>
    public abstract class GetLetterFunc
    {
        /// <summary>
        /// Invokes the function.
        /// </summary>
        /// <returns>Some letter.</returns>
        public abstract char Invoke();
    }
}
