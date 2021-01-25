using System;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels
{
    /// <summary>
    /// General contract of the game level.
    /// </summary>
    public interface IWordisGameLevel
    {
        /// <inheritdoc cref="WordisGame"/>
        WordisGame Game { get; }

        /// <inheritdoc cref="WordisSettings"/>
        WordisSettings Settings { get; }

        /// <summary>
        /// How the level is titled.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// The goal of the level.
        /// </summary>
        string Goal { get; }

        /// <summary>
        /// Is level successfully completed.
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Is level failed
        /// </summary>
        bool IsFailed { get; }

        /// <inheritdoc cref="WordisGame.Handle"/>
        IWordisGameLevel Handle(GameEvent gameEvent);

        /// <summary>
        /// Returns this level in default state.
        /// </summary>
        /// <returns><see cref="IWordisGameLevel "/>.</returns>
        IWordisGameLevel Reset();

        /// <summary>
        /// Sets the output system to be used.
        /// </summary>
        /// <returns><see cref="IWordisGameLevel "/>.</returns>
        IWordisGameLevel WithOutput(Action<string> outFunc);
    }

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
        public virtual WordisGame Game { get; }

        /// <inheritdoc cref="IWordisGameLevel" />
        public virtual WordisSettings Settings => Game.Settings;

        /// <inheritdoc cref="IWordisGameLevel" />
        public abstract string Title { get; }

        /// <inheritdoc cref="IWordisGameLevel" />
        public abstract string Goal { get; }

        /// <inheritdoc cref="IWordisGameLevel" />
        public virtual bool IsCompleted => false;

        /// <inheritdoc cref="IWordisGameLevel" />
        public virtual bool IsFailed => Game.IsGameOver;

        /// <inheritdoc cref="IWordisGameLevel" />
        public IWordisGameLevel Handle(GameEvent gameEvent)
        {
            var updatedGame = Game.Handle(gameEvent);

            return WithUpdatedGame(updatedGame);
        }

        /// <inheritdoc cref="IWordisGameLevel" />
        public IWordisGameLevel Reset() => new T();

        /// <inheritdoc cref="IWordisGameLevel" />
        public virtual IWordisGameLevel WithOutput(Action<string> outFunc) =>
            (IWordisGameLevel)this;

        /// <summary>
        /// Returns a new instance of level with the update game state.
        /// </summary>
        /// <param name="updatedGame">Updated game state.</param>
        /// <returns><see cref="WordisGameLevelBase{T}"/>.</returns>
        public abstract T WithUpdatedGame(WordisGame updatedGame);
    }
}
