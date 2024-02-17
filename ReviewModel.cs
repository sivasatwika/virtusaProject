using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotnetapp.Models
{

    public class ReviewModel
    {
        public int orderId { get; set; }
        public string name { get; set; }

        public string comments { get; set; }
    }

}