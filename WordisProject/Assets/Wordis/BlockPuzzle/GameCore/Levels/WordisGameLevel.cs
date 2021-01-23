using Assets.Wordis.BlockPuzzle.GameCore.Functions;
using Assets.Wordis.BlockPuzzle.GameCore.Words;

namespace Assets.Wordis.BlockPuzzle.GameCore.Levels
{
    /// <summary>
    /// General contract of the game level.
    /// TODO: should it be covariant interface (out)?
    /// </summary>
    public interface IWordisGameLevel<out T> where T : IWordisGameLevel<T>
    {
        //protected WordisGameLevel(
        //    WordisSettings settings,
        //    LetterSource letterSource = null,
        //    FindWordMatchesFunc findWordMatchesFunc = null)
        //{
        //    Game = new WordisGame(
        //        settings: settings,
        //        letterSource: letterSource,
        //        findWordMatchesFunc: findWordMatchesFunc);
        //}

        WordisGame Game { get; }

        string Title { get; }

        string Goal { get; }

        bool IsCompleted { get; }

        T Handle(GameEvent gameEvent);
    }
}
