using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;
using NUnit.Framework;

namespace Assets.Wordis.BlockPuzzle.GameCore.Tests
{
    /// <summary>
    /// Tests for <see cref="WordisGame"/> with water field enabled.
    /// </summary>
    public class WordisGameWaterTests
    {
        private readonly WordisSettings _settings = new WordisSettings(3, 3);

        [Test]
        public void HandleStep_ForActiveCharOnWater_MakesItStatic()
        {
            /* Initial state:
             * [-] [A] [-]
             * [-] [-] [-]
             * [-]-[-]-[-] -- water -- */
            var game = new WordisGame(
                    _settings.With(width: 3, height: 3, waterLevel: 1))
                .With(new ActiveChar(1, 0, 'A'))
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step);

            /* Expected state:
             * [-] [-] [-]
             * [-] [A] [-]
             * [-]-[-]-[-] -- water -- */
            Assert.Contains(
                new StaticChar(1, 1, 'A'),
                game.GameObjects.ToArray());
        }

        [Test]
        public void HandleDown_WhenActiveCharHasNoObstacles_ItStopsOnWater()
        {
            /* Initial state:
             * [-] [A] [-]
             * [-] [-] [-]
             * [-]-[-]-[-] -- water -- */
            var game = new WordisGame(
                    _settings.With(width: 3, height: 3, waterLevel: 1))
                .With(new ActiveChar(1, 0, 'A'))
                .Handle(GameEvent.Down);

            /* Expected state:
             * [-] [-] [-]
             * [-] [A] [-]
             * [-]-[-]-[-] -- water -- */
            Assert.AreEqual(
                new ActiveChar(1, 1, 'A'),
                game.GameObjects.Single());
        }

        [Test]
        public void StaticBlocks_WhenInWater_RiseUp()
        {
            /* Initial state:
             * [-] [-] [-]
             * [-]-[-]-[X] -- water --
             * [-]-[Z]-[Y] -- water -- */
            var game = new WordisGame(
                    _settings.With(width: 3, height: 3, waterLevel: 2))
                .With(new StaticChar(2, 1, 'X'))
                .With(new StaticChar(2, 2, 'Y'))
                .With(new StaticChar(1, 2, 'Z'))
                .Handle(GameEvent.Step);

            /* Expected state:
             * [-] [-] [X]
             * [-]-[Z]-[Y] -- water --
             * [-]-[-]-[-] -- water -- */
            Assert.Contains(
                new StaticChar(2, 0, 'X'),
                game.GameObjects.ToArray());
            Assert.Contains(
                new StaticChar(2, 1, 'Y'),
                game.GameObjects.ToArray());
            Assert.Contains(
                new StaticChar(1, 1, 'Z'),
                game.GameObjects.ToArray());
        }
    }
}
