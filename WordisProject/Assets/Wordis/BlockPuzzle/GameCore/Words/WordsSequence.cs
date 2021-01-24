using System;
using System.Collections.Generic;

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

        public override WordSource Next => IsLast
            ? this
            : new WordsSequence(Words, _index + 1);

        public override string Word => Words[_index];

        public override bool IsLast => _index == Words.Count - 1;

        public static WordsSequence FromCsv(string csv)
        {
            var words = csv.Split(new[] { ',', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return new WordsSequence(words);
        }
    }
}
