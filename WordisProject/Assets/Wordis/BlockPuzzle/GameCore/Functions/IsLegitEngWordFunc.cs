using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary;
using Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary.English;
using WeCantSpell.Hunspell;

namespace Assets.Wordis.BlockPuzzle.GameCore.Functions
{
    /// <summary>
    /// Determines if a word is legit in English.
    /// </summary>
    public class IsLegitEngWordFunc : IsLegitWordFunc
    {
        private static readonly WordList aLookUp = WordList.CreateFromWords(new[] { "car", "cat", "cart", "art" });

        /// <inheritdoc />
        public override bool Invoke(string word)
        {
            //var isLegitWord = EngLookup.Contains(word);
            return aLookUp.Check(word);
        }

        private static WordList BuildId(int i)
        {
            return WordList.CreateFromWords(
                Enumerable
                    .Range(0, i)
                    .Select(q => "cat" + q)
                    .ToArray());
        }
    }
}
