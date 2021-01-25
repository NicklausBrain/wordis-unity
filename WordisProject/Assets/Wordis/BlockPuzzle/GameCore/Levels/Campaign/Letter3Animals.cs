using System;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
using Assets.Wordis.BlockPuzzle.GameCore.Words;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels.Campaign
{
    /// <summary>
    /// 3-letter animals.
    /// See https://7esl.com/animals-vocabulary-animal-names/
    /// See https://bestforpuzzles.com/lists/animals/3.html
    /// </summary>
    public class Letter3Animals : WordisGameLevelBase<Letter3Animals>, IWordisGameLevel
    {
        private const int NeededMatches = 10;

        private static readonly WordisSettings LevelSettings = new WordisSettings(
            width: 5,
            height: 6,
            minWordMatch: 3,
            waterLevel: 0);

        /// <inheritdoc cref="Letter3Animals"/>
        private static WordsSequence Animals => WordsSequence.FromCsv(
            "cat,dog,rat,pup,pig,bee,cow,owl,fox,bat,elk,ant,fly");

        private Letter3Animals(WordisGame game) : base(game)
        {
        }

        public Letter3Animals() : this(new WordisGame(
            LevelSettings,
            Animals.AsLetterSource()))
        {
        }

        public override string Title => "3-letter animals";

        public override string Goal => $"Match {NeededMatches} animals";

        public override bool IsCompleted =>
            Game.Matches.All
                .Select(m => m.Word)
                .Intersect(Animals.Words, StringComparer.OrdinalIgnoreCase)
                .Count() >= NeededMatches;

        public override Letter3Animals WithUpdatedGame(WordisGame updatedGame) =>
            new Letter3Animals(updatedGame);
    }
}
