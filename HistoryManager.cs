using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace PandaLyrics
{
    internal class HistoryManager
    {
        SQLiteConnection connection;
        public HistoryManager()
        {
            string currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string sqlFilePath = Path.Combine(currentPath, "data.db");

            if (!File.Exists(sqlFilePath))
            {
                SQLiteConnection.CreateFile(sqlFilePath);
            }

            connection = new SQLiteConnection($"Data Source={sqlFilePath}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS HISTORY (songID TEXT PRIMARY KEY, lyricID INT)";
            command.ExecuteNonQuery();

        }

        public void Set(string songID, int lyricID)
        {
            var command = connection.CreateCommand();
            command.CommandText = @"INSERT OR REPLACE INTO HISTORY(songID, lyricID) VALUES (@songID, @lyricID)";
            command.Parameters.Add(new SQLiteParameter("@songID", songID));
            command.Parameters.Add(new SQLiteParameter("@lyricID", lyricID));
            command.ExecuteNonQuery();
        }

        public int? Get(string songID)
        {
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT lyricID FROM HISTORY WHERE songID = @songID";
            command.Parameters.Add(new SQLiteParameter("@songID", songID));
            var result = command.ExecuteScalar();
            if (result == null)
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(result);
            }
        }
    }
}
