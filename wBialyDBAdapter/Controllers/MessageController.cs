using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using wBialyDBAdapter.Extensions;
using wBialyDBAdapter.Model.Message;
using wBialyDBAdapter.Model.User;
using wBialyDBAdapter.Services.Message;
using wBialyDBAdapter.Services.User;

namespace wBialyDBAdapter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _messageService.GetMessages());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateMessageInput input)
        {
            await _messageService.CreateMessage(input, User.GetUserId());
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateMessageInput input)
        {
            var result = await _messageService.UpdateMessage(input, User.GetUserId());
            if (!result)
                return Forbid();

            return Ok();
        }

        [HttpPut("/editors")]
        [Authorize]
        public async Task<IActionResult> UpdateEditors([FromBody] UpdateMessageEditorsInput input)
        {
            var result = await _messageService.UpdateCanModifyBatch(input, User.GetUserId());
            if (!result)
                return Forbid();

            return Ok();
        }

        [HttpDelete("{messageId}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int messageId)
        {
            var result = await _messageService.DeleteMessage(messageId, User.GetUserId());
            if (!result)
                return Forbid();

            return Ok();
        }
    }
}
