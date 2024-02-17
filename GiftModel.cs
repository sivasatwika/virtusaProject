using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotnetapp.Models
{
    public class GiftModel
    {
        public int giftId { get; set; }
        public string giftName { get; set; }
        public int giftPrice { get; set; }
        public string GiftImageUrl { get; set; }
        public string giftQuantity { get; set; }
        public string giftDetails { get; set; }
    }
}