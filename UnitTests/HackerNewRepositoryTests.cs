using Adrian;
using Adrian.Apis.HackerNews;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    [TestFixture]
    public class HackerNewRepositoryTests
    {
        HackerNewsRepository repo;
        private Mock<ICacheProvider> mockCacheProvider;

        [SetUp]
        public void setup()
        {
            mockCacheProvider = new Mock<ICacheProvider>();

            repo = new HackerNewsRepository(mockCacheProvider.Object);
        }

        [Test]
        public void testGetCache()
        {
            mockCacheProvider.Setup(x => x.Get<IEnumerable<HackerNews>>(It.IsAny<string>())).Returns((IEnumerable<HackerNews>)null);
            Assert.DoesNotThrow(() => repo.Get(0));
        }
    }
}
