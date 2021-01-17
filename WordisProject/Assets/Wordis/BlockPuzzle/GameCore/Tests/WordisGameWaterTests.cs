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
             * [-] [-] [-] */
            var game = new WordisGame(
                    _settings.With(width: 3, height: 3, waterLevel: 1))
                .With(new ActiveChar(1, 0, 'A'))
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step);

            /* Expected state:
             * [-] [-] [-]
             * [-] [A] [-]
             * [-] [-] [-] */
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
             * [-] [-] [-] */
            var game = new WordisGame(
                    _settings.With(width: 3, height: 3, waterLevel: 1))
                .With(new ActiveChar(1, 0, 'A'))
                .Handle(GameEvent.Down);

            /* Expected state:
             * [-] [-] [-]
             * [-] [A] [-]
             * [-] [-] [-] */
            Assert.AreEqual(
                new ActiveChar(1, 1, 'A'),
                game.GameObjects.Single());
        }
    }
}
