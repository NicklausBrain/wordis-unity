﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;
using Newtonsoft.Json;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    public class WordMatch
    {
        [JsonProperty("chars")]
        private readonly WordisChar[] _chars;

        public ISet<WordisChar> CharsSet { get; private set; }

        [JsonConstructor]
        public WordMatch(IEnumerable<WordisChar> chars)
        {
            _chars = chars.ToArray();
            CharsSet = new HashSet<WordisChar>(_chars) ?? new HashSet<WordisChar>();
        }

        [JsonIgnore]
        public string Word => new string(MatchedChars.Select(c => c.Value).ToArray());

        [JsonIgnore]
        public WordisChar[] MatchedChars => _chars;

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
