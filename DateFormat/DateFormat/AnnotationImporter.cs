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
        public static List<string> Import(List<AnnotationItem> newAnnotations, List<LionWord> words, ref List<string> exWord)
        {
            List<AnnotationItem> annotationList = new List<AnnotationItem>();

            foreach (AnnotationItem item in newAnnotations)
            {
                if (annotationList.All(a => a.CleanWord != item.CleanWord) && !exWord.Contains(item.CleanWord))
                {
                    annotationList.Add(item);
                    exWord.Add(item.CleanWord);
                }
            }

            annotationList.Sort();

            List<AnnotationItem> withoutTranslate = Separate(annotationList, words, out List<LionWord> forgotten);

            List<string> endings = new List<string> { "ing", "ed", "es", "ly", "ful", "les" };

            List<Tuple<string, List<AnnotationItem>>> separetedByEnding = SepareteByEnding<AnnotationItem>(withoutTranslate, endings);

            List<Tuple<string, List<LionWord>>> separetedByEndingForgotten = SepareteByEnding<LionWord>(forgotten, endings);

            string blankHeader = "---------------------------------------------------";

            List<string> resultData = new List<string>();

            foreach (var i in separetedByEnding)
            {
                resultData.Add(blankHeader + i.Item1);
                resultData.AddRange(i.Item2.Select(n => n.MarkedText));
            }

            foreach (var i in separetedByEndingForgotten)
            {
                resultData.Add(blankHeader + i.Item1);
                resultData.AddRange(i.Item2.Select(n => n.CleanWord + " - " + n.Translate));
            }

            return resultData;
        }

        private static List<Tuple<string, List<T>>> SepareteByEnding<T>(List<T> words, List<string> endings) where T : Word
        {

            List<Tuple<string, List<T>>> result = new List<Tuple<string, List<T>>>();

            string otherHeader = "NO: " + string.Join("_", endings);

            var t = words.Where(w => endings.All(e => !w.CleanWord.EndsWith(e)));
            var ec = t.ToList();

            result.Add(new Tuple<string, List<T>>(otherHeader, ec));
            foreach (string ending in endings)
            {
                result.Add(new Tuple<string, List<T>>(ending, words.Where(w => w.CleanWord.EndsWith(ending)).ToList()));
            }

            return result;
        }

        private static List<AnnotationItem> Separate(List<AnnotationItem> annotationList,
            List<LionWord> dict,
            out List<LionWord> forgotten)
        {
            List<AnnotationItem> result = new List<AnnotationItem>();
            forgotten = new List<LionWord>();

            foreach (AnnotationItem item in annotationList)
            {
                LionWord dictItem = dict.FirstOrDefault(i => i.CleanWord == item.CleanWord);

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
