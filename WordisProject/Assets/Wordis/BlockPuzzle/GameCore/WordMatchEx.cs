using System;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    public class WordMatchEx : WordMatch
    {
        public WordMatchEx(
            WordMatch wordMatch,
            int gameStep,
            DateTimeOffset timestamp) : base(wordMatch.MatchedChars)
        {
            GameStep = gameStep;
            Timestamp = timestamp;
        }

        public int GameStep { get; }

        public DateTimeOffset Timestamp { get; }
    }
}
