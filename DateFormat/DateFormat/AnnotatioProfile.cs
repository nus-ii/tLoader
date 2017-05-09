using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
    public class AnnotationProfile
    {
        public int Id;
        public int ContentId;
        public int Type;
        public int AddedDate;
        public int ModdifiedDate;
        public int Name;
        public int MarkedText;
        public int Mark;
        public int MarkEnd;
        public int Page;
        public int TotalPage;
        public int MimeType;
        public int FilePath;

        internal static AnnotationProfile GetProfile(BookProfileType targetType)
        {
            var result = new AnnotationProfile();
            if(targetType==BookProfileType.T2)
            {
                result.Id = 0;
                result.ContentId = 1;
                result.Type = 2;
                result.AddedDate = 3;
                result.ModdifiedDate = 4;
                result.Name = 5;
                result.MarkedText = 6;
                result.Mark = 7;
                result.MarkEnd = 8;
                result.Page = 9;
                result.TotalPage = 10;
                result.MimeType = 11;
                result.FilePath = 12;
            }
            return result;
        }
    }
}
