using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
	class WordInfo
	{
		public string value;

		public int number;

		public string CSVstring
		{
			get { return string.Format($"{value};{number}"); }
		}

		public WordInfo(string val)
		{
			this.value = val;
			number = 1;
		}

		
	}
}
