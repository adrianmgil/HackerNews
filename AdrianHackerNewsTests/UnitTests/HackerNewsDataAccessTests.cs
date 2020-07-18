using AdrianHackerNews.DataAccess;
using NUnit.Framework;

namespace AdrianHackerNews.UnitTests
{
    [TestFixture]
    public class HackerNewsDataAccessTests
    {
        private IDatabase db;
        private readonly IHackerNewsDataAccess dataAccess;

        public HackerNewsDataAccessTests()
        {
            db = new Database();
            dataAccess = new HackerNewsDataAccess(db);
        }

        [SetUp]
        public void setup()
        {
        }

        [Test]
        public void TestGet()
        {
            Assert.DoesNotThrow(() => dataAccess.Get(0, 0));
        }
    }
}