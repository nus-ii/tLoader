using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Management;
using DataFormat;
//Работа с датой

namespace DateFormat
{
    class Program
    {
        static void Main(string[] args)
        {
            var profile = BookProfile.GetProfile(BookProfileType.T2);

            MenuMaster mainMenu = new MenuMaster(new string[] { "Import annotation", "Analysis annotation" });
            
            mainMenu.GetAnswer("",true);
            if (mainMenu.Answer == "Import annotation")
                ImportLogic(profile);

            if (mainMenu.Answer == "Analysis annotation")
                AnalysisLogic();

        }

        private static void AnalysisLogic(string diretoryPath= @"C:\AllHarry\")
        {

            List<AnnotationItem> annotationList = GetAnnotationsFromAllDb(diretoryPath);

            var bookMenu = annotationList.Select(a => a.BookTittle).Distinct().ToList();
            bookMenu.Add("All");
            MenuMaster bookMenuMaster = new MenuMaster(bookMenu);

            string bookAnswer = bookMenuMaster.GetAnswer("Choice book for analysis", true);

            if(bookAnswer!="All")
            annotationList = FilterByBook(annotationList, bookAnswer);

            string bookShortName = GetShortName(bookAnswer);

            CsvSaveMaster saveMaster = new CsvSaveMaster(diretoryPath, bookShortName);            

            var uniq=UniqAnnotaion(annotationList);
            saveMaster.Save(uniq,"UniqAnnotaion");

            var words=WordsAddings(annotationList);
            saveMaster.Save(words, "WordsRepeat");

            var pages=PagesPerDay(annotationList);
            saveMaster.Save(pages, "PagesPerDay");

            Console.ReadLine();
        }

        private static string GetShortName(string book)
        {
            book = book.ToLower().Trim();
            string temp = "";
            
            foreach(char c in book)
            {
                if (char.IsLetter(c)||c==' ')
                {
                    temp += c.ToString();
                }
            }

            var splittedTemp = temp.Split(' ');
            string result = "";
            for(int i = splittedTemp.Count()-1 ; i >= 0; i--)
            {
                
                result = splittedTemp[i] + "_" + result;

                if (splittedTemp[i] == "the")
                    break;
            }          
            return result.Trim('_');
        }

        /// <summary>
        /// Количество страниц в день
        /// </summary>
        /// <param name="annotationList"></param>
        /// <param name="diretoryPath"></param>
        private static List<DateInfo> PagesPerDay(List<AnnotationItem> annotationList)
        {
            List<DateInfo> DateInfoList = new List<DateInfo>();

            foreach (var a in annotationList)
            {
                if (DateInfoList.Count == 0 || !DateInfoList.Any(d => d.Equal(a.AddedDate)))
                {
                    DateInfoList.Add(new DateInfo(a.AddedDate));
                }
                else
                {
                    var h = DateInfoList.FirstOrDefault(f => f.Equal(a.AddedDate));

                    if (h != null)
                        h.Maxpage = a.Page;
                }
            }
            return DateInfoList;
        }

        /// <summary>
        /// Количество повторных добавлений слова
        /// </summary>
        /// <param name="annotationList"></param>
        /// <param name="diretoryPath"></param>
        private static List<WordInfo> WordsAddings(List<AnnotationItem> annotationList)
        {
            List<WordInfo> wordInfoList = new List<WordInfo>();

            foreach (var a in annotationList)
            {
                if (wordInfoList.Count == 0 || !wordInfoList.Any(w => w.value == a.CleanMarkedText))
                {
                    wordInfoList.Add(new WordInfo(a.CleanMarkedText));
                }
                else
                {
                    var w = wordInfoList.FirstOrDefault(i => i.value == a.CleanMarkedText);
                    if (w != null)
                        w.number++;
                }
            }
            return wordInfoList;
        }


        /// <summary>
        /// Количество уникальных слов на странице
        /// </summary>
        /// <param name="annotationList"></param>
        /// <param name="diretoryPath"></param>
        private static List<PageInfo> UniqAnnotaion(List<AnnotationItem> annotationList)
        {
            List<PageInfo> pages = new List<PageInfo>();
            List<int> pagesNumbers = annotationList.Select(a => a.Page).ToList();
            var en = Enumerable.Range(pagesNumbers.Min(), pagesNumbers.Max());

            foreach (var VARIABLE in en)
            {
                var p = new PageInfo(VARIABLE);
                var annOnPage = annotationList.Where(a => a.Page == VARIABLE);
                var annBeforePage = annotationList.Where(a => a.Page < VARIABLE);

                foreach (var anno in annOnPage)
                {
                    if (annBeforePage.Any(a => a.ValueEqual(anno)))
                    {
                        p.NotuniqAnnotaion++;
                    }
                    else
                    {
                        p.UniqAnnotation++;
                    }
                }
                pages.Add(p);
            }
            return pages;
        }

        private static List<AnnotationItem> GetAnnotationsFromAllDb(string diretoryPath)
        {
            List<AnnotationItem> fullResult =new List<AnnotationItem>();

            DirectoryInfo dir = new DirectoryInfo(diretoryPath);
            var fileList = dir.GetFiles().Where(i => i.Name.Contains("book") && i.Name.Contains(".db"));
            foreach (var file in fileList)
            {
                fullResult.AddRange(AnnotationReader.Read(file.FullName));
            }
            
            fullResult = AnnotationItem.OnlyUnique(fullResult);


            return fullResult;
        }

        private static List<AnnotationItem>  FilterByBook(List<AnnotationItem> annotation,string book)
        {
            return annotation.Where(a => a.BookTittle == book).ToList();
        }

        private static void ImportLogic(BookProfile profile)
        {
            Console.Clear();
            string dbCardFileName;
            List<DbFileDescription> pretendents = new List<DbFileDescription>();
            WaitReaderConnection(profile, ref pretendents);

            dbCardFileName = pretendents.FirstOrDefault(p => p.Drive.VolumeLabel != profile.ReaderDriveLabel).FilePath;

            string basePath = Environment.CurrentDirectory;
            string logPath = Path.Combine(basePath, "Log.txt");
            List<string> logList = File.ReadAllLines(logPath).ToList();

            var annotationList = AnnotationReader.Read(dbCardFileName);

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

    public class MenuMaster
    {
        private Dictionary<int, string> menuDict;

        private int Result;

        public MenuMaster(IEnumerable<string> menuVariants)
        {
            menuDict = new Dictionary<int, string>();
            int i = 1;
            foreach(string variant in menuVariants)
            {
                menuDict.Add(i, variant);
                i++;
            }
        }

        public string GetAnswer(string header = "", bool clear = false)
        {
            if (clear)
                Console.Clear();

            Console.WriteLine(header);
            foreach(var variant in menuDict)
            {
                Console.WriteLine($"{variant.Key} - {variant.Value}");
            }
            Result = Int32.Parse(Console.ReadLine());

            return Answer;
        }

        public string Answer
        {
            get
            {
                return menuDict.First(i => i.Key == Result).Value;
            }
        }


    }
}
