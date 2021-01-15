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
                case 'I': return EngLookupI.ILookUp.Check(word);
                case 'J': return EngLookupJ.JLookUp.Check(word);
                case 'K': return EngLookupK.KLookUp.Check(word);
                case 'L': return EngLookupL.LLookUp.Check(word);
                case 'M': return EngLookupM.MLookUp.Check(word);
                case 'N': return EngLookupN.NLookUp.Check(word);
                case 'O': return EngLookupO.OLookUp.Check(word);
                case 'P': return EngLookupP.PLookUp.Check(word);
                case 'Q': return EngLookupQ.QLookUp.Check(word);
                case 'R': return EngLookupR.RLookUp.Check(word);
                case 'S': return EngLookupS.SLookUp.Check(word);
                case 'T': return EngLookupT.TLookUp.Check(word);
                case 'U': return EngLookupU.ULookUp.Check(word);
                case 'V': return EngLookupV.VLookUp.Check(word);
                case 'W': return EngLookupW.WLookUp.Check(word);
                case 'X': return EngLookupX.XLookUp.Check(word);
                case 'Y': return EngLookupY.YLookUp.Check(word);
                case 'Z': return EngLookupZ.ZLookUp.Check(word);
                default: return false;
            }
        }
    }
}
