using Assets.Wordis.BlockPuzzle.GameCore.Words;
using NUnit.Framework;

namespace Assets.Wordis.BlockPuzzle.GameCore.Tests
{
    /// <summary>
    /// Tests for <see cref="WordLetters"/>.
    /// </summary>
    public class WordLettersTests
    {
        [Test]
        public void Next_OnFirstLetter_ProceedsToSecond()
        {
            var letters = new WordLetters("RAT");

            Assert.AreEqual('A', letters.Next.Char);
        }

        [Test]
        public void Next_OnLastLetter_Restarts()
        {
            var letters = new WordLetters("RAT");

            Assert.AreEqual('R', letters.Next.Next.Next.Char);
        }

        [Test]
        public void IsLast_OnLastLetter_ReturnsTrue()
        {
            var letters = new WordLetters("RAT");

            Assert.AreEqual('T', letters.Next.Next.Char);
            Assert.IsTrue(letters.Next.Next.IsLast);
        }
    }
}
