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
            var wordMatches = new List<WordMatch>();
            var staticCharsArr = staticChars.ToArray();

            var rows = staticCharsArr.GroupBy(c => c.Y);

            foreach (var row in rows)
            {
                wordMatches.AddRange(FindInVector(row.ToArray(), c => c.X));
            }

            var columns = staticCharsArr.GroupBy(c => c.X);

            foreach (var column in columns)
            {
                wordMatches.AddRange(FindInVector(column.ToArray(), c => c.Y));
            }

            return wordMatches.ToArray();
        }

        private WordMatch[] FindInVector(
            StaticChar[] staticChars,
            Func<StaticChar, int> getAxisIndex)
        {
            if (staticChars.Length == 0 ||
                staticChars.Length < _minWordLength)
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

                    if (potentialWord.Count >= _minWordLength)
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
                                FindInVector(staticChars.Skip(wordMatches[0].Word.Length - 1).ToArray(), getAxisIndex));
                        }
                        else
                        {
                            wordMatches.AddRange(
                                FindInVector(staticChars.Skip(1).ToArray(), getAxisIndex));
                        }
                    }
                }
                else
                {
                    wordMatches.AddRange(
                        FindInVector(staticChars.Skip(potentialWord.Count).ToArray(), getAxisIndex));
                }
            }

            return wordMatches.ToArray();
        }
    }
}
