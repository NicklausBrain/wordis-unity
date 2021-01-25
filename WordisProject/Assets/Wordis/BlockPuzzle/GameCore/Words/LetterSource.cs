namespace Assets.Wordis.BlockPuzzle.GameCore.Words
{
    /// <summary>
    /// Abstract letters source/generator.
    /// </summary>
    public abstract class LetterSource
    {
        /// <summary>
        /// Gets the current character.
        /// </summary>
        protected abstract char GetChar();

        /// <summary>
        /// Proceeds to the next character.
        /// </summary>
        public abstract LetterSource Next { get; }

        /// <summary>
        /// Current letter. Uppercase.
        /// </summary>
        public virtual char Char => char.ToUpperInvariant(GetChar());

        /// <summary>
        /// Is the last letter in the sequence.
        /// </summary>
        public abstract bool IsLast { get; }
    }
}
