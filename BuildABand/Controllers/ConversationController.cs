﻿using BuildABand.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace BuildABand.Controllers
{
    /// <summary>
    /// This class serves as the controller
    /// for data related to Conversation table in DB.
    /// It is a mediator between the front-end 
    /// and data access layer for Conversation media.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ConversationDAL conversationSource;

        /// <summary>
        /// 1-param constructor
        /// </summary>
        /// <param name="configuration"></param>
        public ConversationController(IConfiguration configuration)
        {
            _configuration = configuration;
            this.conversationSource = new ConversationDAL(configuration);
        }


        /// <summary>
        /// Gets Musician Conversation
        /// GET: api/Conversation/MusicianID
        /// </summary>
        /// <param name="MusicianID"></param>
        /// <returns>JsonResult with Conversation</returns
        [HttpGet("{MusicianID}")]
        public JsonResult GetConversationByMusicianID(int MusicianID)
        {
            if (MusicianID < 0)
            {
                throw new ArgumentException("Invalid Conversation request");
            }

            try
            {
                return new JsonResult(
                    this.conversationSource.GetConversationByMusicianID(MusicianID));
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);
            }
        }

        /// <summary>
        ///   Get Conversation by both SenderID and ReceiverID 
        ///   Get: api/conversation/SenderID/ReceiverID
        /// </summary>
        /// <param name="SenderID"></param>
        /// <param name="ReceiverID"></param>
        /// <returns></returns>
        [HttpGet("{SenderID}/{ReceiverID}")]
        public JsonResult GetConversationBySenderIDReceiverID(int SenderID, int ReceiverID)
        {
            if (SenderID < 0 || ReceiverID < 0)
            {
                throw new ArgumentException("Invalid musician");
            }

            try
            {
                return new JsonResult(
                    this.conversationSource.GetConversationBySenderIDReceiverID(SenderID,ReceiverID));
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);
            }
        }


        /// <summary>
        ///   Post new Conversation 
        ///   post: api/conversation/SenderID/ReceiverID
        /// </summary>
        /// <param name="SenderID"></param>
        /// <param name="ReceiverID"></param>
        /// <returns></returns>
        [HttpPost("{SenderID}/{ReceiverID}")]
        public JsonResult AddConversation(int SenderID, int ReceiverID)
        {
            if (SenderID < 0 || ReceiverID < 0)
            {
                throw new ArgumentException("Invalid musician");
            }

            try
            {
                this.conversationSource.AddConversation(SenderID, ReceiverID);
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);
            }


            return new JsonResult("Conversation added");
        }



    }
}
