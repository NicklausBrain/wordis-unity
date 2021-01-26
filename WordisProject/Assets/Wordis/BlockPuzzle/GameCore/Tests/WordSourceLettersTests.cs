using Assets.Wordis.BlockPuzzle.GameCore.Words;
using NUnit.Framework;

namespace Assets.Wordis.BlockPuzzle.GameCore.Tests
{
    /// <summary>
    /// Tests for <see cref="WordSourceLetters"/>.
    /// </summary>
    public class WordSourceLettersTests
    {
        [Test]
        public void Next_AfterFirstWord_PreceedsToSecond()
        {
            var letters = new WordSourceLetters(
                new WordsSequence(
                    new[] { "cat", "rat" }));

            Assert.AreEqual('R', letters.Next.Next.Next.Char);
        }

        [Test]
        public void Next_OnLastWordAndLetter_Restarts()
        {
            var letters = new WordSourceLetters(
                new WordsSequence(
                    new[] { "cat", "rat" }));

            Assert.AreEqual('C', letters.Next.Next.Next.Next.Next.Next.Char);
        }

        [Test]
        public void IsLast_OnLastLetter_ReturnsTrue()
        {
            var letters = new WordSourceLetters(
                new WordsSequence(
                    new[] { "cat", "rat" }));

            Assert.AreEqual('T', letters.Next.Next.Next.Next.Next.Char);
            Assert.IsTrue(letters.Next.Next.Next.Next.Next.IsLast);
        }
    }
}
