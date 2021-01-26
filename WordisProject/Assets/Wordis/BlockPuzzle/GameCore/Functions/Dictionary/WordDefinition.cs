namespace Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary
{
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
