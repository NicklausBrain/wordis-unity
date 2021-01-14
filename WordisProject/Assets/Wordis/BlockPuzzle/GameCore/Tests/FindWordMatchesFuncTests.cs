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
        // [F][R][E][E][D][O][M]
        // [K][I][S][S][I][L][L][Y]

        // [K][I][S][-][I][L][L][Y]
        // 4 min word len

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

        // simple column match

        [Test]
        public void Invoke_WhenLongerWordCanBeMatched_TakesIt()
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

        // two in a column

        // row - column intersection

        // no match
    }
}
