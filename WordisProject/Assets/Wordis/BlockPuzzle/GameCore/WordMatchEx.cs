using System;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    /// <summary>
    /// Extension for essential <see cref="WordMatch"/>.
    /// </summary>
    public class WordMatchEx : WordMatch
    {
        public WordMatchEx(
            WordMatch wordMatch,
            int gameEventId,
            DateTimeOffset timestamp)
            : base(
                wordMatch.MatchedChars)
        {
            GameEventId = gameEventId;
            Timestamp = timestamp;
        }

        /// <summary>
        /// Zero-based event id (when the match occured).
        /// </summary>
        public int GameEventId { get; }

        /// <summary>
        /// Match timestamp.
        /// </summary>
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
