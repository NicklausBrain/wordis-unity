using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    public class WordMatch
    {
        private readonly Lazy<WordisChar[]> _chars;

        public WordMatch(
            IEnumerable<WordisChar> chars)
        {
            _chars = new Lazy<WordisChar[]>(
                () => chars?.ToArray() ?? Array.Empty<WordisChar>());
        }

        public string Word => new string(MatchedChars.Select(c => c.Value).ToArray());

        public IReadOnlyList<WordisChar> MatchedChars => _chars.Value;

        protected bool Equals(WordMatch other)
        {
            return _chars.Value.SequenceEqual(other._chars.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((WordMatch)obj);
        }

        public override int GetHashCode()
        {
            return _chars.GetHashCode();
        }
    }
}
