using Adrian.DataAccess;
using System.Collections.Generic;

namespace Adrian.Apis.HackerNews
{
    public interface IHackerNewsRepository
    {
        IEnumerable<HackerNews> Get(string searchFor, int offset, int linePerPage);
        int Create(string title, string link);
        void Delete(int id);
    }

    public class HackerNewsRepository : IHackerNewsRepository
    {
        private const string key = "hackerNews";
        private readonly IHackerNewsDataAccess dataAccess;
        private readonly ICacheProvider cacheProvider;

        public HackerNewsRepository(IHackerNewsDataAccess dataAccess, ICacheProvider cacheProvider)
        {
            this.dataAccess = dataAccess;
            this.cacheProvider = cacheProvider;
        }

        public IEnumerable<HackerNews> Get(string searchFor, int offset, int linePerPage)
        {
            string cacheKey = $"{key}/{offset}/{linePerPage}{(string.IsNullOrEmpty((searchFor ?? "").Trim()) ? "" : $"/{searchFor}")}";
            IEnumerable<HackerNews> results = cacheProvider.Get<IEnumerable<HackerNews>>(cacheKey);
            if (results == null)
            {
                results = dataAccess.Get(searchFor, offset, linePerPage);
                if (results != null)
                {
                    cacheProvider.Set(cacheKey, results);
                }
            }
            return results;
        }

        public int Create(string title, string link)
        {
            return dataAccess.Create(title, link);
        }

        public void Delete(int id)
        {
            dataAccess.Delete(id);
        }
    }
}