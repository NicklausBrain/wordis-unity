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
    }
}
