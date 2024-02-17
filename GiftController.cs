using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dotnetapp.Models;
using System.Data.SqlClient;
using System.Data;
namespace dotnetapp.Controllers
{
    [ApiController]
    public class GiftController : ControllerBase
    {
        private readonly BusinessLayer bal = new BusinessLayer();

        [HttpGet]
        [Route("admin/getGift")]
        public IActionResult getAllGifts()
        {
            return Ok(bal.getAllGifts());
        }

        [HttpGet]
        [Route("admin/selectGift/{giftId}")]
        public GiftModel getGift(int giftId)
        {
            return bal.getGift(giftId);
        }

        [HttpPost]
        [Route("admin/addGift")]
        public string addGift(GiftModel data)
        {
            return bal.addGift(data);
        }

        [HttpPut]
        [Route("admin/editGift/{giftid}")]
        public string editGift(int giftid, GiftModel data)
        {
            return bal.editGift(giftid, data);
        }

        [HttpDelete]
        [Route("admin/deleteGift/{giftid}")]
        public string DeleteGift(int giftid)
        {
            return bal.DeleteGift(giftid);
        }
        [HttpPost]
        [Route("user/selectGift")]
        public string selectGift(GiftModel data)
        {
            return bal.selectGift(data);
        }
    }
}