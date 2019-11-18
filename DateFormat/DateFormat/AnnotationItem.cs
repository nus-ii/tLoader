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
        public string MarkedText { get => _markedText; set { _markedText = value; CleanMarkedText = value.ToLower().Trim(); } }
        private string _markedText { get; set; }
        public DateTime AddedDate { get; set; }
        public int Page { get; set; }
        public string BookTittle { get; set; }

        public bool OneWord
        {
            get
            {
                if (this.CleanMarkedText.Split(' ').Length != 1)
                    return true;

                return false;
            }
        }

        public string CleanMarkedText { get; private set; }

        public bool ValueEqual(AnnotationItem another)
        {
            if (this.CleanMarkedText == another.CleanMarkedText)
                return true;

            return false;
        }

        public bool FullEqual(AnnotationItem another)
        {
            if (this.Page == another.Page && this.ValueEqual(another) && this.AddedDate == another.AddedDate)
            {
                return true;
            }
            return false;
        }

        public string ToCsvString()
        {
            string result = $"{MarkedText};{AddedDate.ToString("G")};{BookTittle};{Page}";
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
