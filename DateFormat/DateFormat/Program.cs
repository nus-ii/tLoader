using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Management;
//Работа с датой

namespace DateFormat
{
    class Program
    {
        static void Main(string[] args)
        {


            var profile = BookProfile.GetProfile(BookProfileType.T2);

            String dbFileName = GetFilePath(profile,true);
            
            string basePath = @"C:\MyTempBook\";
            string logPath = string.Concat(basePath, "Log.txt");
            var log = File.ReadAllLines(logPath);
            List<string> logList = GetList(log);

            #region MyRegion
            SQLiteConnection m_dbConn;
            SQLiteCommand m_sqlCmd;
            m_dbConn = new SQLiteConnection();
            m_sqlCmd = new SQLiteCommand();
            m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
            m_dbConn.Open();
            m_sqlCmd.Connection = m_dbConn;
            var sqlQuery = "SELECT * FROM annotation";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            DataTable dTable = new DataTable();
            adapter.Fill(dTable); 
            #endregion            
            
            List<AnnotationItem> AnnotationList = new List<AnnotationItem>();

            foreach (DataRow r in dTable.Rows)
            {
                var item = AnnotationItem.GetItem(r, profile);
                AnnotationList.Add(item);
                Console.WriteLine(item.ToCSVstring());    
            }

            List<string> anList = GetListAn(AnnotationList);
            var res = GetSource(anList, ref logList);

            var dn = DateTime.Now;
            string ps = string.Format("{0}_{1}_{2}_{3}-{4}", dn.Day, dn.Month, dn.Year, dn.Hour, dn.Minute);
            //string[] res = GetRes(sourceList, ps);
            File.WriteAllLines(basePath + ps + ".txt", res);
            File.WriteAllLines(logPath, GetArray(logList));
            Console.WriteLine("All done!!!");
            Console.ReadLine();

        }

        private static List<string> GetListAn(List<AnnotationItem> annotationList)
        {
            List<string> result = new List<string>();
            foreach(var i in annotationList)
            {
                result.Add(i.ToString());
            }
            return result;
        }

        private static string GetFilePath(BookProfile profile,bool card=true)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            List<DriveInfo> pretendent = new List<DriveInfo>();

            foreach(var d in allDrives)
            {
                if(File.Exists(Path.Combine(d.RootDirectory.FullName,profile.bookDbPath)))
                    {
                    pretendent.Add(d);
                    }
            }

            DriveInfo firstPretendent = pretendent.FirstOrDefault(p => p.VolumeLabel != "READER");

            if(card==false)
                firstPretendent = pretendent.FirstOrDefault(p => p.VolumeLabel == "READER");

            string result = Path.Combine(firstPretendent.RootDirectory.FullName, profile.bookDbPath);
            return result;
        }

        private static string[] GetArray(List<string> logList)
        {
            string[] result = new string[logList.Count];
            int i = 0;
            foreach (var s in logList)
            {
                result[i] = s;
                i++;
            }
            return result;
        }

        private static List<string> GetList(string[] logData)
        {
            List<string> result = new List<string>();
            foreach (var s in logData)
            {
                result.Add(s);
            }
            return result;
        }

        private static List<string> GetSource(List<string> sourceData, ref List<string> log)
        {
            List<string> result = new List<string>();
            foreach (var s in sourceData)
            {
                var t = s.ToLower();

                if (!ListContainValue(result, t) && !log.Contains(t))
                {
                    result.Add(GetHTMLstring(t));
                    log.Add(t);
                }
            }
            return result;
        }

        private static string GetHTMLstring(string t)
        {
            return string.Concat("<p>", t, "</p>");
        }

        private static bool ListContainValue(List<string> result, string t)
        {
            return result.Contains(t);
        }
    }
}
