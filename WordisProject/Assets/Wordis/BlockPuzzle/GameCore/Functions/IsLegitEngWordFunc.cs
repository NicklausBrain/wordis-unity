using Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary.English;

namespace Assets.Wordis.BlockPuzzle.GameCore.Functions
{
    /// <summary>
    /// Determines if a word is legit in English.
    /// </summary>
    public class IsLegitEngWordFunc : IsLegitWordFunc
    {
        /// <inheritdoc />
        public override bool Invoke(string word)
        {
            var isLegitWord = EngLookup.Contains(word);
            return isLegitWord;
        }
    }
}
