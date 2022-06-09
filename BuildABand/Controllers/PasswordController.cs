using BuildABand.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.Controllers
{
    // <summary>
    /// This class serves as the controller,
    /// for hashing password
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PasswordController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/GetSha256Hash/password
        // Get password hash
        [HttpGet("{password}")]
        public JsonResult GetSha256Hash(string password)
        {
            return new JsonResult( PasswordHash.GetSha256Hash(password));
        }
    }
}
