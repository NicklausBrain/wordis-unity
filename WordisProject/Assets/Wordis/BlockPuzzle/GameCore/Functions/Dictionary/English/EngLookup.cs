namespace Assets.Wordis.BlockPuzzle.GameCore.Functions.Dictionary.English
{
    public static class EngLookup
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
                case 'A': return EngLookupA.LookUp.Check(word);
                case 'B': return EngLookupB.LookUp.Check(word);
                case 'C': return EngLookupC.LookUp.Check(word);
                case 'D': return EngLookupD.LookUp.Check(word);
                case 'E': return EngLookupE.LookUp.Check(word);
                case 'F': return EngLookupF.LookUp.Check(word);
                case 'G': return EngLookupG.LookUp.Check(word);
                case 'H': return EngLookupH.LookUp.Check(word);
                case 'I': return EngLookupI.LookUp.Check(word);
                case 'J': return EngLookupJ.LookUp.Check(word);
                case 'K': return EngLookupK.LookUp.Check(word);
                case 'L': return EngLookupL.LookUp.Check(word);
                case 'M': return EngLookupM.LookUp.Check(word);
                case 'N': return EngLookupN.LookUp.Check(word);
                case 'O': return EngLookupO.LookUp.Check(word);
                case 'P': return EngLookupP.LookUp.Check(word);
                case 'Q': return EngLookupQ.LookUp.Check(word);
                case 'R': return EngLookupR.LookUp.Check(word);
                case 'S': return EngLookupS.LookUp.Check(word);
                case 'T': return EngLookupT.LookUp.Check(word);
                case 'U': return EngLookupU.LookUp.Check(word);
                case 'V': return EngLookupV.LookUp.Check(word);
                case 'W': return EngLookupW.LookUp.Check(word);
                case 'X': return EngLookupX.LookUp.Check(word);
                case 'Y': return EngLookupY.LookUp.Check(word);
                case 'Z': return EngLookupZ.LookUp.Check(word);
                default: return false;
            }
        }
    }
}
