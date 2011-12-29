using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;

namespace SD.FC.MVC.Application.Models
{
    public class InMemoryUserStore
    {
        private static readonly ConcurrentBag<FacebookUser> Users = new ConcurrentBag<FacebookUser>();

        public static void AddOrUpdate(FacebookUser user)
        {
            var ux = Users.SingleOrDefault(u => u.FacebookId == user.FacebookId);

            if (ux == null)
            {
                Users.Add(user);
            }
            else
            {
                ux.AccessToken = user.AccessToken;
                ux.FacebookId = user.FacebookId;
                ux.Name = user.Name;
                ux.Expires = user.Expires;
            }
        }

        public static FacebookUser Get(string facebookId)
        {
            return Users.SingleOrDefault(u => u.FacebookId == facebookId);
        }
    }
}