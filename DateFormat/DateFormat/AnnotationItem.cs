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
        public string markedText;
        public string addedDate;

        internal static AnnotationItem GetItem(DataRow r, BookProfile profile)
        {
            AnnotationItem result = new AnnotationItem();
            result.markedText = r.ItemArray[profile.Annotation.markedText].ToString();
            result.addedDate = r.ItemArray[profile.Annotation.addedDate].ToString();
            return result;
        }

        public string ToCSVstring()
        {
            string result="";
            result = string.Concat(markedText,";",uHelper.uDateFormat(addedDate).ToString());
            return result;
        }

        public string ToHTMLstring()
        {
            string result = "";
            result = markedText.Trim().ToLower();
            result = string.Concat("<p>",result, "</p>");
            return result;
        }

        public override string ToString()
        {
            return markedText.Trim().ToLower();
        }
    }
}
