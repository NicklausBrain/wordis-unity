using System;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
using Assets.Wordis.BlockPuzzle.GameCore.Words;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels.Campaign
{
    /// <summary>
    /// 3-letter animals.
    /// See https://7esl.com/animals-vocabulary-animal-names/
    /// See https://bestforpuzzles.com/lists/animals/4.html
    /// </summary>
    public class Letter4Animals : WordisGameLevelBase<Letter4Animals>, IWordisGameLevel
    {
        public const int NeededMatches = 20;

        public static readonly WordisSettings LevelSettings = new WordisSettings(
            width: 5,
            height: 6,
            minWordMatch: 4,
            waterLevel: 0);

        /// <inheritdoc cref="Letter4Animals "/>
        public static WordsSequence Animals => WordsSequence.FromCsv(
@"Bear,Boar,Buck,Bull,Calf,Cavy,Colt,Cony,Coon,Deer,Foal,Gaur,Goat,Guib,
Hare,Hogg,Ibex,Lamb,Lion,Lynx,Maki,Mara,Mare,Mink,Moke,Mole,Mona,Mule,Musk,
Orca,Oryx,Oxen,Puma,Seal,Vole,Wolf");

        private Letter4Animals(WordisGame game) : base(game)
        {
        }

        public Letter4Animals() : this(new WordisGame(
            LevelSettings,
            Animals
                .Shuffle()
                .AsLetterSource()))
        {
        }

        public override string Title => "4-letter animals";

        public override string Goal => $"Match {NeededMatches} animals";

        public override bool IsCompleted =>
            Game.Matches.All
                .Select(m => m.Word)
                .Intersect(Animals.Words, StringComparer.OrdinalIgnoreCase)
                .Count() >= NeededMatches;

        public override Letter4Animals WithUpdatedGame(WordisGame updatedGame) =>
            new Letter4Animals(updatedGame);
    }
}
