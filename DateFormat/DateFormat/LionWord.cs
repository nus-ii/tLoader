using DataFormat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
    public class LionWord:IDataObject
    {
        public string Word { get; set; }

        public string Translate { get; set; }

        public string CleanWord
        {
            get
            {
                return Word.ToLower().Trim();
            }
        }

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
