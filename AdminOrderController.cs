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
    public class AdminOrderController : ControllerBase
    {
        private readonly BusinessLayer bal = new BusinessLayer();

        [HttpGet]
        [Route("admin/getAllOrders")]
        public IActionResult viewOrder()
        {

            return bal.viewOrder();
        }
        [HttpDelete]
        [Route("admin/deleteOrder")]
        public string AdminDeleteOrder([FromBody] int orderID)
        {
            return bal.AdminDeleteOrder(orderID);
        }
    }
}