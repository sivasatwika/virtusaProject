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
using System.Xml.Linq;
namespace dotnetapp.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {


        private readonly BusinessLayer bal = new BusinessLayer();


        [HttpPost]
        [Route("user/addOrdersCart")]
        public string addOrdersCart(OrderModel order)
        {
            return bal.addOrdersCart(order);

        }
        [HttpPost]
        [Route("user/addOrders")]
        public string addOrders([FromBody] string userEmail)
        {

            return bal.addOrders(userEmail);
        }

        [HttpGet]
        [Route("user/getOrdersCart")]
        public IActionResult viewPlacedOrders(string userEmail)
        {

            return bal.viewPlacedOrders(userEmail);
        }

        [HttpGet]
        [Route("user/getOrderAllDetails")]
        public List<OrderModel> viewOrders(string userEmail)
        {
            return bal.viewOrders(userEmail);
        }

        [HttpGet]
        [Route("user/getOrdersCartByEmail/{orderId}")]
        public OrderModel viewPlacedOrderBYEmail(int orderId)
        {
            return bal.viewPlacedOrderBYEmail(orderId);
        }


        [HttpPut]
        [Route("user/editOrder/{orderId}")]
        public string editOrder(int orderId, OrderModel order)
        {
            return bal.editOrder(orderId, order);
        }
        [HttpDelete]
        [Route("user/deleteOrder/{orderId}")]
        public string deleteOrder(int orderId)
        {
            return bal.deleteOrder(orderId);
        }

        [HttpGet]
        [Route("user/getallorders")]
        public IActionResult MyOrders(string userEmail)
        {

            return bal.MyOrders(userEmail);
        }


    }
}