using gnuciDictionary;

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
            var definition = EnglishDictionary.Define(word);

            return definition != null;
        }
    }
}
