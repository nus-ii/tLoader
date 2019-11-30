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

        public static List<string> ImportLogic(List<AnnotationItem> annotationItems, List<LionWord> words)
        {
            List<AnnotationItem> annotationList = new List<AnnotationItem>();

            foreach (AnnotationItem p in annotationItems)
            {
                if (annotationList.All(a => a.CleanWord != p.CleanWord))
                    annotationList.Add(p);
            }

            annotationList.Sort();

            List<AnnotationItem> withoutTranslate = Separate(annotationList, words, out List<LionWord> forgotten);

            List<string> endings = new List<string> { "ing", "ed", "es", "ly", "ful", "les" };

            List<Tuple<string, List<AnnotationItem>>> separetedByEnding = SepareteByEnding<AnnotationItem>(withoutTranslate, endings);

            List<Tuple<string, List<LionWord>>> separetedByEndingForgotten = SepareteByEnding<LionWord>(forgotten, endings);

            string blankHeader = "---------------------------------------------------";

            List<string> resultData = new List<string>();

            foreach(var i in separetedByEnding)
            {
                resultData.Add(blankHeader + i.Item1);
                resultData.AddRange(i.Item2.Select(n=>n.MarkedText));
            }

            foreach (var i in separetedByEndingForgotten)
            {
                resultData.Add(blankHeader + i.Item1);
                resultData.AddRange(i.Item2.Select(n => n.CleanWord+" - "+n.Translate));
            }

            return resultData;

            ////---------------------
            //List<Tuple<string, string>> myEnDict = words.Select(i => new Tuple<string, string>(i.Word, i.Translate)).ToList();
            //return ImportLogic(annotationItems, myEnDict);
        }

        //public static List<string> ImportLogic(List<AnnotationItem> annotationItems, List<Tuple<string,string>> dict)
        //{            
            

        //    ////string basePath = Environment.CurrentDirectory;
        //    ////string logPath = Path.Combine(basePath, "Log.txt");
        //    ////List<string> logList = File.ReadAllLines(logPath).ToList();

            
        //    //List<AnnotationItem> annotationList = new List<AnnotationItem>();           
 
        //    //foreach(AnnotationItem p in annotationItems)
        //    //{
        //    //    if (annotationList.All(a => a.CleanWord != p.CleanWord))
        //    //        annotationList.Add(p);
        //    //}

        //    //annotationList.Sort();

        //    //List<AnnotationItem> withoutTranslate = Separate(annotationList,dict,out List<Tuple<string,string>> forgotten);

        //    //List<string> resultData = new List<string>();           

        //    //List<Tuple<string, List<AnnotationItem>>> separetedByEnding = separeteByEnding(withoutTranslate,new List<string> {"ing","ed","es","ly","ful","les"});

        //    //foreach(var bigT in separetedByEnding)
        //    //{
        //    //    resultData.Add("----------------------------------------------" + bigT.Item1);
        //    //    resultData.AddRange(bigT.Item2.Select(a=>a.MarkedText));
        //    //}

        //    //resultData.Add("----------------------------------------------forgotten");

        //    //resultData.AddRange(forgotten.Select(i=>$"{i.Item1} - {i.Item2}"));

        //    //return resultData;
        //}

        //private static List<Tuple<string, List<AnnotationItem>>> SepareteByEnding(List<AnnotationItem> annotations, List<string> list)
        //{

        //    //List<Tuple<string, List<AnnotationItem>>> result = new List<Tuple<string, List<AnnotationItem>>>
        //    //{
        //    //    new Tuple<string, List<AnnotationItem>>("other", annotations.Where(a => list.All(e => !a.CleanWord.EndsWith(e))).ToList())
        //    //};
        //    //foreach (string ending in list)
        //    //{
        //    //    result.Add(new Tuple<string, List<AnnotationItem>>(ending,annotations.Where(a=>a.CleanWord.EndsWith(ending)).ToList()));
        //    //}           

        //    //return result;
        //}

        private static List<Tuple<string, List<T>>> SepareteByEnding<T>(List<T> words, List<string> endings) where T:Word
        {

            List<Tuple<string, List<T>>> result = new List<Tuple<string, List<T>>>();

            string otherHeader = "NO: " + string.Join("_", endings);

            var t=words.Where(w => endings.All(e => !w.CleanWord.EndsWith(e)));
            var ec = t.ToList();

            result.Add(new Tuple<string, List<T>>(otherHeader, ec));
            foreach (string ending in endings)
            {
                result.Add(new Tuple<string, List<T>>(ending, words.Where(w => w.CleanWord.EndsWith(ending)).ToList()));
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
        //private static List<AnnotationItem> Separate(List<AnnotationItem> annotationList, 
        //    List<Tuple<string, string>> dict, 
        //    out List<Tuple<string, string>> forgotten)
        //{
        //    //List<AnnotationItem> result = new List<AnnotationItem>();
        //    //forgotten = new List<Tuple<string, string>>();

        //    //List<Tuple<string, string>> normalDict = dict.Select(i => new Tuple<string, string>(i.Item1.ToLower().Trim(), i.Item2)).ToList();

        //    //foreach(AnnotationItem item in annotationList)
        //    //{
        //    //    var t = normalDict.FirstOrDefault(i => i.Item1 == item.MarkedText.ToLower().Trim());
        //    //    if(t!=null)
        //    //    {
        //    //        forgotten.Add(t);
        //    //    }
        //    //    else
        //    //    {
        //    //        result.Add(item);
        //    //    }
        //    //}

        //    //return result;
        //}

        private static List<AnnotationItem> Separate(List<AnnotationItem> annotationList,
            List<LionWord> dict,
            out List<LionWord> forgotten)
        {
            List<AnnotationItem> result = new List<AnnotationItem>();
            forgotten = new List<LionWord>();      

            foreach (AnnotationItem item in annotationList)
            {
                LionWord dictItem = dict.FirstOrDefault(i=>i.CleanWord==item.CleanWord);

                if (dictItem == null)
                {
                    result.Add(item);
                }
                else
                {
                    forgotten.Add(dictItem);
                }
            }

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
