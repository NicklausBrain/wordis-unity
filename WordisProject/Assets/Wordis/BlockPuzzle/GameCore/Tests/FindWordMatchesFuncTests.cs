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

        // simple row match

        // simple column match

        [Test]
        public void Invoke_WhenLongerWordCanBeMatched_TakesIt()
        {
            var findWordMatchesFunc = new FindWordMatchesFunc(
                isLegitWordFunc: new IsLegitEngWordFunc(),
                minWordLength: 4);

            // [-][-][-][E][-][-][-]
            // [F][I][R][-][F][L][Y]
            // ---------------------
            // [F][I][R][E][F][L][Y] - longer world should count
            var chars = new[] { 'F', 'I', 'R', 'E', 'F', 'L', 'Y' };

            var matches = findWordMatchesFunc.Invoke(
                chars.Select((@char, x) => new StaticChar(x, 0, @char)));

            Assert.AreEqual(new WordMatch(new[]
            {
                new StaticChar(0, 0, 'F'),
                new StaticChar(1, 0, 'I'),
                new StaticChar(2, 0, 'R'),
                new StaticChar(3, 0, 'E'),
                new StaticChar(4, 0, 'F'),
                new StaticChar(5, 0, 'L'),
                new StaticChar(6, 0, 'Y'),
            }), matches.Single());
        }

        [Test]
        public void Invoke_WhenTwoWordMatchInARow_AcceptsBoth()
        {
            var findWordMatchesFunc = new FindWordMatchesFunc(
                isLegitWordFunc: new IsLegitEngWordFunc(),
                minWordLength: 4);
            // [-][-][-][K][-][-][-]
            // [S][I][C][-][I][L][L] - one end - another start
            // ---------------------
            // [S][I][C][K][I][L][L] - both should match
            var chars = new[] { 'S', 'I', 'C', 'K', 'I', 'L', 'L' };

            var matches = findWordMatchesFunc.Invoke(
                chars.Select((@char, x) => new StaticChar(x, 0, @char)));

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(0, 0, 'S'),
                new StaticChar(1, 0, 'I'),
                new StaticChar(2, 0, 'C'),
                new StaticChar(3, 0, 'K'),
            }), matches);

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(3, 0, 'K'),
                new StaticChar(4, 0, 'I'),
                new StaticChar(5, 0, 'L'),
                new StaticChar(6, 0, 'L'),
            }), matches);
        }

        // two in a column

        // row - column intersection

        // no match
    }
}
