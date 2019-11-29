using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
    /// <summary>
    /// Класс поллучения аннотаций из книги
    /// </summary>
    public class TBookConnector
    {
        public static List<AnnotationItem> GetAnnotations(BookProfile profile)
        {
            string dbCardFileName;
            List<DbFileDescription> pretendents = new List<DbFileDescription>();
            WaitReaderConnection(profile, ref pretendents);

            dbCardFileName = pretendents.FirstOrDefault(p => p.Drive.VolumeLabel != profile.ReaderDriveLabel).FilePath;
            List<AnnotationItem> preAnnotationList = AnnotationReader.Read(dbCardFileName);
            return preAnnotationList;
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
    }
}
