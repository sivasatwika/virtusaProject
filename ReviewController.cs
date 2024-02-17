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


    public class ReviewController : ControllerBase
    {

        private readonly BusinessLayer bal = new BusinessLayer();

        [HttpGet]
        [Route("getreview")]
        public List<ReviewModel> GetReviews()
        {
            return bal.GetReviews();
        }
        // POST: api/Review
        [HttpPost]
        [Route("insertreview")]
        public string Post(ReviewModel r)
        {
            return bal.Postreview(r);
        }
    }
}