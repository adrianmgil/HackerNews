using Adrian;
using Adrian.Apis.HackerNews;
using Adrian.DataAccess;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    [TestFixture]
    public class HackerNewRepositoryTests
    {
        HackerNewsRepository repo;
        private Mock<IHackerNewsDataAccess> mockDataAccess;
        private Mock<ICacheProvider> mockCacheProvider;

        [SetUp]
        public void setup()
        {
            mockDataAccess = new Mock<IHackerNewsDataAccess>();
            mockCacheProvider = new Mock<ICacheProvider>();

            repo = new HackerNewsRepository(mockDataAccess.Object, mockCacheProvider.Object);
        }

        [Test]
        public void testGetCache()
        {
            mockCacheProvider.Setup(x => x.Get<IEnumerable<HackerNews>>(It.IsAny<string>())).Returns((IEnumerable<HackerNews>)null);
            repo.Get("", 0, 0);
            mockDataAccess.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            mockCacheProvider.Setup(x => x.Get<IEnumerable<HackerNews>>(It.IsAny<string>())).Returns(new List<HackerNews>());
            repo.Get("", 0, 0);
            mockDataAccess.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}
