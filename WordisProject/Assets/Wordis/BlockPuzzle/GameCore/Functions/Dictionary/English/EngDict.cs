using System;
using System.Collections.Generic;

namespace Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words definitions.
    /// </summary>
    public class EngDict : DictionaryBase
    {
        private static readonly IReadOnlyDictionary<char, DictionaryBase> EngDictionaries =
            new Dictionary<char, DictionaryBase>
            {
                { 'A', new EngDictA() },
                { 'B', new EngDictB() },
                { 'C', new EngDictC() },
                { 'D', new EngDictD() },
                { 'E', new EngDictE() },
                { 'F', new EngDictF() },
                { 'G', new EngDictG() },
                { 'H', new EngDictH() },
                { 'I', new EngDictI() },
                { 'J', new EngDictJ() },
                { 'K', new EngDictK() },
                { 'L', new EngDictL() },
                { 'M', new EngDictM() },
                { 'N', new EngDictN() },
                { 'O', new EngDictO() },
                { 'P', new EngDictP() },
                { 'Q', new EngDictQ() },
                { 'R', new EngDictR() },
                { 'S', new EngDictS() },
                { 'T', new EngDictT() },
                { 'U', new EngDictU() },
                { 'V', new EngDictV() },
                { 'W', new EngDictW() },
                { 'X', new EngDictX() },
                { 'Y', new EngDictY() },
                { 'Z', new EngDictZ() },
            };

        /// <inheritdoc />
        public override IReadOnlyList<WordDefinition> Define(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return Array.Empty<WordDefinition>();
            }

            var trimmedWord = word.Trim(); // remove empty chars

            var firstChar = char.ToUpperInvariant(trimmedWord[0]);

            if (char.IsLetter(firstChar)) // avoid special characters
            {
                return EngDictionaries[firstChar].Define(trimmedWord);
            }

            return Array.Empty<WordDefinition>();
        }

        protected override string WordsInCsv => string.Empty;

        /// <summary>
        /// Preload all the words to speedup lookup function.
        /// </summary>
        public void WarmUp()
        {
            foreach (var letter in EngDictionaries.Keys)
            {
                EngDictionaries[letter].Define($"{letter}");
            }
        }
    }
}