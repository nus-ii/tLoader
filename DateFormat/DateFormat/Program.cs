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
            String dbCardFileName = "";
            string dbReaderFileName = "";
            WaitReaderConnection(profile,ref dbCardFileName,ref dbReaderFileName);

            string basePath = Environment.CurrentDirectory;// @"C:\MyTempBook\";
            string logPath = Path.Combine(basePath, "Log.txt");
            var log = File.ReadAllLines(logPath);
            List<string> logList = GetList(log);

            #region MyRegion
            SQLiteConnection m_dbConn;
            SQLiteCommand m_sqlCmd;
            m_dbConn = new SQLiteConnection();
            m_sqlCmd = new SQLiteCommand();
            m_dbConn = new SQLiteConnection("Data Source=" + dbCardFileName + ";Version=3;");
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
            string ps = string.Format("{0}_{1}_{2}_{3}-{4}.txt", dn.Day, dn.Month, dn.Year, dn.Hour, dn.Minute);
            string resultPath = Path.Combine(basePath,ps);
            File.WriteAllLines(resultPath, res);
            File.WriteAllLines(logPath, GetArray(logList));
            Console.WriteLine("All done!!!");
            Console.ReadLine();

        }
        private static void WaitReaderConnection(BookProfile profile, ref string dbA,ref string dbB)
        {
            while(!GetFilePath(profile,ref dbA,ref dbB))
            {
                Console.Clear();
                Console.WriteLine("Reader not found!");
                System.Threading.Thread.Sleep(250);
            }
            Console.Clear();
            Console.WriteLine("Reader ready for work, press any key!");
            Console.ReadLine();
        }
        private static List<string> GetListAn(List<AnnotationItem> annotationList)
        {
            List<string> result = new List<string>();
            foreach (var i in annotationList)
            {
                result.Add(i.ToString());
            }
            return result;
        }

        private static bool GetFilePath(BookProfile profile, ref string card, ref string reader)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            List<DriveInfo> pretendent = new List<DriveInfo>();

            foreach (var d in allDrives)
            {
                if (File.Exists(Path.Combine(d.RootDirectory.FullName, profile.bookDbPath)))
                {
                    pretendent.Add(d);
                }
            }

            if (pretendent.Count != 0)
            {
                DriveInfo cardPretendent = pretendent.FirstOrDefault(p => p.VolumeLabel != "READER");
                DriveInfo readerPretendent = pretendent.FirstOrDefault(p => p.VolumeLabel == "READER");

                card = Path.Combine(cardPretendent.RootDirectory.FullName, profile.bookDbPath);
                reader= Path.Combine(readerPretendent.RootDirectory.FullName, profile.bookDbPath);
                return true;
            }
            else
            {
                return false;
            }
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
