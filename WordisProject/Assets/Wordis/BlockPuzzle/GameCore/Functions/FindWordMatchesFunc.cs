using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Wordis.BlockPuzzle.GameCore.Objects;

namespace Assets.Wordis.BlockPuzzle.GameCore.Functions
{
    /// <summary>
    /// Finds the word matches among the <see cref="StaticChar"/> on board.
    /// </summary>
    public class FindWordMatchesFunc
    {
        private readonly IsLegitWordFunc _isLegitWordFunc;
        private readonly int _minWordLength;

        public FindWordMatchesFunc(
            IsLegitWordFunc isLegitWordFunc,
            int minWordLength)
        {
            _isLegitWordFunc = isLegitWordFunc;
            _minWordLength = minWordLength;
        }

        public virtual WordMatch[] Invoke(IEnumerable<StaticChar> staticChars)
        {
            var staticCharsArr = staticChars.ToArray();

            // 
            // [F][R][E][E][D][O][M]
            // [K][I][S][S][I][L][L][Y]
            var rows = staticCharsArr.GroupBy(c => c.X);

            var columns = staticCharsArr.GroupBy(c => c.Y);

            return Array.Empty<WordMatch>();
        }

        //private IEnumerable<WordMatch> FindInString(IEnumerable<StaticChar> staticChars)
        //{
        //}

        //  FindInRow startIndex, finIndex, staticCharArray
    }
}
