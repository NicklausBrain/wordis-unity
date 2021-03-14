using System;

namespace Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary
{
    public class WordDefinition
    {
        private readonly string _definition;

        public WordDefinition(
            string word,
            string category,
            string definition)
        {
            Word = word;
            Category = category;
            _definition = definition;
        }

        public string Word { get; }

        public string Category { get; }

        /// <summary>
        /// Display-friendly definition.
        /// </summary>
        public string Definition
        {
            get
            {
                // case for grammar forms
                if (_definition.StartsWith("of", StringComparison.OrdinalIgnoreCase))
                {
                    return $"{Category} {_definition}";
                }

                // shorten long definitions
                if (_definition.Length > 64) // magic length
                {
                    if (_definition.Contains("; --"))
                    {
                        return _definition.Split(new[] { "; --" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    else
                    {
                        return _definition.Split('.', ';')[0];
                    }
                }

                return _definition;
            }
        }

        /// <summary>
        /// Original word definition.
        /// </summary>
        public string FullDefinition => _definition;

        public static WordDefinition Parse(string rawDefinition)
        {
            var parts = rawDefinition.Split(new[] { '(', ')' });

            return new WordDefinition(
                word: parts[0].Trim(),
                category: parts[1].Trim(),
                definition: parts[2].Trim());
        }
    }
}
