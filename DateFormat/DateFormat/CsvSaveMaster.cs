using DataFormat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
    public class OutputMaster
    {
        private readonly string diretoryPath;        

        public OutputMaster(string diretoryPath)
        {
            this.diretoryPath = diretoryPath;            
        }

        internal void Save<T>(List<T> data,string reportName) where T :IDataObject
        {
            List<string> result = Combine(data);
          
            string fileName = $"{reportName}.csv";

            File.WriteAllLines(Path.Combine(diretoryPath, fileName), result);
        }

        internal void Print<T>(List<T> data, string reportName="") where T : IDataObject
        {
            List<string> result = Combine(data);

            Console.WriteLine(reportName);
            foreach(string s in result)
            {
                Console.WriteLine(s);
            }
        }

        private List<string> Combine<T>(List<T> data) where T : IDataObject
        {
            var t = data.First();
            List<string> result = new List<string> { t.CSVHeader };

            result.AddRange(data.Select(s => s.CSVstring).ToList());

            return result;
        }

    }
}
