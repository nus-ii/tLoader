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
            try
            {
                var profile = BookProfile.GetProfile(BookProfileType.T2);
                var l = new LionSupporter();
                OutputMaster outputMaster = new OutputMaster(@"C:\AllHarry\");

                MenuMaster mainMenu = new MenuMaster(new string[] {
                "Get new annotation",
                "Copy annotation to PC",
                "Analysis annotation",
                "Print Lingualeo Dictionary"
            });

                mainMenu.GetAnswer("", true);
                if (mainMenu.Answer == "Get new annotation")
                {
                    List<Tuple<string, string>> myEnDict = l.Words.Select(i => new Tuple<string, string>(i.Word, i.Translate)).ToList();
                    List<string> res = ImportLogic(profile, l.Words);

                    var dn = DateTime.Now;
                    string ps = $"{dn.Day}_{dn.Month}_{dn.Year}_{dn.Hour}-{dn.Minute}";

                    outputMaster.Print(res);
                    List<string> resultData = HtmlMaster.GetHtmlList(res);
                    outputMaster.Save(resultData, ps);
                }

                if (mainMenu.Answer == "Copy annotation to PC")
                {
                    MenuMaster.GetConfidentAnswer();
                }

                if (mainMenu.Answer == "Analysis annotation")
                { AnalysisLogic(@"C:\AllHarry\", outputMaster); }

                if (mainMenu.Answer == "Print Lingualeo Dictionary")
                {
                    outputMaster.Print(l.Words);
                    outputMaster.Save(l.Words, "dict.csv");

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message+" "+ex.StackTrace);
            }
            Console.WriteLine("All done!!!!!");
            Console.ReadLine();
        }

        private static void AnalysisLogic(string diretoryPath, OutputMaster outputMaster)
        {

            List<AnnotationItem> annotationList = GetAnnotationsFromAllDb(diretoryPath);

            var bookMenu = annotationList.Select(a => a.BookTittle).Distinct().ToList();
            bookMenu.Add("All");
            MenuMaster bookMenuMaster = new MenuMaster(bookMenu);

            string bookAnswer = bookMenuMaster.GetAnswer("Choice book for analysis", true);

            if (bookAnswer != "All")
                annotationList = FilterByBook(annotationList, bookAnswer);

            string bookShortName = GetShortName(bookAnswer);



            var uniq = UniqAnnotaion(annotationList);
            outputMaster.Save(uniq, "UniqAnnotaion" + "_" + bookShortName);

            var words = WordsAddings(annotationList);
            outputMaster.Save(words, "WordsRepeat" + "_" + bookShortName);

            var pages = PagesPerDay(annotationList);
            outputMaster.Save(pages, "PagesPerDay" + "_" + bookShortName);
                        

            for (int i = 3; i <= 6; i++)
            {
                var sameRoot = SameRoot(annotationList, i);
                outputMaster.Save(sameRoot, $"SameRoot_len{i}");
            }            
        }

        private static string GetShortName(string book)
        {
            book = book.ToLower().Trim();
            string temp = "";

            foreach (char c in book)
            {
                if (char.IsLetter(c) || c == ' ')
                {
                    temp += c.ToString();
                }
            }

            var splittedTemp = temp.Split(' ');
            string result = "";
            for (int i = splittedTemp.Count() - 1; i >= 0; i--)
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
                if (wordInfoList.Count == 0 || !wordInfoList.Any(w => w.value == a.CleanWord))
                {
                    wordInfoList.Add(new WordInfo(a.CleanWord));
                }
                else
                {
                    var w = wordInfoList.FirstOrDefault(i => i.value == a.CleanWord);
                    if (w != null)
                        w.number++;
                }
            }
            return wordInfoList;
        }


        private static List<WordInfo> SameRoot(List<AnnotationItem> annotationList, int rootLength = 4)
        {
            List<WordInfo> wordInfoList = WordsAddings(annotationList);

            var temp = wordInfoList.Where(w => w.value.Length >= rootLength);

            var preResult = temp.Where(w => temp.Where(i => i.value.Substring(0, rootLength) == w.value.Substring(0, rootLength)).Count() >= 4).ToList();

            preResult.Sort();

            return preResult;

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
            List<AnnotationItem> fullResult = new List<AnnotationItem>();

            DirectoryInfo dir = new DirectoryInfo(diretoryPath);
            var fileList = dir.GetFiles().Where(i => i.Name.Contains("book") && i.Name.Contains(".db"));
            foreach (var file in fileList)
            {
                fullResult.AddRange(AnnotationReader.Read(file.FullName));
            }

            fullResult = AnnotationItem.OnlyUnique(fullResult);


            return fullResult;
        }

        private static List<AnnotationItem> FilterByBook(List<AnnotationItem> annotation, string book)
        {
            return annotation.Where(a => a.BookTittle == book).ToList();
        }

        private static List<string> ImportLogic(BookProfile profile, List<LionWord> words)
        {
            List<AnnotationItem> annotationItems = TBookConnector.GetAnnotations(profile);
            List<string> res = AnnotationImporter.ImportLogic(annotationItems, words);



            return res;


            //string resultPath = Path.Combine(basePath, ps);
            //File.WriteAllLines(resultPath, res);
            //File.WriteAllLines(logPath, GetArray(logList));
        }




    }

    
}
