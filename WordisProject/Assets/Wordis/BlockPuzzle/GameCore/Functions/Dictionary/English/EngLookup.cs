using System.Collections.Generic;

namespace Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary.English
{
    /// <summary>
    /// English words lookup.
    /// </summary>
    public class EngLookup : WordLookupBase
    {
        private static readonly IReadOnlyDictionary<char, WordLookupBase> EngLookups =
            new Dictionary<char, WordLookupBase>
            {
                { 'A', new EngLookupA() },
                { 'B', new EngLookupB() },
                { 'C', new EngLookupC() },
                { 'D', new EngLookupD() },
                { 'E', new EngLookupE() },
                { 'F', new EngLookupF() },
                { 'G', new EngLookupG() },
                { 'H', new EngLookupH() },
                { 'I', new EngLookupI() },
                { 'J', new EngLookupJ() },
                { 'K', new EngLookupK() },
                { 'L', new EngLookupL() },
                { 'M', new EngLookupM() },
                { 'N', new EngLookupN() },
                { 'O', new EngLookupO() },
                { 'P', new EngLookupP() },
                { 'Q', new EngLookupQ() },
                { 'R', new EngLookupR() },
                { 'S', new EngLookupS() },
                { 'T', new EngLookupT() },
                { 'U', new EngLookupU() },
                { 'V', new EngLookupV() },
                { 'W', new EngLookupW() },
                { 'X', new EngLookupX() },
                { 'Y', new EngLookupY() },
                { 'Z', new EngLookupZ() },
            };

        /// <inheritdoc />
        public override bool Check(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return false;
            }

            var firstChar = char.ToUpperInvariant(word[0]);

            if (char.IsLetter(firstChar))
            {
                return EngLookups[firstChar].Check(word);
            }

            return false;
        }

        /// <inheritdoc />
        protected override string WordsInCsv => string.Empty;

        /// <summary>
        /// Preload all the words to speedup lookup function.
        /// </summary>
        public void WarmUp()
        {
            foreach (var letter in EngLookups.Keys)
            {
                EngLookups[letter].Check($"{letter}");
            }
        }
    }
}
