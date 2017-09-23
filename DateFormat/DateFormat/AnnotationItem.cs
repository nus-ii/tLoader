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
        private string _markedText;
        public string MarkedText
        {
            get
            {
                return _markedText;
            }
            set
            {
                _markedText = value;
                _cleanMarkedText=MarkedText.ToLower().Trim();

            }
        }
        public DateTime AddedDate { get; set; }
        public int Page { get; set; }

        public bool OneWord
        {
            get
            {
                if (!this.CleanMarkedText.Contains(" "))
                    return true;

                return false;
            }
        }

        private string _cleanMarkedText;

        public string CleanMarkedText
        {
            get
            {
                return _cleanMarkedText;
            }
        }

        internal static AnnotationItem GetItem(DataRow r, BookProfile profile)
        {
            AnnotationItem result = new AnnotationItem();
            result.MarkedText = r.ItemArray[profile.Annotation.MarkedText].ToString();
            result.AddedDate = UHelper.UDateFormat(r.ItemArray[profile.Annotation.AddedDate].ToString());
            result.Page = Convert.ToInt32(r.ItemArray[profile.Annotation.Page].ToString());
            return result;
        }

        public bool ValueEqual(AnnotationItem another)
        {
            if (this.CleanMarkedText == another.CleanMarkedText)
                return true;

            return false;
        }

        public bool FullEqual(AnnotationItem another)
        {
            if (this.AddedDate == another.AddedDate && this.Page == another.Page)
            {
                if (this.ValueEqual(another))
                    return true;
            }
            return false;
        }

        public string ToCsvString()
        {
            string result = "";
            result = string.Concat(MarkedText, ";", AddedDate.ToString("G"), ";", Page);
            return result;
        }

        public override string ToString()
        {
            return MarkedText.Trim().ToLower();
        }

        public static List<AnnotationItem> OnlyUnique(List<AnnotationItem> target)
        {
            List<AnnotationItem> result = new List<AnnotationItem>();

            foreach (var i in target)
            {
                if (result.Count == 0 || !result.Any(a => a.FullEqual(i)))
                    result.Add(i);
            }
            return result;
        }


    }
}
