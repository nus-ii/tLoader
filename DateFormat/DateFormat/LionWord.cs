using DataFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
    public class LionWord : Word, IDataObject
    {
        public string Word { get { return _word; } set { _word = value; _cleanWord = value.ToLower().Trim(); } }

        private string _word;

        public string Translate { get; set; }

        new public string CleanWord
        {
            get
            {
                return Word.ToLower().Trim();
            }
        }

        private string _cleanWord;

        public string CSVstring
        {
            get
            {
                return $"{Word};{Translate};";
            }
        }

        public string CSVHeader => "Word;Translate;";
    }
}
