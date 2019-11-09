using DataFormat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
    public class CsvSaveMaster
    {
        private readonly string diretoryPath;
        private readonly string bookShortName;

        public CsvSaveMaster(string diretoryPath, string bookShortName)
        {
            this.diretoryPath = diretoryPath;
            this.bookShortName = bookShortName;
        }

        internal void Save<T>(List<T> data,string reportName) where T :IDataObject
        {
            var t = data.First();
            List<string> result = new List<string> { t.CSVHeader };

            result.AddRange(data.Select(s => s.CSVstring).ToList());
          
            string fileName = $"{bookShortName}-{reportName}.csv";

            File.WriteAllLines(Path.Combine(diretoryPath, fileName), result);
        }
    }
}
