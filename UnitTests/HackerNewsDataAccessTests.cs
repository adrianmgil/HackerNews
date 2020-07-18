using Adrian;
using Adrian.DataAccess;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class HackerNewsDataAccessTests
    {
        private Database db;
        private readonly HackerNewsDataAccess dataAccess;

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
            Assert.DoesNotThrow(() => dataAccess.Get("", 0, 0));
        }
    }
}