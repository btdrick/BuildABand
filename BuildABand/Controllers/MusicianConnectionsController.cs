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
    public class MusicianConnectionsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MusicianConnectionDAL connectionSource;

        public MusicianConnectionsController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.connectionSource = new MusicianConnectionDAL(configuration);
        }


        /// <summary>
        /// Gets Musician Connections
        /// GET: api/MusicianConnections/MusicianID
        /// </summary>
        /// <param name="MusicianID"></param>
        /// <returns>JsonResult with connections</returns
        [HttpGet("{MusicianID}")]
        public JsonResult GetMusicianConnections(int MusicianID)
        {
            try
            {
                return new JsonResult(
                    this.connectionSource.GetMusicianConnectionsByID(MusicianID));
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);
            }
        }
    }
}