using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TJM103.Models;

namespace TJM103.Controllers
{
    public class AccountController : Controller
    {
        private readonly List<UserModel> users;
        public AccountController()
        {
            users = new List<UserModel>
            {
                new UserModel { Account = "user1@gmail.com", Password = "password1" },
                new UserModel { Account = "user2@gmail.com", Password = "password2" }
            };
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            var user = users.FirstOrDefault(x => x.Account == model.Account && x.Password == model.Password);
            if (user == null)
            {
                return View();
            }
            //login


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Account),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim("Name", "Reds")
            };
            
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            return RedirectToAction("index","home");
        }
    }
}
