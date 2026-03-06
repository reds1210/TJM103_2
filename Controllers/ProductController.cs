using Microsoft.AspNetCore.Mvc;
using TJM103.Models;

namespace TJM103.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;

        public ProductController(AppDbContext db)
        {
            this._db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
