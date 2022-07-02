﻿using BuildABand.DAL;
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
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MessageDAL messageSource;

        public MessageController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.messageSource = new MessageDAL(configuration);
        }

        // <summary>
        /// Gets Musician Conversation
        /// GET: api/message/ConversationID
        /// </summary>
        /// <param name="ConversationID"></param>
        /// <returns>JsonResult with message</returns
        [HttpGet("{ConversationID}")]
        public JsonResult GetMessageByConversationID(int ConversationID)
        {
            if (ConversationID < 0)
            {
                throw new ArgumentException("Invalid Conversation request");
            }

            try
            {
                return new JsonResult(
                    this.messageSource.GetMessageByConversationID(ConversationID));
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);
            }
        }


        // Post: api/Message
        // Post new Messsage
        [HttpPost]
        public JsonResult AddMessage(Message message)
        {
            if (message == null)
                throw new ArgumentException("Invalid arguement");
            try
            {
                this.messageSource.AddMessage(message);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

            return new JsonResult(message);
        }
    }
}