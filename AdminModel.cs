using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotnetapp.Models
{
    public class AdminModel
    {
        public String email { get; set; }
        public String password { get; set; }
        public String mobileNumber { get; set; }
        public String userRole { get; set; }
    }
}