using ChatWebAPI.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QuokkaMesh.Models.Data;
using QuokkaMesh.Models.DataModels;

namespace QuokkaMesh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly UserManager _userManager;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly ApplicationDbContext _db;


        public ChatController(UserManager userManager, IHubContext<ChatHub> hubContext , ApplicationDbContext db)
        {
            _userManager = userManager;
            _hubContext = hubContext;
            _db = db;
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] SendMessageRequest request)
        {
            SendMessageRequestModel mes = new()
            {
                FromUserId = request.FromUserId,
                ToUserId = request.ToUserId,
                Message = request.Message,
            };

            await _hubContext.Clients.User(mes.ToUserId).SendAsync("ReceiveMessage", mes.FromUserId, mes.Message);

            await _db.SendMessageRequestModel.AddAsync(mes);
            _db.SaveChanges();

            return Ok(mes);
        }

        [HttpPost("JoinRoom")]
        public async Task<IActionResult> JoinRoom([FromForm] string userId)
        {

           // await _hubContext.Groups.AddToGroupAsync(Context.ConnectionId, userId);

            await _hubContext.Groups.AddToGroupAsync($"{userId}", userId);

            return Ok();
        }

        [HttpPost("LeaveRoom")]
        public async Task<IActionResult> LeaveRoom([FromBody] string userId)
        {

            await _hubContext.Groups.RemoveFromGroupAsync($"{userId}", userId);

            return Ok();
        }
    }



}