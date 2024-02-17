using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotnetapp.Models
{
    public class OrderModel
    {
        public int orderId { get; set; }

        public string orderName { get; set; }
        public string orderDescription { get; set; }

        public List<ThemeModel> themeModel { get; set; }

        public GiftModel giftModel { get; set; } = new GiftModel();

        public DateTime orderDate { get; set; }
        public int orderPrice { get; set; }
        public string orderAddress { get; set; }
        public string orderPhone { get; set; }
        public string orderEmail { get; set; }

        public string orderQuantity { get; set; }
    }
}