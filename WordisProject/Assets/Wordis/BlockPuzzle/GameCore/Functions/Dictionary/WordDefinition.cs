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

        public string Definition
        {
            get
            {
                if (_definition.Length > 64) // magic string
                {
                    var shorterVersion = _definition.Split('.', ';')[0];
                    return shorterVersion;
                }

                return _definition;
            }
        }

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
