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
        [Test]
        public void Invoke_WhenRowContainsWord_TakesIt()
        {
            var y = 1;
            var findWordMatchesFunc = new FindWordMatchesFunc(
                isLegitWordFunc: new IsLegitEngWordFunc(),
                minWordLength: 3);

            // [-][C][-][-][-]
            // [X][-][A][T][-]
            // ---------------------
            // [X][C][A][T][-][-][-] - here we have a CAT
            var chars = new[] { 'X', 'C', 'A', 'T' };

            var matches = findWordMatchesFunc.Invoke(
                chars.Select((@char, x) => new StaticChar(x, y, @char)));

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
            var findWordMatchesFunc = new FindWordMatchesFunc(
                isLegitWordFunc: new IsLegitEngWordFunc(),
                minWordLength: 3);

            // [-][R][-][-][-]
            // [-][A][-][-][-]
            // [-][T][-][-][-]
            var chars = new[] { 'X', 'R', 'A', 'T', 'Y' };

            var matches = findWordMatchesFunc.Invoke(
                chars.Select((@char, y) => new StaticChar(x, y, @char)));

            Assert.AreEqual(new WordMatch(new[]
            {
                new StaticChar(x, 1, 'R'),
                new StaticChar(x, 2, 'A'),
                new StaticChar(x, 3, 'T'),
            }), matches.Single());
        }

        [Test]
        public void Invoke_WhenLongerWordCanBeMatchedInARow_TakesIt()
        {
            var y = 2;
            var findWordMatchesFunc = new FindWordMatchesFunc(
                isLegitWordFunc: new IsLegitEngWordFunc(),
                minWordLength: 4);

            // [-][-][-][E][-][-][-]
            // [F][I][R][-][F][L][Y]
            // ---------------------
            // [F][I][R][E][F][L][Y] - longer world should count
            var chars = new[] { 'F', 'I', 'R', 'E', 'F', 'L', 'Y' };

            var matches = findWordMatchesFunc.Invoke(
                chars.Select((@char, x) => new StaticChar(x, y, @char)));

            Assert.AreEqual(new WordMatch(new[]
            {
                new StaticChar(0, y, 'F'),
                new StaticChar(1, y, 'I'),
                new StaticChar(2, y, 'R'),
                new StaticChar(3, y, 'E'),
                new StaticChar(4, y, 'F'),
                new StaticChar(5, y, 'L'),
                new StaticChar(6, y, 'Y'),
            }), matches.Single());
        }

        [Test]
        public void Invoke_WhenTwoWordMatchInARow_AcceptsBoth()
        {
            var y = 0;
            var findWordMatchesFunc = new FindWordMatchesFunc(
                isLegitWordFunc: new IsLegitEngWordFunc(),
                minWordLength: 4);
            // [-][-][-][K][-][-][-]
            // [S][I][C][-][I][L][L] - one end - another start
            // ---------------------
            // [S][I][C][K][I][L][L] - both should match
            var chars = new[] { 'S', 'I', 'C', 'K', 'I', 'L', 'L' };

            var matches = findWordMatchesFunc.Invoke(
                chars.Select((@char, x) => new StaticChar(x, y, @char)));

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
        public void Invoke_WhenLongerWordCanBeMatchedInAColumn_TakesIt()
        {
            var x = 2;
            var findWordMatchesFunc = new FindWordMatchesFunc(
                isLegitWordFunc: new IsLegitEngWordFunc(),
                minWordLength: 4);

            // [F][-]
            // [I][-]
            // [R][-]
            // [-][E]
            // [F][-]
            // [L][-]
            // [Y][-]
            // ---------------------
            // [F][I][R][E][F][L][Y] - longer world should count
            var chars = new[] { 'F', 'I', 'R', 'E', 'F', 'L', 'Y' };

            var matches = findWordMatchesFunc.Invoke(
                chars.Select((@char, y) => new StaticChar(x, y, @char)));

            Assert.AreEqual(new WordMatch(new[]
            {
                new StaticChar(x, 0, 'F'),
                new StaticChar(x, 1, 'I'),
                new StaticChar(x, 2, 'R'),
                new StaticChar(x, 3, 'E'),
                new StaticChar(x, 4, 'F'),
                new StaticChar(x, 5, 'L'),
                new StaticChar(x, 6, 'Y'),
            }), matches.Single());
        }

        [Test]
        public void Invoke_WhenTwoWordMatchInAColumn_AcceptsBoth()
        {
            var x = 0;
            var findWordMatchesFunc = new FindWordMatchesFunc(
                isLegitWordFunc: new IsLegitEngWordFunc(),
                minWordLength: 4);

            // [S][-]
            // [I][-]
            // [C][-]
            // [-][K]- one end - another start
            // [I][-]
            // [L][-]
            // [L][-]
            // ---------------------
            // [S][I][C][K][I][L][L] - both should match
            var chars = new[] { 'S', 'I', 'C', 'K', 'I', 'L', 'L' };

            var matches = findWordMatchesFunc.Invoke(
                chars.Select((@char, y) => new StaticChar(x, y, @char)));

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
            var findWordMatchesFunc = new FindWordMatchesFunc(
                isLegitWordFunc: new IsLegitEngWordFunc(),
                minWordLength: 3);

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

            var matches = findWordMatchesFunc.Invoke(chars);

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
            var findWordMatchesFunc = new FindWordMatchesFunc(
                isLegitWordFunc: new IsLegitEngWordFunc(),
                minWordLength: 4);

            // [-][C][-][-][-]
            // [X][-][A][T][-]
            // ---------------------
            // [X][C][A][T][-][-][-] - here we have a CAT
            var chars = new[] { 'X', 'C', 'A', 'T' };

            var matches = findWordMatchesFunc.Invoke(
                chars.Select((@char, x) => new StaticChar(x, y, @char)));

            Assert.IsEmpty(matches);
        }

        [Test]
        public void Invoke_WhenInputIsInWrongOrder_FindsTheMatchAnyway()
        {
            var findWordMatchesFunc = new FindWordMatchesFunc(
                isLegitWordFunc: new IsLegitEngWordFunc(),
                minWordLength: 3);

            var matches = findWordMatchesFunc.Invoke(
                new[]
                {
                    new StaticChar(3, 1, 'T'), // input order is wrong,
                    new StaticChar(1, 1, 'C'), // but coordinates are correct
                    new StaticChar(2, 1, 'A'),
                });

            Assert.AreEqual("CAT", matches.First().Word);
        }

        [Test]
        // pseudo-performance test
        public void Invoke_ForLongWrongVector_ZZZ()
        {
            var findWordMatchesFunc = new FindWordMatchesFunc(
                isLegitWordFunc: new IsLegitEngWordFunc(),
                minWordLength: 4);

            var chars = new[] { 'C', 'T', 'C', 'T', 'C', 'T', 'C', 'T', 'C' };

            var matches = findWordMatchesFunc.Invoke(
                chars.SelectMany((_, y) => chars.Select((@char, x) => new StaticChar(x, y, @char))));

            Assert.IsEmpty(matches);
        }
    }
}
