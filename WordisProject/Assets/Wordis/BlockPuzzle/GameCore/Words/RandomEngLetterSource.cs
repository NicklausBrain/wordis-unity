using System;
using Assets.Wordis.BlockPuzzle.GameCore.Functions;

namespace Assets.Wordis.BlockPuzzle.GameCore.Words
{
    public class RandomEngLetterSource : LetterSource
    {
        private static readonly GetEngLetterFunc GetEngLetterFunc = new GetEngLetterFunc();

        private readonly Lazy<RandomEngLetterSource> _next =
            new Lazy<RandomEngLetterSource>(() => new RandomEngLetterSource());

        private readonly char _char;

        private RandomEngLetterSource(char @char)
        {
            _char = @char;
        }

        public RandomEngLetterSource() :
            this(GetEngLetterFunc.Invoke())
        {
        }

        protected override char GetChar() => _char;

        public override LetterSource Next => _next.Value;

        public override bool IsLast => false;
    }
}
