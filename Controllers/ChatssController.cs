
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuokkaMesh.Models.Data;
using QuokkaMesh.Models.DataModels;

namespace QuokkaMesh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatssController : ControllerBase
    {
        private readonly ApplicationDbContext _db;


        public ChatssController(ApplicationDbContext db)
        {
            _db = db;
        }


        [HttpGet("GetMessages")]
        public async Task<IActionResult> GetMessages()
        {
            var mes = await _db.Messages.Select(x=> new
            {
                x.Text,
                x.Timestamp,
            }).ToListAsync();
            return Ok(mes);
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] MessageDTO message ,string sender )
        {
            Message Mes = new()
            {
                Text = message.Text,
                Timestamp =DateTime.Now,
                ReciverId = message.ReciverId,
            };

            await _db.Messages.AddAsync(Mes);
            _db.SaveChanges();

            UserMessage UserMes = new UserMessage { UserId = sender, MessageId = Mes.Id };
            _db.UserMessages.Add(UserMes);
            _db.SaveChanges();


            return Ok(new { Mes.Id, Mes.Text, Mes.Timestamp });
        }

        [HttpGet("GetChat")]
        public async Task<IActionResult> GetChat(string senderId , string reciverId )
        {
            var sendermes = await _db.UserMessages.Where(x => x.UserId == senderId && x.Messages.ReciverId == reciverId ).SelectMany(x=>x.Messages.UserMessage.Select( x=> new
            {
                  x.User.FullName,
                  x.Messages.Text,
                  x.Messages.Timestamp,
                  x.User.Id,
                  
            })).ToListAsync();
            var recivermes = await _db.UserMessages.Where(x => x.UserId == reciverId &&   x.Messages.ReciverId == senderId).SelectMany(x => x.Messages.UserMessage.Select(x => new
            {
                x.User.FullName,
                x.Messages.Text,
                x.Messages.Timestamp,
                x.User.Id,

            })).ToListAsync();
            return Ok(new { sendermes, recivermes });
        }

    }
}
