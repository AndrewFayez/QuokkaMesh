using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuokkaMesh.Models.Data;

namespace QuokkaMesh.Controllers.CategoryAndCar
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;


        public StatisticsController(ApplicationDbContext db , UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }


        [HttpGet("Admin/GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {

            var user = await _userManager.Users.Select(x => new
                {
                    x.Id,
                    x.FullName,
                    x.PhoneNumber,
                    x.UserName,
                    x.Email,
                    x.DateTime,
                    x.CountPoint,
                    x.ImageCover
                }).OrderByDescending(x=>x.CountPoint).ToListAsync();
            return Ok(new { user });
        }


        [HttpGet("Admin/GetOneUser")]
        public async Task<IActionResult> GetOneUser(string userId)
        {

            var user = await _userManager.Users.Where(x=>x.Id == userId).Select(x => new
            {
                x.Id,
                x.FullName,
                x.PhoneNumber,
                x.UserName,
                x.Email,
                x.DateTime,
                x.CountPoint,
                x.ImageCover
            }).ToListAsync();
            return Ok(new { user });
        }




        [HttpGet("Admin/CountAllUser")]
        public async Task<IActionResult> CountAlluser()
        {
            var user = _userManager.Users.Count();
            return  Ok (new{ Numbers = user });
        }

        [HttpGet("Admin/CountAllCategory")]
        public async Task<IActionResult> CountAllCategory()
        {
            var user = _db.Categories.Count();
            return Ok(new { Numbers = user });
        }


        [HttpGet("Admin/CountAllCart")]
        public async Task<IActionResult> CountAllCart()
        {
            var user = _db.Cart.Count();
            return Ok(new { Numbers = user });
        }


        [HttpGet("User/UserPoint")]
        public async Task<IActionResult> UserPoint(string userId)
        {
            var user =await _db.Users.FindAsync(userId);
            return Ok(new { Numbers = user.CountPoint });
        }

        [HttpGet("User/CountAllCartSend")]
        public async Task<IActionResult> UserCountAllCartSend(string userId)
        {
            var user = _db.UserCart.Where(x=>x.SenderId == userId).Count();
            return Ok(new { Numbers = user });
        }



        [HttpGet("User/CountAllCartReceiver")]
        public async Task<IActionResult> UserCountAllCartReceiver(string userId)
        {
            var user = _db.UserCart.Where(x => x.ReceiverId == userId).Count();
            return Ok(new { Numbers = user });
        }

    }
}
