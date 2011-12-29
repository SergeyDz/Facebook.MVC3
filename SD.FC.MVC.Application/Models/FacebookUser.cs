using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD.FC.MVC.Application.Models
{
    public class FacebookUser
    {
        public string FacebookId { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expires { get; set; }

        public string Name { get; set; }
    }
}