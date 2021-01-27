using Assets.Wordis.BlockPuzzle.GameCore.Words;
using NUnit.Framework;

namespace Assets.Wordis.BlockPuzzle.GameCore.Tests
{
    /// <summary>
    /// Tests for <see cref="WordsSequence"/>.
    /// </summary>
    public class WordsSequenceTests
    {
        [Test]
        public void Next_OnFirstWord_ProceedsToSecond()
        {
            var words = new WordsSequence(
                new[] { "cat", "rat", "dog" });

            Assert.AreEqual("rat", words.Next.Word);
        }

        [Test]
        public void Next_OnLastWord_Restarts()
        {
            var words = new WordsSequence(
                new[] { "cat", "rat", "dog" });

            Assert.AreEqual("cat", words.Next.Next.Next.Word);
        }

        [Test]
        public void IsLast_OnLastWord_ReturnsTrue()
        {
            var words = new WordsSequence(
                new[] { "cat", "rat", "dog" });

            Assert.AreEqual("dog", words.Next.Next.Word);
            Assert.IsTrue(words.Next.Next.IsLast);
        }
    }
}
