using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProtinTrackerRedisDemo.Models;
using ServiceStack.Redis;

namespace ProtinTrackerRedisDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (IRedisClient client = new RedisClient())
            {
                var userClient = client.As<User>();
                var users = userClient.GetAll();
                var userSelection = new SelectList(users, "Id", "Name", string.Empty);
                ViewBag.UserId = userSelection;

            }
            return View();
        }
    }
}