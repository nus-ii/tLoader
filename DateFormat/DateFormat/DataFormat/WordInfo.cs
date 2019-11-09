using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFormat
{
	public class WordInfo:IDataObject
	{
		public string value;

		public int number;

		public string CSVstring
		{
			get { return string.Format($"{value};{number}"); }
		}

        public string CSVHeader => "Word;Count";

        public WordInfo(string val)
		{
			this.value = val;
			number = 1;
		}

		
	}
}
