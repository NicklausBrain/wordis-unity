namespace Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary.English
{
    public static partial class EngLookup
    {
        public static bool Contains(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return false;
            }

            var firstChar = char.ToUpperInvariant(word[0]);

            switch (firstChar)
            {
                case 'A': return ALookUp.Check(word);
                case 'B': return BLookUp.Check(word);
                case 'C': return CLookUp.Check(word);
                case 'D': return DLookUp.Check(word);
                case 'E': return ELookUp.Check(word);
                case 'F': return FLookUp.Check(word);
                case 'G': return GLookUp.Check(word);
                case 'H': return HLookUp.Check(word);
                case 'I': return ILookUp.Check(word);
                case 'J': return JLookUp.Check(word);
                case 'K': return KLookUp.Check(word);
                case 'L': return LLookUp.Check(word);
                case 'M': return MLookUp.Check(word);
                case 'N': return NLookUp.Check(word);
                case 'O': return OLookUp.Check(word);
                case 'P': return PLookUp.Check(word);
                case 'Q': return QLookUp.Check(word);
                case 'R': return RLookUp.Check(word);
                case 'S': return SLookUp.Check(word);
                case 'T': return TLookUp.Check(word);
                case 'U': return ULookUp.Check(word);
                case 'V': return VLookUp.Check(word);
                case 'W': return WLookUp.Check(word);
                case 'X': return XLookUp.Check(word);
                case 'Y': return YLookUp.Check(word);
                case 'Z': return ZLookUp.Check(word);
                default: return false;
            }
        }
    }
}
