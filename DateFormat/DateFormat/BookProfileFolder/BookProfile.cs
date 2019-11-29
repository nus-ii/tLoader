using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DateFormat.BookProfileFolder;

namespace DateFormat
{
    public enum BookProfileType { T2 }
    public class BookProfile : BookProfileBase
    {
        public BookProfileType BookType { get; set; }
        public string BookDbPath { get; set; }
        public string ReaderDriveLabel { get; set; }


        public static BookProfile GetProfile(BookProfileType targetType)
        {
            BookProfile result = new BookProfile();
            if (targetType == BookProfileType.T2)
            {
                result.BookType = targetType;
                result.BookDbPath = @"Sony_Reader\database\books.db";
                result.ReaderDriveLabel = "READER";
            }
            return result;
        }
    }
}
