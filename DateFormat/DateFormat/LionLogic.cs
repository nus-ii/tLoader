using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DateFormat
{
    public class LionLogic
    {
        private string _dictPath;

        public LionLogic(string dictPath = @"C:\AllHarry\lingualeo_dict.csv")
        {
            _dictPath = dictPath;
        }

        public void Analysis()
        {
            var data = File.ReadAllLines(_dictPath);

            List<LionWord> words = data.Select(l => l.Split(';'))
                .Select(a => new LionWord()
                {
                    Word = a[0].Trim('"'),
                    Translate = a[1].Trim('"')
                }).ToList();

            int i = 0;
            foreach(var w in words)
            {
                Console.WriteLine($"{w.Word} - {w.Translate}");
                i++;
                if (i == 100)
                    break;
            }
            Console.Write(words.Count());
            
        }
    }
}
