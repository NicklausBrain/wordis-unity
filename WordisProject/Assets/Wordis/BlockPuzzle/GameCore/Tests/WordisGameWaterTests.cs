using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;
using Assets.Wordis.BlockPuzzle.GameCore.Words;
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

        [Test]
        public void HandleStep_WhenColumnWithWaterIsNotFull_TheGameIsNotOver()
        {
            /* Initial state:
             * [-] [-] [-]
             * [-] [-] [-]
             * [-]-[-]-[-] -- water --
             * [-]-[-]-[-] -- water -- */
            var game = new WordisGame(_settings.With(width: 3, height: 4, waterLevel: 2, minWordLength: int.MaxValue));
            game = game.Handle(GameEvent.Step);
            game = game.Handle(GameEvent.Step);
            game = game.Handle(GameEvent.Step);
            game = game.Handle(GameEvent.Step);

            /* Initial state:
             * [-] [-] [-]
             * [-] [*] [-]
             * [-]-[*]-[-] -- water --
             * [-]-[-]-[-] -- water -- */

            Assert.IsFalse(game.IsGameOver);
            // todo add coordinates assertion
        }

        [Test]
        public void HandleStep_OnCollision_LiftsActiveCharAndMakeItStatic()
        {
            /* Initial state:
             * [-] [-] [-] [-]
             * [-] [-] [A] [-]
             * [-]-[-]-[X]-[-] -- water --
             * [-]-[Z]-[Y]-[-] -- water -- */
            var game = new WordisGame(
                    _settings.With(width: 4, height: 4, waterLevel: 2),
                    new WordLetters("*"))
                .With(new StaticChar(2, 2, 'X'))
                .With(new StaticChar(2, 3, 'Y'))
                .With(new StaticChar(1, 3, 'Z'))
                .With(new ActiveChar(2, 1, 'A'))
                .Handle(GameEvent.Step);

            /* Expected state:
             * [-] [-] [A] [-]
             * [-] [-] [X] [-]
             * [-]-[Z]-[Y]-[-] -- water --
             * [-]-[-]-[-] [-] -- water -- */

            var gameObjects = game.GameObjects.ToArray();
            Assert.Contains(
                new StaticChar(2, 1, 'X'), gameObjects);
            Assert.Contains(
                new StaticChar(2, 2, 'Y'), gameObjects);
            Assert.Contains(
                new StaticChar(1, 2, 'Z'), gameObjects);
            Assert.Contains(
                new StaticChar(2, 0, 'A'), gameObjects);
        }

        [Test]
        public void HandleStep_AfterCollision_DrownsTheColumn()
        {
            /* Initial state:
             * [-] [-] [-] [-]
             * [-] [-] [A] [-]
             * [-]-[-]-[X]-[-] -- water --
             * [-]-[Z]-[Y]-[-] -- water -- */
            var game = new WordisGame(
                    _settings.With(width: 4, height: 4, waterLevel: 2),
                    new WordLetters("*"))
                .With(new StaticChar(2, 2, 'X'))
                .With(new StaticChar(2, 3, 'Y'))
                .With(new StaticChar(1, 3, 'Z'))
                .With(new ActiveChar(2, 1, 'A'))
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step);

            /* Expected state:
             * [-] [-] [*] [-]
             * [-] [Z] [A] [-]
             * [-]-[-]-[X]-[-] -- water --
             * [-]-[-]-[Y] [-] -- water -- */

            var gameObjects = game.GameObjects.ToArray();
            Assert.Contains(
                new StaticChar(2, 2, 'X'), gameObjects);
            Assert.Contains(
                new StaticChar(2, 3, 'Y'), gameObjects);
            Assert.Contains(
                new StaticChar(1, 1, 'Z'), gameObjects);
            Assert.Contains(
                new StaticChar(2, 1, 'A'), gameObjects);
        }

        [Test]
        public void HandleStep_OnDoubleCollision_ResolvesIt()
        {
            /* Initial state:
             * [-] [-] [-] [-]
             * [-] [Q] [-] [-] - static
             * [-] [A] [-] [-] - active
             * [-]-[X]-[-] [-]-- water --
             * [Z]-[Y]-[-] [-]-- water -- */
            var game = new WordisGame(
                    _settings.With(width: 4, height: 5, waterLevel: 2),
                    new WordLetters("*"))
                .With(new StaticChar(1, 1, 'Q'))
                .With(new ActiveChar(1, 2, 'A'))
                .With(new StaticChar(1, 3, 'X'))
                .With(new StaticChar(1, 4, 'Y'))
                .With(new StaticChar(0, 4, 'Z'))
                .Handle(GameEvent.Step);

            /* Expected state:
             * [-] [-] [-] [-]
             * [-] [Q] [-] [-] - static
             * [-] [A] [-] [-] - active
             * [Z]-[X]-[-] [-]-- water --
             * [-]-[Y]-[-] [-]-- water -- */

            var gameObjects = game.GameObjects.ToArray();
            Assert.IsNull(
                   game.Matrix[0, 1]);
            Assert.Contains(
                new StaticChar(1, 1, 'Q'), gameObjects);
            Assert.Contains(
                new StaticChar(1, 2, 'A'), gameObjects);
            Assert.Contains(
                new StaticChar(1, 3, 'X'), gameObjects);
            Assert.Contains(
                new StaticChar(1, 4, 'Y'), gameObjects);
            Assert.Contains(
                new StaticChar(0, 3, 'Z'), gameObjects);
        }
    }
}
