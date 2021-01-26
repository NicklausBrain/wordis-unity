using System;
using gnuciDictionary;

namespace DictPreprocessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var defns = gnuciDictionary.EnglishDictionary.Define("beaver");

            foreach (Word defn in defns)
            {
                Console.WriteLine(defn.Definition);

            }
        }
    }
}
