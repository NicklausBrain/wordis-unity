namespace Assets.Wordis.BlockPuzzle.GameCore.Functions
{
    /// <summary>
    /// Determines if a word is a legit word.
    /// </summary>
    public abstract class IsLegitWordFunc
    {
        /// <summary>
        /// Invokes the function.
        /// </summary>
        /// <param name="word">Target word.</param>
        /// <returns>True if the word is legit.</returns>
        public abstract bool Invoke(string word);
    }
}
