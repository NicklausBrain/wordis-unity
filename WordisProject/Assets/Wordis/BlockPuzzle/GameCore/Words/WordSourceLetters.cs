using System;

namespace Assets.Wordis.BlockPuzzle.GameCore.Words
{
    /// <summary>
    /// Represents words sequence as a letters sequence.
    /// </summary>
    public class WordSourceLetters : LetterSource
    {
        private readonly WordSource _wordSource;
        private readonly LetterSource _letterSource;

        private WordSourceLetters(
            WordSource wordSource,
            LetterSource letterSource)
        {
            _wordSource = wordSource;
            _letterSource = letterSource;
        }

        public WordSourceLetters(WordSource wordSource)
            : this(
                wordSource,
                new WordLetters(wordSource.Word))
        {
        }

        protected override char GetChar() => _letterSource.Char;

        public override LetterSource Next =>
            IsLast
                ? new WordSourceLetters(_wordSource.Next)
                : _letterSource.IsLast
                    ? new WordSourceLetters(_wordSource.Next)
                    : new WordSourceLetters(_wordSource, _letterSource.Next);

        public override bool IsLast => _wordSource.IsLast && _letterSource.IsLast;
    }
}
