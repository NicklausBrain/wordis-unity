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
        public const int NeededMatches = 5;

        public static readonly WordisSettings LevelSettings = new WordisSettings(
            width: 7,
            height: 7,
            minWordMatch: 3,
            waterLevel: 0);

        /// <inheritdoc cref="Letter3Animals"/>
        public static WordsSequence Animals => WordsSequence.FromCsv(
            "cat,dog,rat,pup,pig,bee,cow,owl,fox,bat,elk,ant,fly");

        private Letter3Animals(WordisGame game) : base(game)
        {
        }

        public Letter3Animals() : this(new WordisGame(
            LevelSettings,
            Animals
                .Shuffle()
                .AsLetterSource(shuffleWordLetters: true)))
        {
        }

        public override string Title => "3-letter animals"; // todo: localize

        public override string Goal => $"Match {NeededMatches} animals"; // todo: localize

        public override string Progress => $"{MatchedAnimals} of {NeededMatches} animals matched"; // todo: localize

        public override bool IsCompleted => MatchedAnimals >= NeededMatches;

        public override Letter3Animals WithUpdatedGame(WordisGame updatedGame) =>
            new Letter3Animals(updatedGame);

        private int MatchedAnimals =>
            Game.Matches.All
                .Select(m => m.Word)
                .Intersect(Animals.Words, StringComparer.OrdinalIgnoreCase)
                .Count();
    }
}
