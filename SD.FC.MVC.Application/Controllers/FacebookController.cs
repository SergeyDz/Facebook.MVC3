using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SD.FC.MVC.Application.Models;
using Facebook;
using System.Web.Mvc;
using System.Web.Security;

namespace SD.FC.MVC.Application.Controllers
{
    public class FacebookController : Controller
    {
        //
        // GET: /Facebook/
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                var user = InMemoryUserStore.Get(User.Identity.Name);

                var fb = new FacebookClient(user.AccessToken);
                dynamic me = fb.Get("me");

                ViewBag.name = me.name;
            }
            catch (FacebookApiException)
            {
                FormsAuthentication.SignOut();
                return new HttpUnauthorizedResult();
            }

            return View();
        }

        [Authorize]
        public ActionResult Friends()
        {
            var user = InMemoryUserStore.Get(User.Identity.Name);
            var fb = new FacebookClient(user.AccessToken);
            JsonObject friendsData = fb.Get("/me/friends") as JsonObject;
            var data = friendsData["data"] as JsonArray;
            ViewBag.Friends = data;
            return View();
        }

        [Authorize]
        public ActionResult PostToWall(FormCollection formCollection)
        {
            List<string> results = new List<string>();
            var user = InMemoryUserStore.Get(User.Identity.Name);
            var fb = new FacebookClient(user.AccessToken);
            
            var friends = formCollection["chkFriends"];
            if (!string.IsNullOrEmpty(friends))
            {


                string[] users = friends.Split(',');
                foreach (var friend in users)
                {
                    if (!string.IsNullOrEmpty(friend))
                    {
                        dynamic fbFriendValue = fb.Get(string.Format("{0}?fields=id,name", friend));
                        results.Add(fbFriendValue.name);
                    }
                }
            }

            ViewBag.Results = results;
            return View();
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (InMemoryUserStore.Get(User.Identity.Name) == null)
                filterContext.Result = new HttpUnauthorizedResult();
        }

    }
}