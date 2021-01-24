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

        public FindWordMatchesFunc(
            IsLegitWordFunc isLegitWordFunc = null)
        {
            _isLegitWordFunc = isLegitWordFunc ?? new IsLegitEngWordFunc();
        }

        /// <summary>
        /// Finds English words in a matrix of Wordis objects and stores the search results in a list. 
        /// </summary>
        /// <param name="m">Matrix of Wordis objects</param>
        /// <param name="l">Length of the shortest acceptable word</param>
        /// <param name="r">Orientation of the searched words: "true" - horizontal, i.e. words are within rows.</param>
        /// <param name="v">List used to store the words which have been found.</param>
        private void GetWordMatch(WordisMatrix m, int l, bool r, List<WordMatch> v)
        {
            var s = new List<StaticChar>();

            int h = r ? m.Height : m.Width;
            int w = r ? m.Width : m.Height;
                       
            int t = w - l + 1;

            for (int y = 0; y < h; y++)
            {
                int x = 0;

                while (x < t)
                {
                    int z = x;

                    do
                    {
                        WordisObj e = r ? m[z, y] : m[y, z];
                        if (e is StaticChar c) { s.Add(c); z++; } else break;
                        
                        if (s.Count < l) continue;
                        
                        string q = new string(s.Select(i => i.Value).ToArray());
                        if (_isLegitWordFunc.Invoke(q)) v.Add(new WordMatch(s)); 
                    }
                    while (z < w);

                    if (s.Count <= l)
                    {
                        for (x = z + 1; x < t; x++)
                            if ((r ? m[x, y] : m[y, x]) is StaticChar) break;
                    }
                    else x++;

                    s.Clear();
                }
            }
        }

        /// <summary>
        /// Finds English words in a matrix of Wordis objects. 
        /// </summary>
        /// <param name="matrix">Matrix of Wordis objects</param>
        /// <param name="minWordLength">Length of the shortest acceptable word</param>
        /// <returns>Array of the search results</returns>
        public virtual WordMatch[] Invoke(WordisMatrix matrix, int minWordLength)
        {
            List<WordMatch> v = new List<WordMatch>();
            GetWordMatch(matrix, minWordLength, true, v);
            GetWordMatch(matrix, minWordLength, false, v);
            return v.ToArray();
        }
    }
}