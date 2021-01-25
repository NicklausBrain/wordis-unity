using System;

namespace Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary
{
    /// <summary>
    /// Abstract words lookup dictionary.
    /// </summary>
    public abstract class WordLookupBase
    {
        private readonly Lazy<string[]> _words;

        protected WordLookupBase()
        {
            _words = new Lazy<string[]>(() => ParseCsv(WordsInCsv));
        }

        /// <summary>
        /// Dictionary in csv format (no word definitions expected).
        /// </summary>
        protected abstract string WordsInCsv { get; }

        /// <summary>
        /// Checks if a word exists in a dictionary.
        /// </summary>
        /// <param name="word">Word to check.</param>
        /// <returns>True if the word exists, otherwise False.</returns>
        public virtual bool Check(string word) =>
            !string.IsNullOrWhiteSpace(word) &&
            Array.BinarySearch(
                _words.Value,
                word.Trim(),
                StringComparer.OrdinalIgnoreCase) > -1;

        private static string[] ParseCsv(string csv) =>
            (csv ?? string.Empty).Split(
                new[] { ',', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);
    }
}
