using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
    public class MenuMaster
    {
        private Dictionary<int, string> menuDict;

        private int Result;

        public MenuMaster(IEnumerable<string> menuVariants)
        {
            menuDict = new Dictionary<int, string>();
            int i = 1;
            foreach (string variant in menuVariants)
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
            foreach (var variant in menuDict)
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


        public static bool GetConfidentAnswer()
        {
            int aDigit=1;
            int bDigit=10;
            int sumDigit;
            int answerDigit;
            DateTime aDate = DateTime.Now;
            Random rhd = new Random(aDate.Hour);

            aDigit = rhd.Next(50,200);

            for(int i=aDate.Minute;i>-10;i--)
            {
                bDigit = rhd.Next(10,75);
            }

            sumDigit = aDigit + bDigit;

            Console.Write($"If you sure, input answer {aDigit}+{bDigit}=");
            string answer = Console.ReadLine();

            if(Int32.TryParse(answer,out answerDigit)&&answerDigit==sumDigit)
            {
                Console.WriteLine("Ok...");
                return true;
            }

            return false;            
        }

    }
}
