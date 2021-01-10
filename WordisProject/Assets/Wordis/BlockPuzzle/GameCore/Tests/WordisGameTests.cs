using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;
using NUnit.Framework;

namespace Assets.Wordis.BlockPuzzle.GameCore.Tests
{
    /// <summary>
    /// Tests for <see cref="WordisGame"/>.
    /// </summary>
    public class WordisGameTests
    {
        [Test]
        public void Settings_ReturnsPassedObject()
        {
            var settings = new WordisSettings(
                width: 5,
                height: 5);

            var game = new WordisGame(settings);

            Assert.AreSame(settings, game.Settings);
        }

        [Test]
        public void HandleStep_ForActiveChar_MovesItDown()
        {
            var settings = new WordisSettings(
                width: 3,
                height: 2);

            /* Initial state:
             * [-] [W] [-]
             * [-] [-] [-] */

            var game =
                new WordisGame(settings)
                    .With(new ActiveChar(1, 1, 'W'))
                    .Handle(GameEvent.Step);

            /* Expected state:
             * [-] [-] [-]
             * [-] [W] [-] */

            Assert.AreEqual(
                new ActiveChar(1, 2, 'W'),
                game.GameObjects.Single());
        }
    }
}
