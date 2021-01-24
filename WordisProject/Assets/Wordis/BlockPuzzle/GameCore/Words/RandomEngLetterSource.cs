using System;
using Assets.Wordis.BlockPuzzle.GameCore.Functions;

namespace Assets.Wordis.BlockPuzzle.GameCore.Words
{
    public class RandomEngLetterSource : LetterSource
    {
        private static readonly GetEngLetterFunc GetEngLetterFunc = new GetEngLetterFunc();

        private readonly Lazy<char> _char = new Lazy<char>(() => GetEngLetterFunc.Invoke());

        protected override char GetChar() => _char.Value;

        public override LetterSource Next => new RandomEngLetterSource();

        public override bool IsLast => false;
    }
}
