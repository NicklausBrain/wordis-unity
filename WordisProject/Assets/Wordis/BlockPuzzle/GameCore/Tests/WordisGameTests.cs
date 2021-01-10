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
        private readonly WordisSettings _settings = new WordisSettings(3, 3);

        [Test]
        public void HandleStep_ForActiveChar_MovesItDown()
        {
            /* Initial state:
             * [-] [W] [-]
             * [-] [-] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 2))
                .With(new ActiveChar(1, 0, 'W'))
                .Handle(GameEvent.Step);

            /* Expected state:
             * [-] [-] [-]
             * [-] [W] [-] */
            Assert.AreEqual(
                new ActiveChar(1, 1, 'W'),
                game.GameObjects.Single());
        }

        [Test]
        public void HandleLeft_ForActiveChar_MovesItLeft()
        {
            /* Initial state:
             * [-] [W] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 2))
                .With(new ActiveChar(1, 0, 'W'))
                .Handle(GameEvent.Left);

            /* Expected state:
             * [W] [-] [-] */
            Assert.AreEqual(
                new ActiveChar(0, 0, 'W'),
                game.GameObjects.Single());
        }

        [Test]
        public void HandleLeft_WhenActiveCharReachedTheEdge_DoesNotMoveIt()
        {
            /* Initial state:
             * [W] [-] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 2))
                .With(new ActiveChar(0, 0, 'W'))
                .Handle(GameEvent.Left);

            /* Expected state:
             * [W] [-] [-] */
            Assert.AreEqual(
                new ActiveChar(0, 0, 'W'),
                game.GameObjects.Single());
        }

        [Test]
        public void HandleRight_ForActiveChar_MovesItRight()
        {
            /* Initial state:
             * [-] [W] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 2))
                .With(new ActiveChar(1, 0, 'W'))
                .Handle(GameEvent.Right);

            /* Expected state:
             * [-] [-] [W] */
            Assert.AreEqual(
                new ActiveChar(2, 0, 'W'),
                game.GameObjects.Single());
        }

        [Test]
        public void HandleRight_WhenActiveCharReachedTheEdge_DoesNotMoveIt()
        {
            /* Initial state:
             * [-] [-] [W] */
            var game = new WordisGame(_settings.With(width: 3, height: 2))
                .With(new ActiveChar(2, 0, 'W'))
                .Handle(GameEvent.Right);

            /* Expected state:
             * [-] [-] [W] */
            Assert.AreEqual(
                new ActiveChar(2, 0, 'W'),
                game.GameObjects.Single());
        }

        [Test]
        public void HandleDown_WhenActiveCharHasNoObstacles_ItDropsToTheBottom()
        {
            /* Initial state:
             * [-] [W] [-]
             * [-] [-] [-]
             * [-] [-] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 3))
                .With(new ActiveChar(1, 0, 'W'))
                .Handle(GameEvent.Down);

            /* Expected state:
             * [-] [-] [-]
             * [-] [-] [-]
             * [-] [W] [-] */
            Assert.AreEqual(
                new ActiveChar(1, 2, 'W'),
                game.GameObjects.Single());
        }

        [Test]
        public void HandleDown_WhenActiveCharHasObstacle_ItStopsBeforeIt()
        {
            /* Initial state:
             * [-] [W] [-]
             * [-] [-] [-]
             * [-] [X] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 3))
                .With(new ActiveChar(1, 0, 'W'))
                .With(new ActiveChar(1, 2, 'X'))
                .Handle(GameEvent.Down);

            /* Expected state:
             * [-] [-] [-]
             * [-] [W] [-]
             * [-] [X] [-] */
            Assert.Contains(
                new ActiveChar(1, 1, 'W'),
                game.GameObjects.ToArray());
            Assert.Contains(
                new ActiveChar(1, 2, 'X'),
                game.GameObjects.ToArray());
        }

        [Test]
        public void Settings_ReturnsPassedObject()
        {
            var settings = new WordisSettings(
                width: 5,
                height: 5);

            var game = new WordisGame(settings);

            Assert.AreSame(settings, game.Settings);
        }
    }
}
