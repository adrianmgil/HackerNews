namespace Adrian.Apis.HackerNews
{
    public interface IHackerNewsRepository
    {
        HackerNews Get(int id);
        int Create(int id, string title, string link);
        void Delete(int id);
    }

    public class HackerNewsRepository : IHackerNewsRepository
    {
        private const string key = "hackerNews";
        private readonly ICacheProvider cacheProvider;

        public HackerNewsRepository(ICacheProvider cacheProvider)
        {
            this.cacheProvider = cacheProvider;
        }


        public HackerNews Get(int id)
        {
            return cacheProvider.Get<HackerNews>($"{key}/{id}");
        }

        public int Create(int id, string title, string url)
        {
            cacheProvider.Set($"{key}/{id}", new HackerNews { Id = id, Title = title, Url = url });
            return id;
        }

        public void Delete(int id)
        {
            cacheProvider.Remove($"{key}/{id}");
        }
    }
}