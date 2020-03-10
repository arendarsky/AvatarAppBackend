using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Avatar.App.Api.Models;
using Avatar.App.Entities;
using Avatar.App.Service.Exceptions;
using Avatar.App.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Avatar.App.Api.Controllers
{
    [Authorize]
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IVideoService _videoService;

        public MessageController(IMessageService messageService, IVideoService videoService)
        {
            _messageService = messageService;
            _videoService = videoService;
        }
        [Route("send")]
        [HttpPost]
        public async Task<ActionResult> SendMessage(string text, string fileName)
        {
            try
            {
                //var userGuid = GetUserGuid();
                //if (userGuid == null) return Unauthorized();
                //var toUser = await _videoService.GetVideoOwnerAsync(fileName);
                //await _messageService.SendMessageAsync(text, userGuid.Value, toUser);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return Unauthorized();
            }
            catch (VideoNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        [Route("accept/{messageId}")]
        [HttpGet]
        public async Task<ActionResult> SetAcceptanceStatus(long messageId, bool isAccepted)
        {
            try
            {
                var userGuid = GetUserGuid();
                if (userGuid == null) return Unauthorized();
                await _messageService.SetAcceptanceStatusAsync(messageId, userGuid.Value, isAccepted);
                return Ok();
            }
            catch (MessageNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        [Route("get/messages")]
        [HttpPost]
        public async Task<ActionResult> GetMessages()
        {
            try
            {
                var userGuid = GetUserGuid();
                if (userGuid == null) return Unauthorized();
                var messages = await _messageService.GetMessages(userGuid.Value);
                var messageModels = messages.Select(m => new MessageModel
                {
                    Id = m.Id,
                    Text = m.Text
                });
                return new JsonResult(messageModels);
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        [Route("get/contacts")]
        [HttpPost]
        public async Task<ActionResult> GetContacts()
        {
            try
            {
                var userGuid = GetUserGuid();
                if (userGuid == null) return Unauthorized();
                var messages = await _messageService.GetContacts(userGuid.Value);
                var messageModels = messages.Select(m => new ContactModel()
                {
                    Id = m.Id,
                    Contact = m.Contact
                });
                return new JsonResult(messageModels);
            }
            catch (Exception ex)
            {
                Logger.Log.LogError(ex.Message + ex.StackTrace);
                return Problem();
            }
        }

        #region Private Methods

        private Guid? GetUserGuid()
        {
            var nameIdentifier = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifier == null) return null;
            return Guid.Parse(nameIdentifier.Value);
        }

        #endregion
    }
}