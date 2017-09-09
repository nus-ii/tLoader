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
		public static List<AnnotationItem> Read(string path, BookProfile profile)
		{
			#region MyRegion
			SQLiteConnection m_dbConn;
			SQLiteCommand m_sqlCmd;
			m_dbConn = new SQLiteConnection();
			m_sqlCmd = new SQLiteCommand();
			m_dbConn = new SQLiteConnection("Data Source=" + path + ";Version=3;");
			m_dbConn.Open();
			m_sqlCmd.Connection = m_dbConn;
			var sqlQuery = "SELECT * FROM annotation";
			SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
			DataTable dTable = new DataTable();
			adapter.Fill(dTable);
			#endregion

			List<AnnotationItem> annotationList = new List<AnnotationItem>();

			foreach (DataRow r in dTable.Rows)
			{
				var item = AnnotationItem.GetItem(r, profile);
				annotationList.Add(item);
			}

			return annotationList;
		}
	}
}
