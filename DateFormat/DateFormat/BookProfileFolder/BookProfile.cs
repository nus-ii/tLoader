using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
   
    public class BookProfile 
    {     
        public string BookDbPath { get; set; }
        public string ReaderDriveLabel { get; set; }
        public static BookProfile GetProfile()
        {
            BookProfile result = new BookProfile();
            result.BookDbPath = @"Sony_Reader\database\books.db";
            result.ReaderDriveLabel = "READER";            
            return result;
        }
    }
}
