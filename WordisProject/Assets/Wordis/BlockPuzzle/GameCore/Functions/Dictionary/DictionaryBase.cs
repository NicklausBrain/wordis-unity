namespace Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary
{
    /// <summary>
    /// Base functionality for language dictionary.
    /// </summary>
    public abstract class DictionaryBase
    {
        /// <summary>
        /// Words and definitions as raw string.
        /// </summary>
        protected abstract string WordsInCsv { get; }
    }
}
