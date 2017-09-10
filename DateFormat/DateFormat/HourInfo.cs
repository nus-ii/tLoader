using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
	public class HourInfo
	{
		private int mp;

		public int maxpage
		{
			get { return mp; }
			set
			{
				if(!pages.Contains(value))
					pages.Add(value);

				if (value > mp)
					mp = value;
			}
		}

		public List<int> pages;

		public string CSVstring
		{
			get { return string.Format("{0};{2};{1}", this.hour.ToString("d"), this.maxpage,pages.Count); }
		}
		
		public DateTime hour;

		public HourInfo(DateTime date)
		{
			pages = new List<int>();
			this.hour = date;
		}

		public bool Eq(DateTime target)
		{
			if (target.Year==hour.Year&&target.Month == hour.Month && target.Day == hour.Day)
				return true;

			return false;
		}

		
	}
}
