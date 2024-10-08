﻿


using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuokkaMesh.Models.Data;
using QuokkaMesh.Models.DataModel.OTPModel;
using QuokkaMesh.Models.DataModel.TokenDataModel;
using QuokkaMesh.Models.DataModels.UserModel;
using QuokkaMesh.Services.Email;
using QuokkaMesh.Services.Users;
using System.IdentityModel.Tokens.Jwt;

namespace QuokkaMesh.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;




        public AuthController(IAuthService authService, IEmailSender emailSender, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _db = db;
            _emailSender = emailSender;
            _userManager = userManager;
        }


        [HttpPost("Register/User")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterModel model)
        {

            //return Ok("aaaaaaaaaaaaaaaaaaaaaaaaaa");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegistrationAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(new { Messages = $"{result.Message}" });

            return Ok(new { result });
        }



        [HttpPost("Login")]
        public async Task<IActionResult> GetTokenAsync([FromForm] TokenRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(new { Messages = result.Message });

            return Ok(new { result });
        }




        [HttpPut("IsAdmin/AddAdmin/{userId}")]
        public async Task<IActionResult> AddAdmin(string userId)
        {

            var Ads = await _db.Users.SingleOrDefaultAsync(x => x.Id == userId);
            if (Ads == null)
            {
                return NotFound(new { Messages = $"User Id {userId} Not Exists Or IsAdmin" });
            }

            if (Ads.IsAdmin == false)
                Ads.IsAdmin = true;


            _db.Users.Update(Ads);
            _db.SaveChanges();


            return Ok(
                new
                {
                    Ads.Id,
                    Ads.FullName,
                    Ads.PhoneNumber,
                    Ads.UserName,
                    Ads.Email,
                    Ads.DateTime,
                    Ads.IsAdmin,
                });

        }



        [HttpPut("UpdateSubProfile/{id}")]
        public async Task<IActionResult> UpdateSubProfil([FromRoute] string id, [FromForm] RegisterModel patch)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var result = await _authService.UpdateSubProfile(id, patch);

            return Ok( result );
        }



        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(new { result });
        }




        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeToken model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _authService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Token is invalid!");

            return Ok();
        }


        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }




   

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(string userId, string newPassword , string oldPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "User Not Found." });
            }

            if (user is null || !await _userManager.CheckPasswordAsync(user, oldPassword))
            {

                return NotFound (new  { Message = "Old Password Is Incorrect!" });

            }

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { Message = "Password Is Updated!" });
        }




        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {

            var user = await _db.Users
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    x.PhoneNumber,
                    x.UserName,
                    x.Email,
                    x.DateTime,
                    x.IsAdmin,
                    x.ImageCover,
                }).ToListAsync();
            return Ok(user);
        }



        [HttpGet("GetOneUser")]
        public async Task<IActionResult> GetOneUser(string userId)
        {

            var user = await _db.Users.Where(x => x.Id == userId)
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    x.PhoneNumber,
                    x.UserName,
                    x.Email,
                    x.DateTime,
                    x.ImageCover,
                    x.IsAdmin,
                }).ToListAsync();
            return Ok(user);
        }





        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string userId)
        {

            var user = await _db.Users
         .Include(u => u.UserCart)
         .Include(u => u.UserMessage)
         .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            _db.UserCart.RemoveRange(user.UserCart);
            _db.UserMessages.RemoveRange(user.UserMessage);
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return Ok();
        }

    }
}
