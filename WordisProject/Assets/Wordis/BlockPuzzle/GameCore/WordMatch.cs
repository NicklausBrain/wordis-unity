using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    public class WordMatch
    {
        private readonly WordisChar[] _chars;

        public WordMatch(IEnumerable<WordisChar> chars)
        {
            _chars = chars?.ToArray() ?? Array.Empty<WordisChar>();
        }

        public string Word => new string(MatchedChars.Select(c => c.Value).ToArray());

        public IReadOnlyList<WordisChar> MatchedChars => _chars;

        protected bool Equals(WordMatch other)
        {
            return _chars.SequenceEqual(other._chars);
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
