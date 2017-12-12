using System;
using System.Web.Mvc;
using ProtinTrackerRedisDemo.Models;
using ServiceStack.Redis;

namespace ProtinTrackerRedisDemo.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult NewUser()
        {
            return View();

        }

        public ActionResult Save(string userName, int goal, long? userId)
        {
            using (IRedisClient client = new RedisClient())
            {
                var userClient = client.As<User>();
                User user;
                if (userId != null)
                {
                    user = userClient.GetById(userId);
                    client.RemoveItemFromSortedSet("urn:leaderboard", user.Name);
                }
                else
                {
                    user = new User
                    {
                        Id = userClient.GetNextSequence()
                    };

                }

                user.Name = userName;
                user.Goal = goal;
                userId = user.Id;
                userClient.Store(user);
                client.AddItemToSortedSet("urn:leaderboard", user.Name, user.Total);
            }
            return RedirectToAction("Index", "Tracker", new { userId = userId });
        }

        public ActionResult Edit(long userId)
        {
            using (IRedisClient client = new RedisClient())
            {
                var userClient = client.As<User>();
                var user = userClient.GetById(userId);
                ViewBag.UserName = user.Name;
                ViewBag.Goal = user.Goal;
                ViewBag.UserId = user.Id;
            }
            return View("NewUser");
        }
    }
}