using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DateFormat
{
    public class AnnotationImporter
    {

        public static List<string> ImportLogic(BookProfile profile,List<Tuple<string,string>> dict)
        {
            Console.Clear();
            string dbCardFileName;
            List<DbFileDescription> pretendents = new List<DbFileDescription>();
            WaitReaderConnection(profile, ref pretendents);

            dbCardFileName = pretendents.FirstOrDefault(p => p.Drive.VolumeLabel != profile.ReaderDriveLabel).FilePath;

            //string basePath = Environment.CurrentDirectory;
            //string logPath = Path.Combine(basePath, "Log.txt");
            //List<string> logList = File.ReadAllLines(logPath).ToList();

            List<AnnotationItem> preAnnotationList = AnnotationReader.Read(dbCardFileName);
            List<AnnotationItem> annotationList = new List<AnnotationItem>();           
 
            foreach(AnnotationItem p in preAnnotationList)
            {
                if (annotationList.All(a => a.CleanMarkedText != p.CleanMarkedText))
                    annotationList.Add(p);
            }

            annotationList.Sort();

            PrintAnnotation(annotationList);

            Console.WriteLine($"Annotation number: {annotationList.Count}");
            Console.ReadLine();

            //var filtredData = GetSource(annotationList, ref logList);

            //

            List<AnnotationItem> withoutTranslate = Separate(annotationList,dict,out List<Tuple<string,string>> forgotten);

            List<string> resultData = new List<string>();           

            List<Tuple<string, List<AnnotationItem>>> separetedByEnding = separeteByEnding(withoutTranslate,new List<string> {"ing","ed","es","ly"});

            foreach(var bigT in separetedByEnding)
            {
                resultData.Add("----------------------------------------------" + bigT.Item1);
                resultData.AddRange(bigT.Item2.Select(a=>a.MarkedText));
            }

            resultData.Add("----------------------------------------------forgotten");

            resultData.AddRange(forgotten.Select(i=>$"{i.Item1} - {i.Item2}"));

            return resultData;
        }

        private static List<Tuple<string, List<AnnotationItem>>> separeteByEnding(List<AnnotationItem> annotations, List<string> list)
        {        
            
            List<Tuple<string, List<AnnotationItem>>> result = new List<Tuple<string, List<AnnotationItem>>>();

            result.Add(new Tuple<string, List<AnnotationItem>>("other", annotations.Where(a => list.All(e => !a.CleanMarkedText.EndsWith(e))).ToList()));
            foreach (string ending in list)
            {
                result.Add(new Tuple<string, List<AnnotationItem>>(ending,annotations.Where(a=>a.CleanMarkedText.EndsWith(ending)).ToList()));
            }           

            return result;
        }

        /// <summary>
        /// Разделение входящих анотаций на забытые и новые
        /// </summary>
        /// <param name="annotationList">Исходный список аннотаций</param>
        /// <param name="dict">Словарь знакомых слов</param>
        /// <param name="forgotten">Список забытых слов с переводом</param>
        /// <returns></returns>
        private static List<AnnotationItem> Separate(List<AnnotationItem> annotationList, 
            List<Tuple<string, string>> dict, 
            out List<Tuple<string, string>> forgotten)
        {
            List<AnnotationItem> result = new List<AnnotationItem>();
            forgotten = new List<Tuple<string, string>>();

            List<Tuple<string, string>> normalDict = dict.Select(i => new Tuple<string, string>(i.Item1.ToLower().Trim(), i.Item2)).ToList();

            foreach(AnnotationItem item in annotationList)
            {
                var t = normalDict.FirstOrDefault(i => i.Item1 == item.MarkedText.ToLower().Trim());
                if(t!=null)
                {
                    forgotten.Add(t);
                }
                else
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private static void PrintAnnotation(List<AnnotationItem> annotationList)
        {
            foreach (var item in annotationList)
            {
                Console.WriteLine(item.ToCsvString());
            }
        }

        private static void WaitReaderConnection(BookProfile profile, ref List<DbFileDescription> pretendents)
        {

            while (!GetFilePath(profile, ref pretendents))
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
                if (File.Exists(Path.Combine(d.RootDirectory.FullName, profile.BookDbPath)))
                {
                    pretendent.Add(d);
                }
            }

            if (pretendent.Count != 0)
            {

                foreach (var p in pretendent)
                {
                    dbPretendents.Add(new DbFileDescription
                    {
                        Drive = p,
                        FilePath = Path.Combine(p.RootDirectory.FullName, profile.BookDbPath)
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
