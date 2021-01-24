namespace Assets.Wordis.BlockPuzzle.GameCore.Words
{
    /// <summary>
    /// Abstract words source/generator.
    /// </summary>
    public abstract class WordSource
    {
        /// <summary>
        /// Proceeds to the next word.
        /// </summary>
        public abstract WordSource Next { get; }

        /// <summary>
        /// Current word.
        /// </summary>
        public abstract string Word { get; }

        /// <summary>
        /// Is it the last word in a sequence.
        /// </summary>
        public abstract bool IsLast { get; }

        /// <summary>
        /// Converts <see cref="WordSource"/> to <see cref="LetterSource"/>.
        /// </summary>
        /// <returns></returns>
        public LetterSource AsLetterSource() => new WordSourceLetters(this);
    }
}

