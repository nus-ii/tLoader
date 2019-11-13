using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
    public class LionWord
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



    }
}
