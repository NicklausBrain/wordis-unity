using System.Linq;

namespace Assets.Wordis.BlockPuzzle.GameCore
{
    /// <summary>
    /// Encapsulates Game score logic.
    /// </summary>
    public class WordisScore
    {
        /// <summary>
        /// Score per char clear.
        /// </summary>
        public const int CharScore = 20;

        /// <summary>
        /// Score on matching the word.
        /// </summary>
        public const int WordScore = 100;

        /// <summary>
        /// Additional score multiplier on matching more than 1 word.
        /// </summary>
        public const int MultiWordScoreMultiplier = 50;

        private readonly WordisGame _game;

        public WordisScore(WordisGame game)
        {
            _game = game;
        }

        /// <summary>
        /// Accumulated score.
        /// </summary>
        public int Value =>
            _game.Matches.All
                .GroupBy(m => m.GameEventId)
                .Sum(g => GetMatchScore(g.ToArray()));

        private static int GetMatchScore(WordMatchEx[] match)
        {
            int scorePerMatch = WordScore + (match.Length - 1) * MultiWordScoreMultiplier;
            int score = match.Length * scorePerMatch + match.SelectMany(m => m.MatchedChars).Count() * CharScore;
            return score;
        }
    }
}
