using Adrian.Apis.HackerNews;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


// Useless this time but I am showing you guys my coding skill.

namespace Adrian.DataAccess
{
    public interface IHackerNewsDataAccess
    {
        IEnumerable<HackerNews> Get(string searchFor, int offset, int linePerPage);
        int Create(string title, string link);
        void Delete(int id);
    }

    public class HackerNewsDataAccess : IHackerNewsDataAccess
    {
        private readonly IDatabase db;

        public HackerNewsDataAccess(IDatabase db)
        {
            this.db = db;
        }

        public IEnumerable<HackerNews> Get(string searchFor, int offset, int linePerPage)
        {
            string sql = $@"SELECT HackerNew_RecID AS Id, Title, Link, Last_Update_UTC AS LastUpdateUtc
                            FROM dbo.HackerNew                                    
                            {(string.IsNullOrEmpty((searchFor ?? "").Trim()) ? "" : $"WHERE Title LIKE '%{searchFor.Trim()}%'")}
                            ORDER BY Last_Update_UTC DESC";
            IEnumerable<HackerNews> results = db.ExecuteQuery<HackerNews>(sql);

            if (offset > 0)
            {
                results = results.Skip(offset);
            }
            if (linePerPage > 0)
            {
                results = results.Take(linePerPage);
            }
            return results;
        }

        public int Create(string title, string link)
        {
            const string sql = @"INSERT INTO dbo.HackerNew (Title, Link) VALUES (@title, @link)";
            return db.Insert(sql, new[] { new SqlParameter("title", title), new SqlParameter("link", link) });
        }

        public void Delete(int id)
        {
            const string sql = @"DELETE dbo.HackerNew WHERE HackerNew_RecID = @id";
            db.ExecuteNonQuery(sql, new[] { new SqlParameter("id", id) });
        }
   }
}