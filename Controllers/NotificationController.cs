using ChatWebAPI.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QuokkaMesh.Hubs;
using QuokkaMesh.Models.Data;
using QuokkaMesh.Models.DataModels.Notify;

namespace QuokkaMesh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private IHubContext<MessageHub, IMessageHubClient> _messageHub;
        private readonly ApplicationDbContext _db;

        public NotificationController(IHubContext<MessageHub, IMessageHubClient> messageHub, ApplicationDbContext db)
        {
            _messageHub = messageHub;
            _db = db;
        }


        [HttpPost]
        [Route("SendNtification")]
        public async Task<IActionResult> SendNotification([FromForm] NotificationDTO notify)
        {

            var not = new NotificationModel
            {
             Title = notify.Title,
             Message = notify.Message,
             DateTime = DateTime.Now,   

            };

            _db.Notifications.Add(not);
            _db.SaveChanges();

            await  _messageHub.Clients.All.SendNotification(notify);
            return Ok (not);
        }

        [HttpPost("JoinRoom")]
        public async Task<IActionResult> JoinRoom([FromForm] string userId)
        {

            // await _hubContext.Groups.AddToGroupAsync(Context.ConnectionId, userId);

            await _messageHub.Groups.AddToGroupAsync($"{userId}", userId);

            return Ok();
        }

        [HttpPost("LeaveRoom")]
        public async Task<IActionResult> LeaveRoom([FromForm] string userId)
        {

            await _messageHub.Groups.RemoveFromGroupAsync($"{userId}", userId);

            return Ok();
        }
       

        [HttpGet("GetAllNotification")]
        public async Task<IActionResult> GetAllNotification()
        {

            var user = await _db.Notifications
                .Select(x => new
                {
                    x.Id,
                   x.Title,
                   x.Message,
                    x.DateTime,
                }).ToListAsync();
            return Ok(user);
        }




    }
}
