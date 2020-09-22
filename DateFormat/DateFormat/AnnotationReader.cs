using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Management;

namespace DateFormat
{
    public class AnnotationReader
    {
        public static List<AnnotationItem> Read(string path)
        {
            
            SQLiteConnection m_dbConn;
            SQLiteCommand m_sqlCmd;
            m_dbConn = new SQLiteConnection();
            m_sqlCmd = new SQLiteCommand();
            m_dbConn = new SQLiteConnection("Data Source=" + path + ";Version=3;");
            m_dbConn.Open();
            m_sqlCmd.Connection = m_dbConn;
            var sqlQuery = "SELECT a.*,b.title FROM annotation a LEFT JOIN books b ON a.content_id=b._id";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
            DataTable dTable = new DataTable();
            adapter.Fill(dTable);

            List<AnnotationItem> annotationList = new List<AnnotationItem>();

            foreach (DataRow r in dTable.Rows)
            {
                var item = GetAnnotation(r);
                annotationList.Add(item);
            }

            return annotationList;
        }

        public static AnnotationItem GetAnnotation(DataRow row)
        {
            return new AnnotationItem
            {
                MarkedText = row.Get("marked_text"),
                AddedDate = UHelper.UDateFormat(row.Get("added_date")),
                Page = Convert.ToInt32(row.Get("page")),
                BookTittle = row.Get("title")
            };
        }
    }

    public static class SqlHelper
    {
        public static string Get(this DataRow row, string header)
        {
            string result = "";
            result = row.ItemArray[row.Table.Columns.IndexOf(header)].ToString();
            return result;
        }
    }
}
