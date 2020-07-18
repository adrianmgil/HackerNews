using Adrian.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Adrian.Apis.HackerNews
{
    [Route("apis/[controller]")]
    public class HackerNewsController : Controller
    {
        private readonly IHackerNewsRepository repository;

        public HackerNewsController(ICacheProvider cacheProvider)
        {
            this.repository = new HackerNewsRepository(new HackerNewsDataAccess(new Database()), cacheProvider);
        }

        [HttpGet("[action]")]
        public IEnumerable<HackerNews> Get(string searchFor, int offset = 0, int linePerPage = 0)
        {
            return repository.Get(searchFor, offset, linePerPage);
        }

        [HttpPost("[action]")]
        public void Create([FromBody]HackerNews item)
        {
            repository.Create(item.Title, item.Link);
        }

        // DELETE api/values/5
        [HttpDelete("[action]")]
        public void Delete(int id)
        {
            repository.Delete(id);
        }
    }
}
