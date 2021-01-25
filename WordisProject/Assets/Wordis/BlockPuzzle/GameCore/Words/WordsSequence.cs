using System;
using System.Collections.Generic;
using Assets.Wordis.BlockPuzzle.GameCore.Extensions;

namespace Assets.Wordis.BlockPuzzle.GameCore.Words
{
    public class WordsSequence : WordSource
    {
        private readonly int _index;

        public IReadOnlyList<string> Words { get; }

        private WordsSequence(
            IReadOnlyList<string> words,
            int index)
        {
            if (words == null || words.Count == 0)
                throw new ArgumentNullException(nameof(words));
            Words = words;

            if (index >= words.Count || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            _index = index;
        }

        public WordsSequence(IReadOnlyList<string> words) : this(words, 0)
        {
        }

        /// <inheritdoc />
        public override WordSource Next => IsLast
            ? new WordsSequence(Words, 0)
            : new WordsSequence(Words, _index + 1);

        /// <inheritdoc />
        public override string Word => Words[_index];

        /// <inheritdoc />
        public override bool IsLast => _index == Words.Count - 1;

        /// <summary>
        /// Makes the words order random.
        /// </summary>
        public WordsSequence Shuffle() => new WordsSequence(Words.Shuffle());

        public static WordsSequence FromCsv(string csv)
        {
            var words = csv.Split(
                new[] { ',', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            return new WordsSequence(words);
        }
    }
}
