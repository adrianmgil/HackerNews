using Microsoft.AspNetCore.Mvc;

namespace Adrian.Apis.HackerNews
{
    [Route("apis/[controller]")]
    public class HackerNewsController : Controller
    {
        private readonly IHackerNewsRepository repository;

        public HackerNewsController(ICacheProvider cacheProvider)
        {
            this.repository = new HackerNewsRepository(cacheProvider);
        }

        [HttpGet("[action]")]
        public HackerNews Item(int id)
        {
            return repository.Get(id);
        }

        [HttpPost("[action]")]
        public void Item([FromBody]HackerNews item)
        {
            repository.Create(item.Id, item.Title, item.Url);
        }

        // DELETE api/values/5
        [HttpDelete("[action]")]
        public void Delete(int id)
        {
            repository.Delete(id);
        }
    }
}
