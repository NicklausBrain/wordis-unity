using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Functions.Lookup.English;
using NUnit.Framework;

namespace Assets.Wordis.BlockPuzzle.GameCore.Tests
{
    /// <summary>
    /// Tests for <see cref="EngLookup"/>.
    /// </summary>
    public class EngLookupTests
    {
        private static readonly EngLookup EngLookup = new EngLookup();

        [Test]
        public void CheckEveryAlphabetLetter_WhenTheWordExists_ReturnsTrue()
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

            var results = azWords.Select(word => EngLookup.Check(word)).ToList();

            results.ForEach(Assert.True);
        }

        [Test]
        public void CheckEveryAlphabetLetter_WhenTheWordDoesNotExist_ReturnsFalse()
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

            var results = azWords.Select(word => EngLookup.Check(word)).ToList();

            results.ForEach(Assert.False);
        }

        [Test]
        public void Check_ForEmptyWord_ReturnsFalse() =>
            Assert.IsFalse(EngLookup.Check(string.Empty));

        [Test]
        public void Check_ForNullWord_ReturnsFalse() =>
            Assert.IsFalse(EngLookup.Check(null));

        [Test]
        public void Check_ForFirstSpecialCharacter_ReturnsFalse() =>
            Assert.IsFalse(EngLookup.Check("`hello"));

        [Test]
        public void Check_ForEmptyCharacters_TrimsThem() =>
            Assert.IsTrue(EngLookup.Check(" hello \r\n"));

        [Test]
        public void WarmUp_ShouldNotFail() =>
            new EngLookup().WarmUp();
    }
}
