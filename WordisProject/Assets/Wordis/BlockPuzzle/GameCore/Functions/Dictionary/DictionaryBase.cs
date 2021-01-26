using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary
{
    /// <summary>
    /// Base functionality for language dictionary.
    /// </summary>
    public abstract class DictionaryBase
    {
        private readonly Lazy<IDictionary<string, WordDefinition[]>> _allDefinitions;

        protected DictionaryBase()
        {
            _allDefinitions = new Lazy<IDictionary<string, WordDefinition[]>>(() =>
                GetWordDefinitions(WordsInCsv));
        }

        /// <summary>
        /// Words and definitions as raw string.
        /// </summary>
        protected virtual string WordsInCsv => string.Empty;

        /// <summary>
        /// Gets word definitions.
        /// </summary>
        public virtual IReadOnlyList<WordDefinition> Define(string word)
        {
            var isWordDefined = _allDefinitions.Value.TryGetValue(
                word,
                out var wordDefinitions);

            return isWordDefined
                ? wordDefinitions
                : Array.Empty<WordDefinition>();
        }

        private static Dictionary<string, WordDefinition[]> GetWordDefinitions(string rawContent)
        {
            var definitions = rawContent
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(WordDefinition.Parse)
                .GroupBy(d => d.Word, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.ToArray(), StringComparer.OrdinalIgnoreCase);

            return definitions;
        }
    }
}
