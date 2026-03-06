using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TJM103.Models;

namespace TJM103.Controllers.api
{
    [Route("api/Product/[action]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProductApiController(AppDbContext db)
        {
            this._db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var prod = _db.Products.ToList();
            return Ok(prod);
        }
    }
}
