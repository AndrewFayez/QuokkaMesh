using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuokkaMesh.Models.Data;
using QuokkaMesh.Models.DataModels.CartCategory.Cart;
using QuokkaMesh.Models.DataModels.CartCategory.CartCategoryDTO;
using QuokkaMesh.Services.Users;

namespace QuokkaMesh.Controllers.CategoryAndCar
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendCartController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;
        private readonly AuthService _tokenGenerate;

        public SendCartController(ApplicationDbContext db, IWebHostEnvironment host , AuthService tokenGenerate)
        {
            _db = db;
            _host = host;
            _tokenGenerate = tokenGenerate;
        }






        [HttpPost("User/SendCart")]
        public async Task<IActionResult> SendCart(string userId , string ReceiverId , int cartId, [FromForm] UserCartDTO userCart)
        {
            
            var sender =await _db.Users.FindAsync( userId);

            if (sender == null)
            {
                return BadRequest(new { Messages = " User Not Found." });
            }

            var cart = await _db.Cart.FindAsync(cartId);
            if (cart == null)
            {
                return BadRequest(new { Messages = "Cart Is Noy Found" });
            }
            if (cart.IsPremium == true)
            {
                if (sender?.IsPremium == true)
                {
                    UserCart resultpre = new()
                    {
                        ImageDesign = cart.ImageDesign,
                        IsPremium = cart.IsPremium,
                        Receiver = userCart.Receiver,
                        Sender = userCart.Sender,
                        ReceiverId = ReceiverId,
                        SenderId = userId,
                        Content = userCart.Content,
                        Created = userCart.Created,
                        Titel = userCart.Titel,
                    };
                    await _db.UserCart.AddAsync(resultpre);
                    _db.SaveChanges();

                    CartAndUserCart CartUserpre = new CartAndUserCart { UserCartId = resultpre.Id, CartId = cartId };
                    _db.CartAndUserCart.Add(CartUserpre);
                    _db.SaveChanges();


                    var countPointpre = await _db.Users.SingleOrDefaultAsync(x => x.Id == userId);
                    countPointpre.CountPoint = countPointpre.CountPoint + cart.NumberOfPoint;
                    _db.Users.Update(countPointpre);

                    _db.SaveChanges();


                    return Ok(new
                    {
                        Messages = "Send succesfully",
                        resultpre.ImageDesign,
                        resultpre.IsPremium,
                        resultpre.Receiver,
                        resultpre.Sender,
                        resultpre.Titel,
                        resultpre.Content,
                        resultpre.Created,
                    });
                }
                return BadRequest(new { Messages = "Cart Is Premium Please Upgrade Your Account" });
            }

            UserCart result = new() 
            {
                ImageDesign = cart.ImageDesign, 
                IsPremium = cart.IsPremium,
                Receiver = userCart.Receiver,
                Sender = userCart.Sender,
                ReceiverId = ReceiverId,
                SenderId = userId,
                Content = userCart.Content, 
                Created = userCart.Created,
                Titel = userCart.Titel,
            };
            await _db.UserCart.AddAsync(result);
            _db.SaveChanges();

           CartAndUserCart CartUser = new CartAndUserCart { UserCartId = result.Id , CartId  = cartId };
            _db.CartAndUserCart.Add(CartUser);
            _db.SaveChanges();


            var countPoint = await _db.Users.SingleOrDefaultAsync(x => x.Id == userId);
            countPoint.CountPoint = countPoint.CountPoint + cart.NumberOfPoint;
            _db.Users.Update(countPoint);

            _db.SaveChanges();


            return Ok(new { 
                Messages = "Send succesfully",
                result.Id,
                result.ImageDesign ,
                result.IsPremium , 
                result.Receiver,
                result.Sender,
                result.Titel ,
                result.Content ,
                result.Created ,  
                result.IsActive,
            });
        }




        [HttpGet("User/ViewMyCartsended")]
        public async Task<IActionResult> ViewMyCartsended(string userId)
        {
            
            var sender = _db.Users.FirstOrDefault(x => x.Id == userId);

            if (sender == null)
            {
                return BadRequest(new { Messages = "Please Login." });
            }

            var cart = await _db.UserCart.Where(x => x.SenderId == sender.Id)
                .Select(x =>new
                {
                    x.Id,
                    x.ImageDesign ,
                    x.IsPremium ,
                    x.Receiver,
                    x.Sender,
                    x.Titel ,
                    x.Created ,
                    x.Content ,
                    x.IsActive, 
                  
                }).ToListAsync();
                
            return Ok(new {
                    Messages = "Send succesfully",
                    cart,
            });
        }




        [HttpGet("User/ViewMyCartrecevied")]
        public async Task<IActionResult> ViewMyCartrecevied(string userId)
        {
            
            var sender = _db.Users.FirstOrDefault(x => x.Id == userId);

            if (sender == null)
            {
                return BadRequest(new { Messages = "Please Login." });
            }

            var cart = await _db.UserCart.Where(x => x.ReceiverId == sender.Id)
                .Select(x => new
                {
                    x.Id,
                    x.ImageDesign,
                    x.IsPremium,
                    x.Receiver,
                    x.Sender,
                    x.Titel,
                    x.Created,
                    x.Content,
                    x.IsActive,
                }).ToListAsync();

            return Ok(new
            {
                Messages = "Send succesfully",
                cart,
            });
        }




        [HttpGet("SearchInAllUser/{userName}")]
        public async Task<IActionResult> SearchInAllUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return (IActionResult)await _db.Users.ToListAsync();
            }

            var result = _db.Users.Where(x => x.UserName.ToLower().Contains(userName.ToLower()))
                .Select(x => new
                {
                   x.Id,
                   x.UserName,
                   x.FullName,
                   x.ImageCover,
                   x.Email,
                  
                }).ToList();


            return Ok(new { Messages = "Send succesfully",  result });
        }


    }
}
