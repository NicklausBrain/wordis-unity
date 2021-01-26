using System;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels.Contracts
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
        /// Level progress data.
        /// </summary>
        string Progress { get;}

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
}
