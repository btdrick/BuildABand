﻿using BuildABand.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildABand.Controllers
{
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