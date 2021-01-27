using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Wordis.BlockPuzzle.GameCore.Extensions
{
    /// <summary>
    /// Extensions for <see cref="System.Collections"/>
    /// </summary>
    public static class CollectionsExtensions
    {
        /// <summary>
        /// Shuffles the generic list.
        /// Taken from https://stackoverflow.com/questions/273313/randomize-a-listt/1262619
        /// </summary>
        public static IReadOnlyList<T> Shuffle<T>(
            this IReadOnlyList<T> sourceList)
        {
            var list = sourceList.ToArray();

            Random rng = new Random(Environment.TickCount);
            int n = list.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}
