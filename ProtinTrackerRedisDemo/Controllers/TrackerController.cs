using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProtinTrackerRedisDemo.Models;
using ServiceStack.Redis;

namespace ProtinTrackerRedisDemo.Controllers
{
    public class TrackerController : Controller
    {
        // GET: Tracker
        public ActionResult Index(long userId, int amount = 0)
        {
            using (IRedisClient client = new RedisClient())
            {
                var userClient = client.As<User>();
                var user = userClient.GetById(userId);
                var historyclient = client.As<int>();
                var hisotryList = historyclient.Lists[$"urn:history:{userId}"];

                if (amount > 0)
                {
                    user.Total += amount;
                    userClient.Store(user);


                    hisotryList.Prepend(amount);
                    hisotryList.Trim(0, 4);

                    client.AddItemToSortedSet("urn:leaderboard", user.Name, user.Total);
                }

                ViewBag.HistoryItems = hisotryList.GetAll();

                ViewBag.UserName = user.Name;
                ViewBag.Total = user.Total;
                ViewBag.Goal = user.Goal;
                ViewBag.UserId = user.Id;
            }
            return View();
        }
    }
}