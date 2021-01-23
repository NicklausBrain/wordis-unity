using System;
using System.Collections.Generic;

namespace Assets.Wordis.BlockPuzzle.GameCore.Words
{
    public class WordsSequence : WordSource
    {
        private readonly IReadOnlyList<string> _words;
        private readonly int _index;

        private WordsSequence(
            IReadOnlyList<string> words,
            int index)
        {
            if (words == null || words.Count == 0)
                throw new ArgumentNullException(nameof(words));
            _words = words;

            if (index >= words.Count || index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            _index = index;
        }

        public WordsSequence(IReadOnlyList<string> words) : this(words, 0)
        {
        }

        public override WordSource Next => new WordsSequence(_words, _index + 1);

        public override string Word => _words[_index];

        public override bool IsLast => _index == _words.Count - 1;

        public LetterSource AsLetterSource() => new WordLetters(this.Word);

        //private class MultiWordsLetters : WordLetters
        //{
        //    private readonly string[] _words;

        //    public MultiWordsLetters(string[] words) : base(words.First())
        //    {
        //        _words = words;
        //    }

        //    public override LetterSource Next
        //    {
        //        get
        //        {
        //            if (base.IsLast)
        //            {
        //                return new WordLetters(_words);
        //            }
        //        }
        //    }
        //}
    }
}
