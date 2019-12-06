using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DateFormat
{
    /// <summary>
    /// Класс поддержки словаря Lingualeo
    /// </summary>
    public class LionSupporter
    {
        private string _dictPath;
        public List<LionWord> Words { get; }

        public LionSupporter(string dictPath = @"C:\AllHarry\lingualeo.txt")
        {
            _dictPath = dictPath;

            var data = File.ReadAllLines(_dictPath);

            Words = data.Select(l => l.Split(';'))
                .Select(a => new LionWord()
                {
                    Word = a[0].Trim('"'),
                    Translate = a[1].Trim('"')
                }).ToList();
        }
    }
}
