using System;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Levels;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Campaign;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;
using NUnit.Framework;

namespace Assets.Wordis.BlockPuzzle.GameCore.Tests
{
    /// <summary>
    /// Testing in-game levels
    /// </summary>
    public class GameLevelsTests
    {
        [Test]
        public void WordisTutorial_AfterXSteps_IsCompleted()
        {
            var tutorial = Enumerable
                .Range(0, 20)
                .Aggregate(
                    new WordisTutorialLevel() as IWordisGameLevel,
                    (t, i) => t.Handle(GameEvent.Step));

            Assert.IsTrue(tutorial.IsCompleted);
        }

        [Test]
        public void Letter3Animals_WhenXAnimalsMatched_IsCompleted()
        {
            var requiredMatches =
                Letter3Animals.Animals.Words
                    .Take(Letter3Animals.NeededMatches)
                    .Select(StrToWordMatch)
                    .ToArray();

            var initialState = new Letter3Animals();
            var finalState = initialState
                .WithUpdatedGame(
                    initialState.Game.WithWordMatches(requiredMatches));

            Assert.IsTrue(finalState.IsCompleted);
        }

        [Test]
        public void Letter3Palindromes_WhenXPalindromesMatched_IsCompleted()
        {
            var requiredMatches =
                Letter3Palindromes.ThreeLetterPalindromes.Words
                    .Take(Letter3Palindromes.NeededMatches)
                    .Select(StrToWordMatch)
                    .ToArray();

            var initialState = new Letter3Palindromes();
            var finalState = initialState
                .WithUpdatedGame(
                    initialState.Game.WithWordMatches(requiredMatches));

            Assert.IsTrue(finalState.IsCompleted);
        }

        [Test]
        public void Letter4Palindromes_WhenXPalindromesMatched_IsCompleted()
        {
            var requiredMatches =
                Letter4Palindromes.FourLetterPalindromes.Words
                    .Take(Letter4Palindromes.NeededMatches)
                    .Select(StrToWordMatch)
                    .ToArray();

            var initialState = new Letter4Palindromes();
            var finalState = initialState
                .WithUpdatedGame(
                    initialState.Game.WithWordMatches(requiredMatches));

            Assert.IsTrue(finalState.IsCompleted);
        }

        [Test]
        public void Letter4Animals_WhenXAnimalsMatched_IsCompleted()
        {
            var requiredMatches =
                Letter4Animals.Animals.Words
                    .Take(Letter4Animals.NeededMatches)
                    .Select(StrToWordMatch)
                    .ToArray();

            var initialState = new Letter4Animals();
            var finalState = initialState
                .WithUpdatedGame(
                    initialState.Game.WithWordMatches(requiredMatches));

            Assert.IsTrue(finalState.IsCompleted);
        }

        private static WordMatchEx StrToWordMatch(string word) =>
            new WordMatchEx(new WordMatch(
                word.Select(c => new StaticChar(1, 3, c))), 3, DateTimeOffset.Now);
    }
}
