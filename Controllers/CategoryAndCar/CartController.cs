using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuokkaMesh.Models.Data;
using QuokkaMesh.Models.DataModels.CartCategory.Cart;
using QuokkaMesh.Models.DataModels.CartCategory.CartCategoryDTO;
using QuokkaMesh.Models.DataModels.CartCategory.Category;

namespace QuokkaMesh.Controllers.CategoryAndCar
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;


        public CartController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }


        [HttpPost("Admin/AddCart")]
        public async Task<IActionResult> AddCart([FromForm] CartDTO cartDTO , int categoryId)
        {

            if (cartDTO.ImageDesign == null || cartDTO.ImageDesign.Length == 0)
            {
                return Ok(new { Message = "No Image File Selected." });
            }
            string randem1 = Guid.NewGuid().ToString();

            string path1 = Path.Combine("StaticFile/Images/", $"{randem1}_{cartDTO.ImageDesign.FileName}");
            string fullPath1 = Path.Combine(_host.ContentRootPath.ToString(), path1);

            using (var stream = new FileStream(fullPath1, FileMode.Create))
            {
                await cartDTO.ImageDesign.CopyToAsync(stream);
            }

            CartModel result = new()
            {
                Titel = cartDTO.Titel,
                ImageDesign = path1,
                Receiver = cartDTO.Receiver,
                Sender = cartDTO.Sender,
                Content = cartDTO.Content,
                NumberOfPoint = cartDTO.NumberOfPoint,  
                IsPremium = cartDTO.IsPremium,
                Created = cartDTO.Created
            };

            await _db.Cart.AddAsync(result);
            _db.SaveChanges();

            CategoryCart categoryCart = new CategoryCart { CartId = result.Id , CategoryId = categoryId };
            _db.CategoryCart.Add(categoryCart);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                Messages = "Send Succesfully",
                result.Id,
                result.Titel,
                result.ImageDesign,
                result.Receiver,
                result.Sender,
                result.Content,
                result.NumberOfPoint,
                result.IsPremium,
                result.IsActive,
                result.Created,
            });
        }



        [HttpGet("Admin/GetAllCart")]
        public async Task<IActionResult> GetAllCart()
        {
            var cart = await _db.Cart.Select(x => new
            {
                x.Id,
                x.Titel,
                x.ImageDesign,
                x.Receiver,
                x.Sender,
                x.Content,
                x.NumberOfPoint,
                x.IsPremium,
                x.IsActive,
                x.Created,
            }).ToListAsync();

            return Ok(new { Messages = "Succesfully", cart });
        }









        [HttpGet("Category/GetAllCart")]
        public async Task<IActionResult> GetAllCartForCategory(int categoryId)
        {
            var cart = await _db.CategoryCart.Where(x => x.CategoryId == categoryId)
                .SelectMany(x => x.Cart.CategoryCart
                .Select(x=>new {
           
                x.Cart.Id,
                x.Cart.Titel,
                x.Cart.ImageDesign,
                x.Cart.Receiver,
                x.Cart.Sender,
                x.Cart.Content,
                x.Cart.NumberOfPoint,
                x.Cart.IsPremium,
                x.Cart.IsActive,
                x.Cart.Created,
            })).ToListAsync();

            return Ok(new { Messages = "Succesfully", cart });
        }



        [HttpPut("Admin/UpdateCart")]
        public async Task<IActionResult> UpdateCart(int id, [FromForm] CartDTO cartDTO)
        {

            var c = await _db.Cart.SingleOrDefaultAsync(x => x.Id == id);
            if (c == null)
            {
                return NotFound(new { Messages = $"Category Id {id} Not Exists" });
            }


            if (c.ImageDesign == null || c.ImageDesign.Length == 0 || cartDTO.ImageDesign != null)
            {
                string randem = Guid.NewGuid().ToString();

                string path1 = Path.Combine("StaticFile/Images/", $"{randem}_{cartDTO.ImageDesign.FileName}");

                string fullPath1 = Path.Combine(_host.ContentRootPath.ToString(), path1);

                using (var stream = new FileStream(fullPath1, FileMode.Create))
                {
                    await cartDTO.ImageDesign.CopyToAsync(stream);
                }
                c.ImageDesign = path1;
            }

            c.ImageDesign = c.ImageDesign;



            if (cartDTO.Titel == null)
                c.Titel = c.Titel;
            else
                c.Titel = cartDTO.Titel;

            if (cartDTO.Sender == null)
                c.Sender = c.Sender;
            else
                c.Sender = cartDTO.Sender;

            if (cartDTO.Receiver == null)
                c.Receiver = c.Receiver;
            else
                c.Receiver = cartDTO.Receiver;


            if (cartDTO.Content == null)
                c.Content = c.Content;
            else
                c.Content = cartDTO.Content;

            if (cartDTO.NumberOfPoint == null)
                c.NumberOfPoint = c.NumberOfPoint;
            else
                c.NumberOfPoint = cartDTO.NumberOfPoint;

            if (cartDTO.IsPremium == null)
                c.IsPremium = c.IsPremium;
            else
                c.IsPremium = cartDTO.IsPremium;


            _db.Cart.Update(c);
            _db.SaveChanges();


            return Ok(new
            {
                Messages = "Send Succesfully",
                c.Id,
                c.Titel,
                c.ImageDesign,
                c.Sender,
                c.Receiver,
                c.Content,  
                c.NumberOfPoint,
                c.IsPremium,
                c.IsActive,
                c.Created,

            });

        }



        [HttpDelete("Admin/DeleteCart")]
        public async Task<IActionResult> DeleteCart( int CartId)
        {
            var cart = await _db.Cart.SingleOrDefaultAsync(x => x.Id == CartId);


            if (cart == null)
            {
                return BadRequest(new { Message = "Cart Is Not Exist." });
            }

            ////////////// CartUser 
            var cartId = _db.CartAndUserCart
              .Where(x => x.CartId == CartId)
              .Select(x => x.UserCartId);

            var finCart = await _db.CartAndUserCart
               .SingleOrDefaultAsync(x => x.CartId == CartId && cartId.Contains(x.UserCartId));

            if (finCart == null)
            {
                return NotFound();
            }
            //////////////////////CategoryCart

            var categId = _db.CategoryCart
              .Where(x => x.CartId == CartId)
              .Select(x => x.CategoryId);

            var finCategory = await _db.CategoryCart
               .SingleOrDefaultAsync(x => x.CartId == CartId && categId.Contains(x.CategoryId));

            if (finCategory == null)
            {
                return NotFound();
            }

            _db.CategoryCart.Remove(finCategory);
            _db.SaveChanges();

            _db.CartAndUserCart.Remove(finCart);
            _db.SaveChanges();

            _db.Cart.Remove(cart);
            await _db.SaveChangesAsync();


            return Ok(new
            {
                Messages = "Send Succesfully",

                cart.Id,
                cart.Titel,
                cart.ImageDesign,
                cart.Sender,
                cart.Receiver,
                cart.Content,
                cart.NumberOfPoint,
                cart.IsPremium,
                cart.IsActive,
                cart.Created,
            });
        }


    }
}
