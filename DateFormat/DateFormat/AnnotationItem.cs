using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
     public class AnnotationItem
    {
        public string MarkedText;
        public string AddedDate;       

        internal static AnnotationItem GetItem(DataRow r, BookProfile profile)
        {
            AnnotationItem result = new AnnotationItem();
            result.MarkedText = r.ItemArray[profile.Annotation.MarkedText].ToString();
            result.AddedDate = r.ItemArray[profile.Annotation.AddedDate].ToString();
            return result;
        }

        public string ToCsvString()
        {
            string result="";
            result = string.Concat(MarkedText,";",UHelper.UDateFormat(AddedDate).ToString());
            return result;
        }

        public string ToHtmlString()
        {
            string result = "";
            result = MarkedText.Trim().ToLower();
            result = string.Concat("<p>",result, "</p>");
            return result;
        }

        public override string ToString()
        {
            return MarkedText.Trim().ToLower();
        }
    }
}
