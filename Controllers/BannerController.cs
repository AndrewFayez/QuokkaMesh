
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuokkaMesh.Models.Data;
using QuokkaMesh.Models.DataModels.News;

namespace QuokkaMesh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {



            private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _host;

        public BannerController(ApplicationDbContext db, IWebHostEnvironment host)
        {
                _db = db;
            _host = host;

        }

        // POST api/<AdsController>
        [HttpPost("AddBanner")]
            public async Task<IActionResult> News([FromForm] DTONews dTOAds)
            {



            if (dTOAds.Image == null || dTOAds.Image.Length == 0)
            {
                return BadRequest(new { Messages = "No File Selected." });
            }
            string randem1 = Guid.NewGuid().ToString();
            string path1 = Path.Combine("StaticFile/Images/", $"{randem1}_{dTOAds.Image.FileName}");
            string fullPath1 = Path.Combine(_host.ContentRootPath.ToString(), path1);

            using (var stream = new FileStream(fullPath1, FileMode.Create))
            {
                await dTOAds.Image.CopyToAsync(stream);
            }


            NewsModel Ads = new()
                {
                    Title = dTOAds.Title,
                    Image = path1,
                    CreatedDate = DateTime.Now,
                    
                };
                await _db.News.AddAsync(Ads);
                _db.SaveChanges();

                //UserNews postUser = new UserNews { UserId = userId, NewsId = Ads.Id };
                //_db.UserNews.Add(postUser);
                //_db.SaveChanges();


                return Ok(new { Ads.Id, Ads.Title, Ads.Image,Ads.CreatedDate });
            }




            [HttpGet("GetAllBanner")]
            public async Task<IActionResult> GetAllNews()
            {
                var posts = await _db.News
                    .Select(x => new
                    {
                        x.Id,
                        x.Title,
                        x.Image,
                        x.CreatedDate
                    }).ToListAsync();
                return Ok(posts);
            }


        [HttpPut("UpdateNews/{id}")]
        public async Task<IActionResult> UpdateNews([FromRoute] int id, [FromForm] DTONews prof)
        {
            var c = await _db.News.SingleOrDefaultAsync(x => x.Id == id);
            if (c == null)
            {
                return Ok(new { Message = $"Animal Id {id} Not Exists" });
            }


            if (c.Image == null || c.Image.Length == 0 || prof.Image != null)
            {
                string randem1 = Guid.NewGuid().ToString();

                string path1 = Path.Combine("StaticFile/Images/", $"{randem1}_{prof.Image.FileName}");

                string fullPath1 = Path.Combine(_host.ContentRootPath.ToString(), path1);

                using (var stream = new FileStream(fullPath1, FileMode.Create))
                {
                    await prof.Image.CopyToAsync(stream);
                }
                c.Image = path1;
            }

            c.Image = c.Image;



            if (prof.Title == null)
                c.Title = c.Title;
            else
                c.Title = prof.Title;


            _db.News.Update(c);
            _db.SaveChanges();

            return Ok(new
            {
                c.Id,
                c.Title,
                c.Image,
                c.CreatedDate,
            });


        }

        

        // DELETE api/<AdsController>/5
        [HttpDelete("DeleteBanner")]
            public async Task<IActionResult> Delete(int id)
            {
                var ads = await _db.News.SingleOrDefaultAsync(x => x.Id == id);
                if (ads == null)
                {
                    return BadRequest(new { Messages = "This Is Banner Not Found" });
                }
                _db.News.Remove(ads);
                _db.SaveChanges();
                return Ok(new { ads.Id, ads.Title, ads.Image,ads.CreatedDate });
            }



        }
    }


