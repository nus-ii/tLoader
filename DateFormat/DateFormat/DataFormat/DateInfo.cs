using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFormat
{
    public class DateInfo : IDataObject
    {
        private int _maxpage;

        public int Maxpage
        {
            get { return _maxpage; }
            set
            {
                if (!pages.Contains(value))
                    pages.Add(value);

                if (value > _maxpage)
                    _maxpage = value;
            }
        }

        public List<int> pages;

        public DateTime hour;

        public DateInfo(DateTime date)
        {
            pages = new List<int>();
            this.hour = date;
        }

        public bool Equal(DateTime target)
        {
            if (target.Year == hour.Year && target.Month == hour.Month && target.Day == hour.Day)
                return true;

            return false;
        }

        public string CSVstring
        {
            get { return string.Format("{0};{2};{1}", this.hour.ToString("d"), this.Maxpage, pages.Count); }
        }

        public string CSVHeader => "Date;PageCount;LastPage";
    }
}
