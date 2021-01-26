using System.Collections.Generic;
using Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary;

namespace Assets.Wordis.BlockPuzzle.GameCore.Functions
{
    /// <summary>
    /// Gets definitions for the given word.
    /// </summary>
    public abstract class DefineWordFunc
    {
        /// <summary>
        /// Invokes the function.
        /// </summary>
        /// <param name="word">Target word.</param>
        /// <returns>Definitions of the word.</returns>
        public abstract IReadOnlyList<WordDefinition> Invoke(string word);
    }
}
