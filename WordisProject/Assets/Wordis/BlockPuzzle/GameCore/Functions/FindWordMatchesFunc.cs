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


        /// <summary>
        /// temp
        /// </summary>
        /// <param name="isLegitWordFunc"></param>
        public FindWordMatchesFunc(
            IsLegitWordFunc isLegitWordFunc = null)
        {
            _isLegitWordFunc = isLegitWordFunc ?? new IsLegitEngWordFunc();
        }

        public virtual WordMatch[] Invoke(WordisMatrix matrix, int minWordLength)
        {
            var staticChars = new List<StaticChar>();

            for (int y = 0; y < matrix.Height; y++)
            {
                for (int x = 0; x < matrix.Width; x++)
                {
                    if (matrix[x, y] is StaticChar)
                    {
                        staticChars.Add((StaticChar)matrix[x, y]);
                    }
                }
            }

            var wordMatches = new List<WordMatch>();
            var staticCharsArr = staticChars.ToArray();

            var rows = staticCharsArr.GroupBy(c => c.Y);

            foreach (var row in rows)
            {
                wordMatches.AddRange(FindInVector(
                    row.OrderBy(c => c.X).ToArray(),
                    c => c.X,
                    minWordLength));
            }

            var columns = staticCharsArr.GroupBy(c => c.X);

            foreach (var column in columns)
            {
                wordMatches.AddRange(FindInVector(
                    column.OrderBy(c => c.Y).ToArray(),
                    c => c.Y,
                    minWordLength));
            }

            return wordMatches.ToArray();
        }

        private WordMatch[] FindInVector(
            StaticChar[] staticChars,
            Func<StaticChar, int> getAxisIndex,
            int minWordLength)
        {
            if (staticChars.Length == 0 ||
                staticChars.Length < minWordLength)
            {
                return Array.Empty<WordMatch>();
            }

            var potentialWord = new List<StaticChar>();
            var wordMatches = new List<WordMatch>();

            foreach (StaticChar staticChar in staticChars)
            {
                if (potentialWord.Count == 0 ||
                    getAxisIndex(potentialWord[potentialWord.Count - 1]) == getAxisIndex(staticChar) - 1)
                {
                    potentialWord.Add(staticChar);

                    if (potentialWord.Count >= minWordLength)
                    {
                        var potentialMatch = new WordMatch(potentialWord);
                        var isLegitWord = _isLegitWordFunc.Invoke(potentialMatch.Word);
                        if (isLegitWord)
                        {
                            wordMatches.Add(potentialMatch);
                            if (wordMatches.Count > 1)
                            {
                                wordMatches.RemoveAt(0);
                            }
                        }
                        else if (wordMatches.Any())
                        {
                            wordMatches.AddRange(
                                FindInVector(
                                    staticChars.Skip(wordMatches[0].Word.Length - 1).ToArray(),
                                    getAxisIndex,
                                    minWordLength));
                        }
                        else
                        {
                            wordMatches.AddRange(
                                FindInVector(
                                    staticChars.Skip(1).ToArray(),
                                    getAxisIndex,
                                    minWordLength));
                        }
                    }
                }
                else
                {
                    wordMatches.AddRange(
                        FindInVector(
                            staticChars.Skip(potentialWord.Count).ToArray(),
                            getAxisIndex,
                            minWordLength));
                }
            }

            return wordMatches.ToArray();
        }
    }
}
