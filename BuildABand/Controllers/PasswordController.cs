using BuildABand.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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

        /// <summary>
        /// 1-param constructor
        /// </summary>
        /// <param name="configuration"></param>
        public PasswordController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Gets password hash
        /// GET: api/GetSha256Hash/password
        /// </summary>
        /// <param name="password"></param>
        /// <returns>JsonResult with hash</returns>
        [HttpGet("{password}")]
        public JsonResult GetSha256Hash(string password)
        {
            return new JsonResult( PasswordHash.GetSha256Hash(password));
        }
    }
}
