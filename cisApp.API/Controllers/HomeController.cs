using cisApp.Function;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cisApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        readonly static IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json")
                      .Build();

        // GET: api/<HomeController>
        [HttpGet]
        public object Get(string search, int? page = 0)
        {
            string webAdmin = config.GetSection("WebConfig:AdminWebStie").Value;
            SearchModel model = new SearchModel()
            {
                text = search,
                currentPage = page,
                pageSize = 10
            };
            var Obj = GetAlbum.Get.GetAlbumImage(model, webAdmin);

            if (Obj.Count > 0)
            {
                return Ok(resultJson.success(null, null, Obj.Select(o => new { o.AttachFileId, o.FileName, o.FullUrlPath, o.UserId, o.JobID, o.AlbumName, o.Category, o.Tags }).ToList(), null, null, page, page + 1));
            }
            else
            {
                return Unauthorized(resultJson.errors("ไม่พบข้อมูล", "ไม่พบข้อมูล", null));
            }

        }

        //// GET api/<HomeController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<HomeController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<HomeController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<HomeController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
