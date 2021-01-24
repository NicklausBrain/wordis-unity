using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Functions;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;
using NUnit.Framework;

namespace Assets.Wordis.BlockPuzzle.GameCore.Tests
{
    /// <summary>
    /// Tests for <see cref="FindWordMatchesFunc"/>
    /// </summary>
    public class FindWordMatchesFuncTests
    {
        private readonly FindWordMatchesFunc _findWordMatchesFunc = new FindWordMatchesFunc();

        [Test]
        public void Invoke_WhenRowContainsWord_TakesIt()
        {
            var y = 1;
            var settings = new WordisSettings(
                width: 4,
                height: 2,
                minWordMatch: 3);

            // [-][C][-][-][-]
            // [X][-][A][T][-]
            // ---------------------
            // [X][C][A][T][-][-][-] - here we have a CAT

            var matrix = new[] { 'X', 'C', 'A', 'T' }
                .Select((@char, x) => new StaticChar(x, y, @char))
                .Aggregate(new WordisGame(settings), (g, c) => g.With(c))
                .Matrix;

            var matches = _findWordMatchesFunc.Invoke(matrix, settings.MinWordLength);

            Assert.AreEqual(new WordMatch(new[]
            {
                new StaticChar(1, y, 'C'),
                new StaticChar(2, y, 'A'),
                new StaticChar(3, y, 'T'),
            }), matches.Single());
        }

        [Test]
        public void Invoke_WhenColumnContainsWord_TakesIt()
        {
            var x = 1;
            var settings = new WordisSettings(
                width: 2,
                height: 5,
                minWordMatch: 3);

            // [-][R][-][-][-]
            // [-][A][-][-][-]
            // [-][T][-][-][-]
            var matrix = new[] { 'X', 'R', 'A', 'T', 'Y' }
                .Select((@char, y) => new StaticChar(x, y, @char))
                .Aggregate(new WordisGame(settings), (g, c) => g.With(c))
                .Matrix;

            var matches = _findWordMatchesFunc.Invoke(matrix, settings.MinWordLength);

            Assert.AreEqual(new WordMatch(new[]
            {
                new StaticChar(x, 1, 'R'),
                new StaticChar(x, 2, 'A'),
                new StaticChar(x, 3, 'T'),
            }), matches.Single());
        }

        [Test]
        public void Invoke_WhenLongerWordAndItsSubwordCanBeMatchedInARow_AcceptsBoth()
        {
            var y = 2;
            var settings = new WordisSettings(
                width: 8,
                height: 3,
                minWordMatch: 4);

            // [-][-][-][E][-][-][-]
            // [F][I][R][-][F][L][Y]
            // ---------------------
            // "Firefly" and "fire" world should be found. "Fly" is too short
            var matrix = new[] { 'F', 'I', 'R', 'E', 'F', 'L', 'Y' }
                .Select((@char, x) => new StaticChar(x, y, @char))
                .Aggregate(new WordisGame(settings), (g, c) => g.With(c))
                .Matrix;

            var matches = _findWordMatchesFunc.Invoke(matrix, settings.MinWordLength);

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(0, y, 'F'),
                new StaticChar(1, y, 'I'),
                new StaticChar(2, y, 'R'),
                new StaticChar(3, y, 'E'),
                new StaticChar(4, y, 'F'),
                new StaticChar(5, y, 'L'),
                new StaticChar(6, y, 'Y'),
            }), matches);

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(0, y, 'F'),
                new StaticChar(1, y, 'I'),
                new StaticChar(2, y, 'R'),
                new StaticChar(3, y, 'E'),
            }), matches);
        }

        [Test]
        public void Invoke_WhenTwoWordMatchInARow_AcceptsBoth()
        {
            var y = 0;
            var settings = new WordisSettings(
                width: 8,
                height: 2,
                minWordMatch: 4);

            // [-][-][-][K][-][-][-]
            // [S][I][C][-][I][L][L] - one end - another start
            // ---------------------
            // [S][I][C][K][I][L][L] - both should match
            var matrix = new[] { 'S', 'I', 'C', 'K', 'I', 'L', 'L' }
                .Select((@char, x) => new StaticChar(x, y, @char))
                .Aggregate(new WordisGame(settings), (g, c) => g.With(c))
                .Matrix;

            var matches = _findWordMatchesFunc.Invoke(matrix, settings.MinWordLength);

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(0, y, 'S'),
                new StaticChar(1, y, 'I'),
                new StaticChar(2, y, 'C'),
                new StaticChar(3, y, 'K'),
            }), matches);

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(3, y, 'K'),
                new StaticChar(4, y, 'I'),
                new StaticChar(5, y, 'L'),
                new StaticChar(6, y, 'L'),
            }), matches);
        }

        [Test]
        public void Invoke_WhenLongerWordAndItsSubwordCanBeMatchedInAColumn_AcceptsBoth()
        {
            var x = 2;
            var settings = new WordisSettings(
                width: 3,
                height: 8,
                minWordMatch: 4);

            // [F][-]
            // [I][-]
            // [R][-]
            // [-][E]
            // [F][-]
            // [L][-]
            // [Y][-]
            // ---------------------
            // "Firefly" and "fire" world should be found. "Fly" is too short
            var matrix = new[] { 'F', 'I', 'R', 'E', 'F', 'L', 'Y' }
                .Select((@char, y) => new StaticChar(x, y, @char))
                .Aggregate(new WordisGame(settings), (g, c) => g.With(c))
                .Matrix;

            var matches = _findWordMatchesFunc.Invoke(matrix, settings.MinWordLength);

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(x, 0, 'F'),
                new StaticChar(x, 1, 'I'),
                new StaticChar(x, 2, 'R'),
                new StaticChar(x, 3, 'E'),
                new StaticChar(x, 4, 'F'),
                new StaticChar(x, 5, 'L'),
                new StaticChar(x, 6, 'Y'),
            }), matches);

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(x, 0, 'F'),
                new StaticChar(x, 1, 'I'),
                new StaticChar(x, 2, 'R'),
                new StaticChar(x, 3, 'E'),
            }), matches);
        }

        [Test]
        public void Invoke_WhenTwoWordMatchInAColumn_AcceptsBoth()
        {
            var x = 0;
            var settings = new WordisSettings(
                width: 3,
                height: 8,
                minWordMatch: 4);

            // [S][-]
            // [I][-]
            // [C][-]
            // [-][K]- one end - another start
            // [I][-]
            // [L][-]
            // [L][-]
            // ---------------------
            // [S][I][C][K][I][L][L] - both should match
            var matrix = new[] { 'S', 'I', 'C', 'K', 'I', 'L', 'L' }
                .Select((@char, y) => new StaticChar(x, y, @char))
                .Aggregate(new WordisGame(settings), (g, c) => g.With(c))
                .Matrix;

            var matches = _findWordMatchesFunc.Invoke(
                matrix, settings.MinWordLength);

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(x, 0, 'S'),
                new StaticChar(x, 1, 'I'),
                new StaticChar(x, 2, 'C'),
                new StaticChar(x, 3, 'K'),
            }), matches);

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(x, 3, 'K'),
                new StaticChar(x, 4, 'I'),
                new StaticChar(x, 5, 'L'),
                new StaticChar(x, 6, 'L'),
            }), matches);
        }

        [Test]
        public void Invoke_WhenColumnAndRowHaveIntersectedMatch_TakesBothWords()
        {
            var settings = new WordisSettings(
                width: 5,
                height: 4,
                minWordMatch: 3);

            // [F]
            // [-][I][R][E]
            // [L]
            // [Y]
            var chars = new[]
            {
                new StaticChar(1, 1, 'F'),
                new StaticChar(2, 1, 'I'),
                new StaticChar(3, 1, 'R'),
                new StaticChar(4, 1, 'E'),
                new StaticChar(1, 2, 'L'),
                new StaticChar(1, 3, 'Y'),
            };
            var matrix = chars
                .Aggregate(new WordisGame(settings), (g, c) => g.With(c))
                .Matrix;

            var matches = _findWordMatchesFunc.Invoke(matrix, settings.MinWordLength);

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(1, 1, 'F'),
                new StaticChar(2, 1, 'I'),
                new StaticChar(3, 1, 'R'),
                new StaticChar(4, 1, 'E'),
            }), matches);

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(1, 1, 'F'),
                new StaticChar(1, 2, 'L'),
                new StaticChar(1, 3, 'Y'),
            }), matches);
        }

        [Test]
        public void Invoke_WhenWordIsTooSmall_IgnoresIt()
        {
            var y = 1;
            var settings = new WordisSettings(
                width: 4,
                height: 2,
                minWordMatch: 4);

            // [-][C][-][-][-]
            // [X][-][A][T][-]
            // ---------------------
            // [X][C][A][T][-][-][-] - here we have a CAT
            var matrix = new[] { 'X', 'C', 'A', 'T' }
                .Select((@char, x) => new StaticChar(x, y, @char))
                .Aggregate(new WordisGame(settings), (g, c) => g.With(c))
                .Matrix;

            var matches = _findWordMatchesFunc.Invoke(matrix, settings.MinWordLength);

            Assert.IsEmpty(matches);
        }

        [Test]
        public void Invoke_WhenInputIsInWrongOrder_FindsTheMatchAnyway()
        {
            var settings = new WordisSettings(
                width: 4,
                height: 2,
                minWordMatch: 3);

            var chars = new[]
            {
                new StaticChar(3, 1, 'T'), // input order is wrong,
                new StaticChar(1, 1, 'C'), // but coordinates are correct
                new StaticChar(2, 1, 'A'),
            };
            var matrix = chars
                .Aggregate(new WordisGame(settings), (g, c) => g.With(c))
                .Matrix;

            var matches = _findWordMatchesFunc.Invoke(matrix, settings.MinWordLength);

            Assert.AreEqual("CAT", matches.First().Word);
        }

        [Test]
        // pseudo-performance test
        public void Invoke_ForLongWrongVector_ZZZ()
        {
            var chars = new[] { 'C', 'T', 'C', 'T', 'C', 'T', 'C', 'T', 'C' };

            var settings = new WordisSettings(
                width: chars.Length,
                height: chars.Length,
                minWordMatch: 3);

            var matrix = chars
                .SelectMany((_, y) => chars.Select((@char, x) => new StaticChar(x, y, @char)))
                .Aggregate(new WordisGame(settings), (g, c) => g.With(c))
                .Matrix;

            var matches = _findWordMatchesFunc.Invoke(matrix, settings.MinWordLength);

            Assert.IsEmpty(matches);
        }
    }
}