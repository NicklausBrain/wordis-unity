using Assets.Wordis.BlockPuzzle.GameCore.Objects;
using NUnit.Framework;

namespace Assets.Wordis.BlockPuzzle.GameCore.Tests
{
    /// <summary>
    /// Tests for <see cref="WordisGame"/> concerning its saving and restoration.
    /// </summary>
    public class GameRestorationTests
    {
        private readonly WordisSettings _settings = new WordisSettings(3, 3);

        [Test]
        public void ToJson_SavesGameStateAsString()
        {
            var game = new WordisGame(_settings.With(width: 3, height: 4))
                .With(new ActiveChar(1, 0, 'R'))
                .With(new StaticChar(1, 2, 'A'))
                .With(new StaticChar(1, 3, 'T'))
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step);

            var gameAsJson = WordisGame.ToJson(game);

            Assert.IsNotEmpty(gameAsJson);
        }

        [Test]
        public void FromJson_RestoresGameState()
        {
            var game = new WordisGame(_settings.With(width: 3, height: 4))
                .With(new ActiveChar(1, 0, 'R'))
                .With(new StaticChar(1, 2, 'A'))
                .With(new StaticChar(1, 3, 'T'))
                .Handle(GameEvent.Step)
                .Handle(GameEvent.Step);

            var gameAsJson = WordisGame.ToJson(game);
            var restoredGame = WordisGame.FromJson(gameAsJson);

            Assert.IsNotNull(restoredGame);
            Assert.IsNotEmpty(restoredGame.GameEvents);
            Assert.IsNotEmpty(restoredGame.GameObjects);
        }
    }
}
