using Assets.Wordis.BlockPuzzle.GameCore.Objects;
using NUnit.Framework;

namespace Assets.Wordis.BlockPuzzle.GameCore.Tests
{
    /// <summary>
    /// Tests for <see cref="WordisMatrix"/>.
    /// </summary>
    public class WordisMatrixTests
    {
        private readonly WordisSettings _settings = new WordisSettings(3, 3);

        [Test]
        public void CoordinatesIndexer_IfObjectExists_ReturnsIt()
        {
            var staticChar = new StaticChar(1, 1, 'Q');

            var game = new WordisGame(_settings)
                .With(staticChar);

            Assert.AreEqual(staticChar, game.Matrix[1, 1]);
        }

        [Test]
        public void CoordinatesIndexer_IfObjectDoesNotExist_ReturnsNull()
        {
            var staticChar = new StaticChar(1, 1, 'Q');

            var game = new WordisGame(_settings)
                .With(staticChar);

            Assert.IsNull(game.Matrix[0, 0]);
        }

        [Test]
        public void CoordinatesIndexer_IfCoordinatesOutOfBound_ReturnsNull()
        {
            var staticChar = new StaticChar(1, 1, 'Q');

            var game = new WordisGame(_settings)
                .With(staticChar);

            Assert.IsNull(game.Matrix[-1, 10]);
        }

        [Test]
        public void PointIndexer_IsSameToCoordinatesIndexer()
        {
            var staticChar = new StaticChar(1, 1, 'Q');

            var game = new WordisGame(_settings)
                .With(staticChar);

            Assert.AreEqual(staticChar, game.Matrix[1, 1]); // coordinates
            Assert.AreEqual(staticChar, game.Matrix[(1, 1)]); // point object
        }

        [Test]
        public void MatrixInitialization_IfThereObjectOutOfBounds_DoesNotCountIt()
        {
            var game = new WordisGame(_settings.With(width: 3, height: 3))
                .With(new StaticChar(4, 4, 'Q')); // object out of bound for some unknown reason

            Assert.IsNull(game.Matrix[1, 1]); // try any call
        }
    }
}
