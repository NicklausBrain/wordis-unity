using System;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
using Assets.Wordis.BlockPuzzle.GameCore.Words;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels.Campaign
{
    /// <summary>
    /// See https://7esl.com/colour-vocabulary/
    /// </summary>
    public class ColorNames : WordisGameLevelBase<ColorNames>, IWordisGameLevel
    {
        public const int NeededMatches = 5;

        public static readonly WordisSettings LevelSettings = new WordisSettings(
            width: 9,
            height: 9,
            minWordLength: 3,
            waterLevel: 3);

        public static WordsSequence Colors => WordsSequence.FromCsv(
@"White,Yellow,Blue,Red,Green,Black,Brown,Azure,Ivory,Teal,Silver,
Purple,Gray,Orange,Maroon,Coral,Fuchsia,Wheat,Lime,Crimson,Khaki,
Pink,Magenta,Olden,Plum,Olive,Cyan");

        private ColorNames(WordisGame game) : base(game)
        {
        }

        public ColorNames() : this(new WordisGame(
            LevelSettings,
            Colors
                .Shuffle()
                .AsLetterSource(shuffleWordLetters: true)))
        {
        }

        public override string Title => "Color names"; // todo: localize

        public override string Goal =>
            $"Match {NeededMatches} colors\n" +
            $"Like '{Colors.Word.ToUpperInvariant()}'"; // todo: localize

        public override string Progress => $"{MatchedColors} of {NeededMatches} colors matched"; // todo: localize

        public override bool IsCompleted => MatchedColors >= NeededMatches;

        public override ColorNames WithUpdatedGame(WordisGame updatedGame) =>
            new ColorNames(updatedGame);

        private int MatchedColors => Game.Matches.All
                .Select(m => m.Word)
                .Intersect(Colors.Words, StringComparer.OrdinalIgnoreCase)
                .Count();
    }
}
