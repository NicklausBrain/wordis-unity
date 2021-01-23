using Assets.Wordis.BlockPuzzle.GameCore.Functions;

namespace Assets.Wordis.BlockPuzzle.GameCore.Words
{
    public abstract class LetterSource
    {
        public abstract LetterSource Next { get; }

        public abstract char Char { get; }

        public abstract bool IsLast { get; }

        //GetLetterFunc AsFunc => new GetEngLetterFunc();

        //private class LetterSourceFunc : GetLetterFunc
        //{
        //    private readonly LetterSource _letterSource;

        //    public LetterSourceFunc(LetterSource letterSource)
        //    {
        //        _letterSource = letterSource;
        //    }

        //    public override char Invoke()
        //    {
        //        _letterSource.
        //    }
        //}
    }
}
