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
                chars.Select((@char, y) => new StaticChar(0, y, @char)));

            Assert.AreEqual(new WordMatch(new[]
            {
                new StaticChar(0, 0, 'F'),
                new StaticChar(0, 1, 'I'),
                new StaticChar(0, 2, 'R'),
                new StaticChar(0, 3, 'E'),
                new StaticChar(0, 3, 'F'),
                new StaticChar(0, 3, 'L'),
                new StaticChar(0, 3, 'Y'),
            }), matches.Single());
        }

        [Test]
        public void Invoke_WhenTwoWordMatchInARow_AcceptsBoth()
        {
            var findWordMatchesFunc = new FindWordMatchesFunc(
                isLegitWordFunc: new IsLegitEngWordFunc(),
                minWordLength: 4);
            // [-][-][-][K][-][-][-]
            // [S][I][C][-][I][L][L]
            // ---------------------
            // [S][I][C][K][I][L][L] - both should match
            var chars = new[] { 'S', 'I', 'C', 'K', 'K', 'I', 'L', 'L' };

            var matches = findWordMatchesFunc.Invoke(
                chars.Select((@char, y) => new StaticChar(0, y, @char)));

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(0, 0, 'S'),
                new StaticChar(0, 1, 'I'),
                new StaticChar(0, 2, 'C'),
                new StaticChar(0, 3, 'K'),
            }), matches);

            Assert.Contains(new WordMatch(new[]
            {
                new StaticChar(0, 3, 'K'),
                new StaticChar(0, 4, 'I'),
                new StaticChar(0, 5, 'L'),
                new StaticChar(0, 6, 'L'),
            }), matches);
        }

        // two in a column

        // row - column intersection

        // no match
    }
}
