using System;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    public class WordMatchEx : WordMatch
    {
        public WordMatchEx(
            WordMatch wordMatch,
            int gameStep,
            DateTimeOffset timestamp)
            : base(
                wordMatch.MatchedChars)
        {
            GameStep = gameStep;
            Timestamp = timestamp;
        }

        public int GameStep { get; }

        public DateTimeOffset Timestamp { get; }

        protected bool Equals(WordMatchEx other)
        {
            return (ReferenceEquals(this, other));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WordMatchEx)obj);
        }

        public override int GetHashCode()
        {
            return MatchedChars.GetHashCode();
        }
    }
}
