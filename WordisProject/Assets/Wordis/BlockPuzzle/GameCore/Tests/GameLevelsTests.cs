﻿using System;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Functions;
using Assets.Wordis.BlockPuzzle.GameCore.Levels;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Campaign;
using Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Wordis.BlockPuzzle.GameCore.Tests
{
    /// <summary>
    /// Testing in-game levels
    /// </summary>
    public class GameLevelsTests
    {
        private readonly IsLegitEngWordFunc _isLegitEngWordFunc = new IsLegitEngWordFunc();

        [Test]
        public void WordisGameLevelBase_WhenGameIsOver_IsCompletedReturnsFalse()
        {
            var initialLevel = new TestLevel();
            var failedLevel = initialLevel.WithUpdatedGame(
                initialLevel.Game
                    .With(new StaticChar(1, 0, 'X'))
                    .With(new StaticChar(1, 1, 'Y'))
                    .With(new StaticChar(1, 2, 'Z')));

            Assert.IsTrue(failedLevel.Game.IsGameOver);
            Assert.IsTrue(failedLevel.IsFailed);
            Assert.IsFalse(failedLevel.IsCompleted);
        }

        [Test]
        public void WordisTutorial_AfterNecessaryInput_IsCompleted()
        {
            var tutorial = new WordisTutorialLevel()
                .WithOutput(Debug.LogWarning)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Left)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Down)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Right)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step);

            tutorial = tutorial.Handle(GameEvent.Match(tutorial.Game.GameObjects.First() as WordisChar));

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
        public void Letter3Animals_AllWordsExists()
        {
            var allWordsExists = Letter3Animals.Animals.Words
                .Aggregate(true, (acc, word) =>
                    acc && CheckWord(word));

            Assert.IsTrue(allWordsExists);
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
        public void Letter3Palindromes_AllWordsExist()
        {
            var allWordsExists = Letter3Palindromes.ThreeLetterPalindromes.Words
                .Aggregate(true, (acc, word) =>
                    acc && CheckWord(word));

            Assert.IsTrue(allWordsExists);
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
        public void Letter4Palindromes_AllWordsExist()
        {
            var allWordsExists = Letter4Palindromes.FourLetterPalindromes.Words
                .Aggregate(true, (acc, word) =>
                    acc && CheckWord(word));

            Assert.IsTrue(allWordsExists);
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

        [Test]
        public void Letter4Animals_AllWordsExist()
        {
            var allWordsExists = Letter4Animals.Animals.Words
                .Aggregate(true, (acc, word) =>
                    acc && CheckWord(word));

            Assert.IsTrue(allWordsExists);
        }

        [Test]
        public void ColorNames_WhenXColorsMatched_IsCompleted()
        {
            var requiredMatches =
                ColorNames.Colors.Words
                    .Take(ColorNames.NeededMatches)
                    .Select(StrToWordMatch)
                    .ToArray();

            var initialState = new ColorNames();
            var finalState = initialState
                .WithUpdatedGame(
                    initialState.Game.WithWordMatches(requiredMatches));

            Assert.IsTrue(finalState.IsCompleted);
        }

        [Test]
        public void ColorNames_AllWordsExist()
        {
            var allWordsExists = ColorNames.Colors.Words
                .Aggregate(true, (acc, word) =>
                    acc && CheckWord(word));

            Assert.IsTrue(allWordsExists);
        }

        private static WordMatchEx StrToWordMatch(string word) =>
            new WordMatchEx(new WordMatch(
                word.Select(c => new StaticChar(1, 3, c))), 3, DateTimeOffset.Now);

        private bool CheckWord(string word)
        {
            var isLegit = _isLegitEngWordFunc.Invoke(word);

            if (!isLegit)
            {
                Debug.LogError($"{word} is not found");
            }

            return isLegit;
        }

        private class TestLevel : WordisGameLevelBase<TestLevel>, IWordisGameLevel
        {
            private static readonly WordisSettings DefaultSettings =
                new WordisSettings(width: 3, height: 3, minWordLength: 3);

            private TestLevel(WordisGame game) : base(game)
            {
            }

            public TestLevel() : base(new WordisGame(DefaultSettings))
            {
            }

            public override string Title => "Test level";

            public override string Goal => "Do testing";

            public override TestLevel WithUpdatedGame(WordisGame updatedGame) =>
                new TestLevel(updatedGame);
        }
    }
}
