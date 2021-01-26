using System.Collections.Generic;
using Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary;
using Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary.English;

namespace Assets.Wordis.BlockPuzzle.GameCore.Functions
{
    /// <summary>
    /// Gets english word definitions
    /// </summary>
    public class DefineEngWordFunc : DefineWordFunc
    {
        public static readonly EngDict EngDict = new EngDict();

        static DefineEngWordFunc()
        {
            EngDict.WarmUp();
        }

        /// <inheritdoc />
        public override IReadOnlyList<WordDefinition> Invoke(string word)
        {
            var definitions = EngDict.Define(word);

            return definitions;
        }
    }
}
