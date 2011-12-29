using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SD.FC.MVC.Application.Models;
using Facebook;
using System.Web.Mvc;
using System.Web.Security;
using System.Dynamic;

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
            List<ExpandoObject> results = new List<ExpandoObject>();
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
                        string path = string.Format(@"/{0}/feed", friend);
                        Dictionary<string, object> parameters = new Dictionary<string,object>();
                        parameters.Add("name", formCollection["Name"]);
                        parameters.Add("link", formCollection["Link"]);
                        parameters.Add("picture", formCollection["Picture"]);
                        parameters.Add("message", formCollection["Message"]);
                        parameters.Add("caption", formCollection["Caption"]);
                        dynamic fbFeedValue = fb.Post(path, parameters);
                        dynamic data = new ExpandoObject();
                        data.FriendName = fbFriendValue.name;
                        data.ArticleId = 1;
                        results.Add(data);
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