using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scraper.Net.Facebook.Tests
{
    [TestClass]
    public class FacebookScraperTests
    {
        private const string User = "ayelet.benshaul.shaked";
        private static FacebookScraper _scraper;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _scraper = new FacebookScraper(
                new FacebookScraperConfig());
        }

        [TestMethod]
        public async Task TestGetPostsAsync()
        {
            IEnumerable<Post> posts = await _scraper.GetPostsAsync(User);
            List<Post> list = posts.ToList();

            Assert.IsNotNull(list);
            CollectionAssert.AllItemsAreNotNull(list);
        }
    }
}