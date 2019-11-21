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

        internal void Save<T>(List<T> data,string reportName, string fileExtension = "csv") where T :IDataObject
        {
            List<string> result = Combine(data);

            this.Save(result, reportName,fileExtension);
        }

        internal void Save(List<string> data,string reportName,string fileExtension="txt")
        {
            if (string.IsNullOrEmpty(reportName))
                throw new Exception("Необходимо название отчёта");

            string fileName = $"{reportName}.{fileExtension}";

            File.WriteAllLines(Path.Combine(diretoryPath, fileName), data);
        }

        internal void Print<T>(List<T> data, string reportName="") where T : IDataObject
        {
            List<string> result = Combine(data);

            this.Print(result,reportName);
        }

        internal void Print(List<string> data, string reportName="")
        {
            Console.WriteLine(reportName);
            foreach (string s in data)
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
