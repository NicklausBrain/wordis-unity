using System.Collections.Generic;
using System.Linq;
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
        public void Next_AfterFirstWord_ProceedsToSecond()
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

        [Test]
        public void Next_WhenShuffleEnabled_ReturnsShuffledLetters()
        {
            var firstWord = "HORSE";
            var secondWord = "DONKEY";

            var letters = new WordSourceLetters(
                new WordsSequence(
                    new[] { firstWord, secondWord }),
                shuffleWordLetters: true);

            var resultSequence = new List<char>();
            Enumerable.Range(0, firstWord.Length + secondWord.Length) // cat + rat
                .Aggregate((LetterSource)letters, (source, _) =>
                {
                    resultSequence.Add(source.Char);
                    return source.Next;
                });

            var firstShWord = resultSequence.Take(firstWord.Length).ToArray();
            var secondShWord = resultSequence.Skip(firstWord.Length).ToArray();

            Assert.AreEqual(firstWord.Length, firstWord.Intersect(firstWord).Count());
            Assert.AreNotEqual(new string(firstShWord), firstWord); // should be shuffled
            Assert.AreEqual(secondWord.Length, secondShWord.Intersect(secondWord).Count());
            Assert.AreNotEqual(new string(secondShWord), secondWord); // should be shuffled
        }
    }
}
