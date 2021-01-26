using System;

namespace Assets.Wordis.BlockPuzzle.GameCore.Words
{
    public class WordLetters : LetterSource
    {
        private readonly string _word;
        private readonly int _index;

        private WordLetters(string word, int index)
        {
            if (string.IsNullOrWhiteSpace(word))
                throw new ArgumentNullException(nameof(word));
            _word = word;

            if (index >= _word.Length || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            _index = index;
        }

        protected override char GetChar() => _word[_index];

        public WordLetters(string word) : this(word, 0)
        {
        }

        public override LetterSource Next =>
            IsLast
                ? new WordLetters(_word)
                : new WordLetters(_word, _index + 1);

        public override bool IsLast => _index == _word.Length - 1;
    }
}
