using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateFormat
{
    public class AnnotationProfile
    {
        public int id;
        public int contentId;
        public int type;
        public int addedDate;
        public int moddifiedDate;
        public int name;
        public int markedText;
        public int mark;
        public int markEnd;
        public int page;
        public int totalPage;
        public int mimeType;
        public int filePath;

        internal static AnnotationProfile GetProfile(BookProfileType targetType)
        {
            var result = new AnnotationProfile();
            if(targetType==BookProfileType.T2)
            {
                result.id = 0;
                result.contentId = 1;
                result.type = 2;
                result.addedDate = 3;
                result.moddifiedDate = 4;
                result.name = 5;
                result.markedText = 6;
                result.mark = 7;
                result.markEnd = 8;
                result.page = 9;
                result.totalPage = 10;
                result.mimeType = 11;
                result.filePath = 12;
            }
            return result;
        }
    }
}
