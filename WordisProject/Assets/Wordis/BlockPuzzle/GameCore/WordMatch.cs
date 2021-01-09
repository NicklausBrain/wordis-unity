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
            int gameStep,
            IEnumerable<WordisChar> chars)
        {
            GameStep = gameStep;
            _chars = new Lazy<WordisChar[]>(
                () => chars?.ToArray() ?? Array.Empty<WordisChar>());
        }

        public int GameStep { get; }

        public string Word => new string(MatchedChars.Select(c => c.Value).ToArray());

        public IReadOnlyList<WordisChar> MatchedChars => _chars.Value;
    }
}
