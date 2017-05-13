using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
	public class HtmlMaster
	{
		public static List<string> GetHtmlList(List<string> data)
		{
			var dt = DateTime.Now;
			string dateFormated=String.Format($"{dt.Day}-{dt.Month}-{dt.Year}_{dt.Hour}:{dt.Minute}");
			List<string> res = new List<string>();			
			res.Add("<div dir=\"ltr\" style=\"text - align: left; \" trbidi=\"on\">");
            res.Add(string.Concat("<p>", dateFormated, "</p>"));
            res.AddRange(data.Select(s=>string.Concat("<p>", s, "</p>")).ToList());
			res.Add("<br /></div>");
			return res;
		}
	}
}
