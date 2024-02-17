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
    public class UserController : ControllerBase
    {


        private readonly BusinessLayer bal = new BusinessLayer();

        [HttpPost]
        [Route("user/addUser")]
        public string addUser(UserModel user)
        {

            return bal.addUser(user);
        }
        [HttpDelete]
        [Route("user/delete/{UserID}")]
        public string deleteUser(string UserID)
        {
            return bal.deleteUser(UserID);
        }
        [HttpPut]
        [Route("user/update/{UserID}")]
        public string editUser(string UserID, UserModel user)
        {
            return bal.editUser(UserID, user);

        }
        [HttpGet]
        [Route("user/getList")]

        public List<UserModel> getUser()
        {
            return getUser();
        }
        [HttpGet]
        [Route("user/getuserbyemail")]
        public UserModel getUserByEmail(string email)
        {
            return bal.getUserByEmail(email);
        }
        [HttpPut]
        [Route("user/updatepassword")]
        public string UpdatePassword(LoginModel login)
        {
            return bal.UpdatePassword(login);
        }
        [HttpPut]
        [Route("user/updatemobilenumber")]
        public string updateMobileNumber(UserModel user)
        {
            return bal.updateMobileNumber(user);
        }
        [HttpPut]
        [Route("user/updateusername")]
        public string updateusername(UserModel user)
        {
            return bal.updateusername(user);
        }
    }
}