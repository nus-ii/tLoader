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

	        var sl = SelectLogic();

			if (sl==1)
			ImportLogic(profile);
        }

	    public static int SelectLogic()
	    {
			Console.WriteLine("1 - Import annotation");
			Console.WriteLine("2 - Analysis annotation");

		   return  Convert.ToInt32(Console.ReadLine());
	    }

	    private static void ImportLogic(BookProfile profile)
	    {
			Console.Clear();
		    string dbCardFileName;
		    List<DbFileDescription> pretendents = new List<DbFileDescription>();
		    WaitReaderConnection(profile, ref pretendents);

		    dbCardFileName = pretendents.FirstOrDefault(p => p.Drive.VolumeLabel != profile.readerDriveLabel).FilePath;

		    string basePath = Environment.CurrentDirectory;
		    string logPath = Path.Combine(basePath, "Log.txt");
		    List<string> logList = File.ReadAllLines(logPath).ToList();
		    // List<string> logList = GetList(log);

		    var annotationList = AnnotationReader.Read(dbCardFileName, profile);

		    PrintAnnotation(annotationList);

		    Console.WriteLine($"Annotation number: {annotationList.Count}");
		    Console.ReadLine();

		    var filtredData = GetSource(annotationList, ref logList);
		    var res = HtmlMaster.GetHtmlList(filtredData);


		    var dn = DateTime.Now;
		    string ps = $"{dn.Day}_{dn.Month}_{dn.Year}_{dn.Hour}-{dn.Minute}.txt";
		    string resultPath = Path.Combine(basePath, ps);
		    File.WriteAllLines(resultPath, res);
		    File.WriteAllLines(logPath, GetArray(logList));
		    Console.WriteLine("All done!!!");
		    Console.ReadLine();
	    }

	    private static void PrintAnnotation(List<AnnotationItem> annotationList)
        {
            foreach(var item in annotationList)
            {
                Console.WriteLine(item.ToCsvString());
            }
        }

        private static void WaitReaderConnection(BookProfile profile, ref List<DbFileDescription> pretendents)
        {            
            while(!GetFilePath(profile,ref pretendents))
            {
                Console.Clear();
                Console.WriteLine("Reader not found!");
                System.Threading.Thread.Sleep(250);
            }
            System.Threading.Thread.Sleep(250);
            Console.Clear();
            Console.WriteLine("Reader ready for work, press any key!");
            Console.ReadLine();
        }

        private static bool GetFilePath(BookProfile profile, ref List<DbFileDescription> dbPretendents)
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

                foreach(var p in pretendent)
                {
                    dbPretendents.Add(new DbFileDescription {
                        Drive = p,
                        FilePath= Path.Combine(p.RootDirectory.FullName, profile.bookDbPath)
                });
                }
                //DriveInfo readerPretendent = pretendent.FirstOrDefault(p => p.VolumeLabel == profile.readerDriveLabel);
                //reader = Path.Combine(readerPretendent.RootDirectory.FullName, profile.bookDbPath);

                //DriveInfo cardPretendent = pretendent.FirstOrDefault(p => p.VolumeLabel != profile.readerDriveLabel);                
                //card = Path.Combine(cardPretendent.RootDirectory.FullName, profile.bookDbPath);
                
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

        private static List<string> GetSource(List<AnnotationItem> sourceData, ref List<string> log)
        {
            List<string> result = new List<string>();
            foreach (var s in sourceData)
            {
                var t = s.MarkedText.ToLower().Trim();

                if (!result.Contains(t) && !log.Contains(t))
                {
                    result.Add(s.MarkedText);
                    log.Add(t);
                }
            }
            return result;
        }               
    }
}
