using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
    public enum BookProfileType {T2 }
    public class BookProfile
    {
        public BookProfileType BookType { get; set; }
        public AnnotationProfile Annotation { get; set; }
        public string bookDbPath { get; set; }
        public string readerDriveLabel { get; set; }


        public static BookProfile GetProfile(BookProfileType targetType)
        {
            BookProfile result = new BookProfile();
            if (targetType == BookProfileType.T2)
            {
                result.BookType = targetType;
                result.Annotation = AnnotationProfile.GetProfile(targetType);
                result.bookDbPath = @"Sony_Reader\database\books.db";
                result.readerDriveLabel = "READER";
            }
            return result;
        }
    }
}
