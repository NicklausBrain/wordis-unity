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
             * [-] [A] [-]
             * [-] [-] [-]
             * [-] [-] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 3))
                .With(new ActiveChar(1, 0, 'A'))
                .Handle(GameEvent.Step);

            /* Expected state:
             * [-] [-] [-]
             * [-] [A] [-]
             * [-] [-] [-] */
            Assert.AreEqual(
                new ActiveChar(1, 1, 'A'),
                game.GameObjects.Single());
        }

        [Test]
        public void HandleStep_ForActiveCharOnBottom_MakesItStatic()
        {
            /* Initial state:
             * [-] [A] [-]
             * [-] [-] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 2))
                .With(new ActiveChar(1, 0, 'A'))
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step);

            /* Expected state:
             * [-] [-] [-]
             * [-] [A] [-] */
            Assert.Contains(
                new StaticChar(1, 1, 'A'),
                game.GameObjects.ToArray());
        }

        [Test]
        public void HandleStep_ForActiveCharOnObstacle_MakesItStatic()
        {
            /* Initial state:
             * [-] [A] [-]
             * [-] [-] [-]
             * [-] [X] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 2))
                .With(new ActiveChar(1, 0, 'A'))
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step);

            /* Expected state:
             * [-] [-] [-]
             * [-] [A] [-]
             * [-] [X] [-] */
            Assert.Contains(
                new StaticChar(1, 1, 'A'),
                game.GameObjects.ToArray());
        }

        [Test]
        public void HandleLeft_ForActiveChar_MovesItLeft()
        {
            /* Initial state:
             * [-] [A] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 2))
                .With(new ActiveChar(1, 0, 'A'))
                .Handle(GameEvent.Left);

            /* Expected state:
             * [A] [-] [-] */
            Assert.AreEqual(
                new ActiveChar(0, 0, 'A'),
                game.GameObjects.Single());
        }

        [Test]
        public void HandleLeft_WhenActiveCharReachedTheEdge_DoesNotMoveIt()
        {
            /* Initial state:
             * [A] [-] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 2))
                .With(new ActiveChar(0, 0, 'A'))
                .Handle(GameEvent.Left);

            /* Expected state:
             * [A] [-] [-] */
            Assert.AreEqual(
                new ActiveChar(0, 0, 'A'),
                game.GameObjects.Single());
        }

        [Test]
        public void HandleLeft_WhenActiveCharHasMultipleObstacles_ItStopsBeforeTheFirst()
        {
            /* Initial state:
             * [Y] [X] [A] [-] [-] */
            var game = new WordisGame(_settings.With(width: 5, height: 1))
                .With(new ActiveChar(2, 0, 'A'))
                .With(new ActiveChar(1, 0, 'X'))
                .With(new ActiveChar(0, 0, 'Y')) // order can be random (here Y is comes last, but actually it is first)
                .Handle(GameEvent.Left);

            /* Expected state:
             * [Y] [X] [A] [-] [-] */
            Assert.Contains(
                new ActiveChar(2, 0, 'A'),
                game.GameObjects.ToArray());
            Assert.Contains(
                new ActiveChar(1, 0, 'X'),
                game.GameObjects.ToArray());
            Assert.Contains(
                new ActiveChar(0, 0, 'Y'),
                game.GameObjects.ToArray());
        }

        [Test]
        public void HandleRight_ForActiveChar_MovesItRight()
        {
            /* Initial state:
             * [-] [A] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 2))
                .With(new ActiveChar(1, 0, 'A'))
                .Handle(GameEvent.Right);

            /* Expected state:
             * [-] [-] [A] */
            Assert.AreEqual(
                new ActiveChar(2, 0, 'A'),
                game.GameObjects.Single());
        }

        [Test]
        public void HandleRight_WhenActiveCharReachedTheEdge_DoesNotMoveIt()
        {
            /* Initial state:
             * [-] [-] [A] */
            var game = new WordisGame(_settings.With(width: 3, height: 2))
                .With(new ActiveChar(2, 0, 'A'))
                .Handle(GameEvent.Right);

            /* Expected state:
             * [-] [-] [A] */
            Assert.AreEqual(
                new ActiveChar(2, 0, 'A'),
                game.GameObjects.Single());
        }

        [Test]
        public void HandleRight_WhenActiveCharHasMultipleObstacles_ItStopsBeforeTheFirst()
        {
            /* Initial state:
             * [-] [-] [A] [X] [Y] */
            var game = new WordisGame(_settings.With(width: 5, height: 1))
                .With(new ActiveChar(2, 0, 'A'))
                .With(new ActiveChar(4, 0, 'Y')) // order can be random (here Y is comes first, but actually it is last)
                .With(new ActiveChar(3, 0, 'X'))
                .Handle(GameEvent.Right);

            /* Expected state:
             * [-] [-] [A] [X] [Y] */
            Assert.Contains(
                new ActiveChar(2, 0, 'A'),
                game.GameObjects.ToArray());
            Assert.Contains(
                new ActiveChar(3, 0, 'X'),
                game.GameObjects.ToArray());
            Assert.Contains(
                new ActiveChar(4, 0, 'Y'),
                game.GameObjects.ToArray());
        }

        [Test]
        public void HandleDown_WhenActiveCharHasNoObstacles_ItDropsToTheBottom()
        {
            /* Initial state:
             * [-] [A] [-]
             * [-] [-] [-]
             * [-] [-] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 3))
                .With(new ActiveChar(1, 0, 'A'))
                .Handle(GameEvent.Down);

            /* Expected state:
             * [-] [-] [-]
             * [-] [-] [-]
             * [-] [A] [-] */
            Assert.AreEqual(
                new ActiveChar(1, 2, 'A'),
                game.GameObjects.Single());
        }

        [Test]
        public void HandleDown_WhenActiveCharHasObstacle_ItStopsBeforeIt()
        {
            /* Initial state:
             * [-] [A] [-]
             * [-] [-] [-]
             * [-] [X] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 3))
                .With(new ActiveChar(1, 0, 'A'))
                .With(new ActiveChar(1, 2, 'X'))
                .Handle(GameEvent.Down);

            /* Expected state:
             * [-] [-] [-]
             * [-] [A] [-]
             * [-] [X] [-] */
            Assert.Contains(
                new ActiveChar(1, 1, 'A'),
                game.GameObjects.ToArray());
            Assert.Contains(
                new ActiveChar(1, 2, 'X'),
                game.GameObjects.ToArray());
        }

        [Test]
        public void HandleDown_WhenActiveCharHasMultipleObstacles_ItStopsBeforeTheFirst()
        {
            /* Initial state:
             * [-] [A] [-]
             * [-] [-] [-]
             * [-] [X] [-]
             * [-] [Y] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 4))
                .With(new ActiveChar(1, 0, 'A'))
                .With(new ActiveChar(1, 3, 'Y')) // order can be random (here Y is comes first, but actually it is last)
                .With(new ActiveChar(1, 2, 'X'))
                .Handle(GameEvent.Down);

            /* Expected state:
             * [-] [-] [-]
             * [-] [A] [-]
             * [-] [X] [-]
             * [-] [Y] [-]*/
            Assert.Contains(
                new ActiveChar(1, 1, 'A'),
                game.GameObjects.ToArray());
            Assert.Contains(
                new ActiveChar(1, 2, 'X'),
                game.GameObjects.ToArray());
            Assert.Contains(
                new ActiveChar(1, 3, 'Y'),
                game.GameObjects.ToArray());
        }

        [Test]
        public void GameStep_OnWordMatch_EliminatesMatchedChars()
        {
            /* Initial state:
             * [-] [R] [-]
             * [-] [-] [-]
             * [-] [A] [-]
             * [-] [T] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 4))
                .With(new ActiveChar(1, 0, 'R'))
                .With(new StaticChar(1, 2, 'A'))
                .With(new StaticChar(1, 3, 'T'))
                .Handle(GameEvent.Step)  // move char down
                .Handle(GameEvent.Step); // count RAT as match

            /* Expected state:
             * [-] [-] [-]
             * [-] [-] [-]
             * [-] [-] [-]
             * [-] [-] [-] */

            Assert.IsNull(game.GameObjects
                .FirstOrDefault(o => o.X == 1 && o.Y == 1)); // R
            Assert.IsNull(game.GameObjects
                .FirstOrDefault(o => o.X == 1 && o.Y == 2)); // A
            Assert.IsNull(game.GameObjects
                .FirstOrDefault(o => o.X == 1 && o.Y == 3)); // T
        }

        [Test]
        public void GameStep_OnWordMatch_RecordsMatch()
        {
            /* Initial state:
             * [-] [R] [-]
             * [-] [-] [-]
             * [-] [A] [-]
             * [-] [T] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 4))
                .With(new ActiveChar(1, 0, 'R'))
                .With(new StaticChar(1, 2, 'A'))
                .With(new StaticChar(1, 3, 'T'))
                .Handle(GameEvent.Step)  // move char down
                .Handle(GameEvent.Step); // count RAT as match

            Assert.AreEqual("RAT", game.AllMatches.First().Word);
            Assert.AreEqual("RAT", game.LastStepMatches.First().Word);
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

        [Test]
        public void StaticBlocks_WhenHaveEmptySpaceBelow_FallDown()
        {
            /* Initial state:
             * [-] [-] [X]
             * [-] [Z] [Y]
             * [-] [-] [-] */
            var game = new WordisGame(_settings.With(width: 3, height: 3))
                .With(new StaticChar(2, 0, 'X'))
                .With(new StaticChar(2, 1, 'Y'))
                .With(new StaticChar(1, 1, 'Z'))
                .Handle(GameEvent.Step);

            /* Expected state:
             * [-] [-] [-]
             * [-] [-] [X]
             * [-] [Z] [Y] */
            Assert.Contains(
                new StaticChar(2, 1, 'X'),
                game.GameObjects.ToArray());
            Assert.Contains(
                new StaticChar(2, 2, 'Y'),
                game.GameObjects.ToArray());
            Assert.Contains(
                new StaticChar(1, 2, 'Z'),
                game.GameObjects.ToArray());
        }

        [Test]
        public void IsGameOver_WhenStartPointIsOccupied_ReturnsTrue()
        {
            var game = new WordisGame(_settings.With(width: 3, height: 3));

            var finishedGame = game.With(
                new StaticChar(game.StartPoint.x, game.StartPoint.y, 'A'));

            Assert.IsTrue(finishedGame.IsGameOver);
        }

        [Test]
        public void IsGameOver_WhenStartPointIsSpare_ReturnsFalse()
        {
            var game = new WordisGame(_settings.With(width: 3, height: 3))
                .Handle(GameEvent.Step);

            Assert.IsFalse(game.IsGameOver);
        }

        [Test]
        public void Handle_WhenGameIsOver_ReturnsTheSameGameInstance()
        {
            /* [-][A][-] game is over
             * [-][B][-] */
            var game = new WordisGame(_settings.With(width: 3, height: 2))
                .With(new StaticChar(1, 0, 'A'))
                .With(new StaticChar(1, 1, 'B'));

            var finishedGame = game
                .Handle(GameEvent.Left)
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Down)
                .Handle(GameEvent.Right)
                .Handle(GameEvent.None);

            Assert.AreSame(game, finishedGame);
        }

        [Test]
        public void Step_WhenLeadsToGameOver_DoesNotProduceNewActiveObject()
        {
            var game = new WordisGame(_settings.With(width: 3, height: 2));

            /* [-][A][-] A is active and next step should finish the game
             * [-][B][-] */
            var finishedGame = game
                .With(new ActiveChar(game.StartPoint.x, game.StartPoint.y, 'A'))
                .With(new StaticChar(game.StartPoint.x, game.StartPoint.y + 1, 'B'))
                .Handle(GameEvent.Step);

            Assert.IsTrue(finishedGame.IsGameOver);
            Assert.IsFalse(finishedGame.GameObjects.Any(o => o is ActiveChar));
        }
    }
}
