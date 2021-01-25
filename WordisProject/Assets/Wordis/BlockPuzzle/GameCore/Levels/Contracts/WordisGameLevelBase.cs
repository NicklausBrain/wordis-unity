using System;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts
{
    /// <summary>
    /// Provides default behavior for the game level.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class WordisGameLevelBase<T>
        where T : WordisGameLevelBase<T>, IWordisGameLevel, new()
    {
        protected WordisGameLevelBase(WordisGame game)
        {
            Game = game;
        }

        /// <inheritdoc cref="IWordisGameLevel" />
        public abstract string Title { get; }

        /// <inheritdoc cref="IWordisGameLevel" />
        public abstract string Goal { get; }

        /// <summary>
        /// Returns a new instance of level with the update game state.
        /// </summary>
        /// <param name="updatedGame">Updated game state.</param>
        /// <returns><see cref="WordisGameLevelBase{T}"/>.</returns>
        public abstract T WithUpdatedGame(WordisGame updatedGame);

        /// <inheritdoc cref="IWordisGameLevel" />
        public virtual WordisGame Game { get; }

        /// <inheritdoc cref="IWordisGameLevel" />
        public virtual WordisSettings Settings => Game.Settings;

        /// <inheritdoc cref="IWordisGameLevel" />
        public virtual bool IsCompleted => false;

        /// <inheritdoc cref="IWordisGameLevel" />
        public virtual bool IsFailed => Game.IsGameOver;

        /// <inheritdoc cref="IWordisGameLevel" />
        public virtual IWordisGameLevel Handle(GameEvent gameEvent)
        {
            var updatedGame = Game.Handle(gameEvent);

            return WithUpdatedGame(updatedGame);
        }

        /// <inheritdoc cref="IWordisGameLevel" />
        public virtual IWordisGameLevel Reset() => new T();

        /// <inheritdoc cref="IWordisGameLevel" />
        public virtual IWordisGameLevel WithOutput(Action<string> outFunc) =>
            (IWordisGameLevel)this;
    }
}
