using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary.English;
using NUnit.Framework;

namespace Assets.Wordis.BlockPuzzle.GameCore.Tests
{
    /// <summary>
    /// Tests for <see cref="EngDict"/>.
    /// </summary>
    public class EngDictTests
    {
        private static readonly EngDict EngDict = new EngDict();

        [Test]
        public void DefineEveryAlphabetLetter_WhenTheWordExists_ReturnsDefinitions()
        {
            var azWords = new[]
            {
                "ark",
                "bard",
                "cellar",
                "door",
                "ELF",
                "FinD",
                "gamE",
                "Hero",
                "inn",
                "jOb",
                "kind",
                "life",
                "mirror",
                "new",
                "ocean",
                "pointer",
                "query",
                "realm",
                "sea",
                "turn",
                "update",
                "velocity",
                "winner",
                "xylophone",
                "yard",
                "zoom",
            };

            var results = azWords.Select(word => EngDict.Define(word)).ToList();

            results.ForEach(Assert.IsNotEmpty);
        }

        [Test]
        public void DefineEveryAlphabetLetter_WhenTheWordDoesNotExist_ReturnsEmpty()
        {
            var azWords = new[]
            {
                "arkz",
                "bardz",
                "cellarz",
                "doorz",
                "ELFz",
                "FinDz",
                "gamEz",
                "Heroz",
                "innz",
                "jObz",
                "kindz",
                "lifez",
                "mirrorz",
                "newz",
                "oceanz",
                "pointerz",
                "queryz",
                "realmz",
                "seaz",
                "turnz",
                "updatez",
                "velocityz",
                "winnerz",
                "xylophonez",
                "yardz",
                "zoomz",
            };

            var results = azWords.Select(word => EngDict.Define(word)).ToList();

            results.ForEach(Assert.IsEmpty);
        }

        [Test]
        public void Define_ForEmptyWord_ReturnsEmpty() =>
            Assert.IsEmpty(EngDict.Define(string.Empty));

        [Test]
        public void Define_ForNullWord_ReturnsEmpty() =>
            Assert.IsEmpty(EngDict.Define(null));

        [Test]
        public void Define_ForFirstSpecialCharacter_ReturnsEmpty() =>
            Assert.IsEmpty(EngDict.Define("`hello"));

        [Test]
        public void Define_ForEmptyCharacters_TrimsThem() =>
            Assert.IsNotEmpty(EngDict.Define(" hello \r\n"));

        [Test]
        public void WarmUp_ShouldNotFail() =>
            new EngDict().WarmUp();
    }
}
