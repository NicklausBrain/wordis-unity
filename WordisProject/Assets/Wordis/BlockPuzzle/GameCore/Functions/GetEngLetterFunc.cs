using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Assets.Wordis.BlockPuzzle.GameCore.Functions
{
    /// <summary>
    /// Generates random english letters according to their frequency.
    /// </summary>
    public class GetEngLetterFunc : GetLetterFunc
    {
        /// <summary>
        /// See https://en.wikipedia.org/wiki/Letter_frequency
        /// </summary>
        private static readonly IReadOnlyDictionary<char, float> RelativeEngFrequency =
            new ReadOnlyDictionary<char, float>(new Dictionary<char, float>
            {
                { 'A', 0.0780f },
                { 'B', 0.0200f },
                { 'C', 0.0400f },
                { 'D', 0.0380f },
                { 'E', 0.1141f },
                { 'F', 0.0140f },
                { 'G', 0.0300f },
                { 'H', 0.0230f },
                { 'I', 0.0860f },
                { 'J', 0.0021f },
                { 'K', 0.0097f },
                { 'L', 0.0530f },
                { 'M', 0.0270f },
                { 'N', 0.0720f },
                { 'O', 0.0610f },
                { 'P', 0.0280f },
                { 'Q', 0.0019f },
                { 'R', 0.0730f },
                { 'S', 0.0870f },
                { 'T', 0.0670f },
                { 'U', 0.0330f },
                { 'V', 0.0100f },
                { 'W', 0.0091f },
                { 'X', 0.0027f },
                { 'Y', 0.0160f },
                { 'Z', 0.0044f },
            });

        /// <summary>
        /// See https://stackoverflow.com/questions/2149914/randomly-generate-letters-according-to-their-frequency-of-use
        /// </summary>
        private static readonly Lazy<IReadOnlyDictionary<char, float>> CummulativeEngFrequency =
            new Lazy<IReadOnlyDictionary<char, float>>(() =>
                RelativeEngFrequency.ToDictionary(
                    p => p.Key,
                    p => p.Value +
                         RelativeEngFrequency.Keys
                             .Where(k => k < p.Key)
                             .Sum(k => RelativeEngFrequency[k])));

        private int i = 0;

        private char[] arr = new[] { 'C', 'A', 'R', 'T' };

        /// <inheritdoc />
        public override char Invoke()
        {
#if !UNITY_IOS
            if (i == arr.Length)
            {
                i = 0;
            }
            return arr[i++];
#endif

            var rn = new Random(Environment.TickCount);

            var randomPick = rn.NextDouble();

            var randomChar = CummulativeEngFrequency.Value.First(p => p.Value > randomPick).Key;

            return randomChar;
        }
    }
}
