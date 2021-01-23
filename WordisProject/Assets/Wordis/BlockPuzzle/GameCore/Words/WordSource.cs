namespace Assets.Wordis.BlockPuzzle.GameCore.Words
{
    public abstract class WordSource
    {
        public abstract WordSource Next { get; }

        public abstract string Word { get; }

        public abstract bool IsLast { get; }
    }
}
