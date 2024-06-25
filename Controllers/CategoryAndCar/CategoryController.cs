
using Microsoft.AspNetCore.Mvc;
using QuokkaMesh.Models.Data;
using QuokkaMesh.Models.DataModels.CartCategory.CartCategoryDTO;
using QuokkaMesh.Models.DataModels.CartCategory.Category;
using Microsoft.EntityFrameworkCore;

namespace QuokkaMesh.Controllers.Category
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;


        public CategoryController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;   
        }


        [HttpPost("Admin/AddCategory")]
        public async Task<IActionResult> AddCategory([FromForm] CategoryDTO categoryDTO)
        {
        

            if (categoryDTO.Image == null || categoryDTO.Image.Length == 0)
            {
                return Ok(new { Message = "No Image File Selected." });
            }
            string randem1 = Guid.NewGuid().ToString();

            string path1 = Path.Combine("StaticFile/Images/", $"{randem1}_{categoryDTO.Image.FileName}");
            string fullPath1 = Path.Combine(_host.ContentRootPath.ToString(), path1);

            using (var stream = new FileStream(fullPath1, FileMode.Create))
            {
                await categoryDTO.Image.CopyToAsync(stream);
            }
       
            CategoryModel result = new()
            {
               Titel = categoryDTO.Titel,   
               Image = path1,
            };

            await _db.Categories.AddAsync(result);
            _db.SaveChanges();


            return Ok(new
            {
                Messages = "Send Succesfully",

                result.Id,
               result.Titel,
               result.Image,
               result.IsActive,
            });
        }



        [HttpGet("Admin/GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var categoey = await _db.Categories.Select(x => new
            {
                x.Id,
                x.Titel,
                x.Image,
                x.IsActive,
            }).ToListAsync();
                
            return Ok(new {Messages = "Succesfully" ,categoey });
        }



        [HttpPut("Admin/UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryDTO categoryDTO)
        {

            var c = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id );
            if (c == null)
            {
                return NotFound(new { Messages = $"Category Id {id} Not Exists" });
            }
            



            if (c.Image ==null || c.Image.Length == 0 ||categoryDTO.Image != null) 
            {
                string randem = Guid.NewGuid().ToString();

                string path1 = Path.Combine("StaticFile/Images/", $"{randem}_{categoryDTO.Image.FileName}");

                string fullPath1 = Path.Combine(_host.ContentRootPath.ToString(), path1);

                using (var stream = new FileStream(fullPath1, FileMode.Create))
                {
                    await categoryDTO.Image.CopyToAsync(stream);
                }
                c.Image = path1 ;

            }


            c.Image = c.Image;



            if (categoryDTO.Titel == null)
                c.Titel = c.Titel;
            else
                c.Titel = categoryDTO.Titel;

         

            _db.Categories.Update(c);
            _db.SaveChanges();


            return Ok(new
            {
                Messages = "Send Succesfully",

                c.Id,
               c.Titel,
               c.Image,
               c.IsActive,

            });

        }



        [HttpDelete("Admin/DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromForm] int id)
        {
            var category = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);

            if (category == null )
            {
                return BadRequest(new{Message = "Category Is Not Exist." });
            }

          category.IsActive = false ;
            
            _db.Categories.Update(category);
            _db.SaveChanges();


            return Ok(new
            {
                Messages = "Send Succesfully",

                category.Id,
                category.Titel,
                category.Image,
                category.IsActive
            });
        }



    }
}
