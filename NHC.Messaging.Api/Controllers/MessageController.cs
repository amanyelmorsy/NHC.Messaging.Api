using Microsoft.AspNetCore.Mvc;
using NHC.Messaging.Application.Services;
using NHC.Messaging.Domain;
using NHC.Messaging.Domain.DTO;

namespace NHC.Messaging.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        [Route("")]
        public  ActionResult GetAll()
        {
            return Ok(_messageService.GetAllMessages()) ;
        }
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> SendMessages(MessageDto message)
        {
            await _messageService.SendMessages(message);
            return Ok();
        }
    }
}
