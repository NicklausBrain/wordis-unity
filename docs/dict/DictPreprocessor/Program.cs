using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using gnuciDictionary;

namespace DictPreprocessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var mContent = File.ReadAllText(@"C:\Projects\wordis-unity\docs\dict\!Dictionary-in-csv\M.csv");

            var definitions = GetWordDefinitions(mContent);

            var mapDefinitions = definitions["map"];

            Console.WriteLine(mapDefinitions);

            //Enum.GetNames(typeof(WordCategory)).ToList().ForEach(Console.WriteLine);
        }

        static Dictionary<string, WordDefinition[]> GetWordDefinitions(string rawContent)
        {
            var definitions = rawContent
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(WordDefinition.Parse)
                .GroupBy(d => d.Word, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.ToArray(), StringComparer.OrdinalIgnoreCase);

            return definitions;
        }

        public class WordDefinition
        {
            public WordDefinition(
                string word,
                string category,
                string definition)
            {
                Word = word;
                Category = category;
                Definition = definition;
            }

            public string Word { get; }

            public string Category { get; }

            public string Definition { get; }

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
}
