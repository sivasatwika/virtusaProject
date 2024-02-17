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
    public class AuthController : ControllerBase
    {
        private readonly BusinessLayer bal = new BusinessLayer();

        [HttpPost]
        [Route("user/login")]
        public IActionResult isUserPresent(LoginModel lm)
        {
            return Created("User", bal.isUserPresent(lm));
        }
        [HttpPost]
        [Route("admin/login")]
        public string isAdminPresent(LoginModel lm)
        {
            return bal.isAdminPresent(lm);
        }
        [HttpPost]
        [Route("user/signup")]
        public IActionResult saveUser(UserModel user)
        {
            return Created("SignUp", bal.saveUser(user));

        }
        [HttpPost]
        [Route("admin/signup")]
        public string saveAdmin(UserModel user)
        {
            return bal.saveAdmin(user);
        }
    }
}