using System.Web.Mvc;
using ServiceStack.Redis;

namespace ProtinTrackerRedisDemo.Controllers
{
    public class LeaderboardController : Controller
    {
        public ActionResult Index()
        {
            using (IRedisClient client=new RedisClient())
            {
                var leaders = client.GetAllWithScoresFromSortedSet("urn:leaderboard");
                ViewBag.Leaders = leaders;
            }
            return View();
        }
    }
}