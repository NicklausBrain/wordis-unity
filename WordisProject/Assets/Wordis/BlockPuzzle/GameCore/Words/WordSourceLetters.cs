using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Extensions;

namespace Assets.Wordis.BlockPuzzle.GameCore.Words
{
    /// <summary>
    /// Represents words sequence as a letters sequence.
    /// </summary>
    public class WordSourceLetters : LetterSource
    {
        private readonly WordSource _wordSource;
        private readonly LetterSource _letterSource;
        private readonly bool _shuffleWordLetters;

        private WordSourceLetters(
            WordSource wordSource,
            LetterSource letterSource,
            bool shuffleWordLetters)
        {
            _wordSource = wordSource;
            _letterSource = letterSource;
            _shuffleWordLetters = shuffleWordLetters;
        }

        public WordSourceLetters(
            WordSource wordSource,
            bool shuffleWordLetters = false)
            : this(
                wordSource,
                new WordLetters(
                    shuffleWordLetters
                    ? new string(wordSource.Word.ToCharArray().Shuffle().ToArray())
                    : wordSource.Word),
                shuffleWordLetters)
        {
        }

        protected override char GetChar() => _letterSource.Char;

        public override LetterSource Next =>
            IsLast
                ? new WordSourceLetters(_wordSource.Next, _shuffleWordLetters)
                : _letterSource.IsLast
                    ? new WordSourceLetters(_wordSource.Next, _shuffleWordLetters)
                    : new WordSourceLetters(_wordSource, _letterSource.Next, _shuffleWordLetters);

        public override bool IsLast => _wordSource.IsLast && _letterSource.IsLast;
    }
}
