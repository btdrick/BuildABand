using BuildABand.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace BuildABand.Controllers
{
    /// <summary>
    /// This class serves as the controller
    /// for data related to States table in DB.
    /// It is a mediator between the front-end 
    /// and data access layer for State media.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly StatesDAL stateSource;

        public StatesController(IConfiguration configuration)
        {
            this._configuration = configuration;
            this.stateSource = new StatesDAL(_configuration);
        }

        [HttpGet]
        //Get: api/states
        //Get states
        public JsonResult GetState()
        {
            try
            {
               return new JsonResult(this.stateSource.GetStates());
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);
            }
        }
    }
}
