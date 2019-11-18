using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFormat
{
	public class PageInfo : IDataObject
    {
		public int Number { get; set; }

		public  int UniqAnnotation{ get; set; }

		public int NotuniqAnnotaion { get; set; }

		public int AllAnnotaion { get; set; }

		public string CSVstring
		{
			get { return string.Format($"{Number};{NotuniqAnnotaion};{UniqAnnotation}"); }
		}

        public string CSVHeader
        {
           get{ return "Page;NotuniqAnnotaion;UniqAnnotation"; }
        }       

        public PageInfo(int number)
		{
			this.Number = number;
		}


	}
}
