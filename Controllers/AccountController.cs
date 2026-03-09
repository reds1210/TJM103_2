using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TJM103.Helper;
using TJM103.Models;
using TJM103.Models.Entity;

namespace TJM103.Controllers
{
    public class AccountController : Controller
    {
        private readonly List<UserModel> users;
        private readonly AppDbContext _db;

        public AccountController(AppDbContext db)
        {
            users = new List<UserModel>
            {
                new UserModel { Account = "user1@gmail.com", Password = "password1" },
                new UserModel { Account = "user2@gmail.com", Password = "password2" }
            };
            this._db = db;
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
            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> RegisterAsync(RegisterModel model)
        {
            var isRegistered = _db.Users.Any(x => x.Email == model.Email);
            if (isRegistered)
            {
                return View();
            }
            _db.Users.Add(new User {Name=model.Email, Email = model.Email, Password = model.Password });
            _db.SaveChanges();
            var myKey = "12345678901234567890123456789012";
            var myIV = "1234567890123456";
            //encrypt
            var encrypted = AesHelper.Encrypt(model.Email, myKey, myIV);

            var url = "https://localhost:7236/account/validate?token=" + encrypted;



            var sh = new SmtpHelper();

            var body = $@"<!DOCTYPE html>
<html lang=""zh-Hant"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>歡迎加入我們的社群</title>
    <style>
        /* 基礎樣式重置 */
        body, table, td, a {{ -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; }}
        table, td {{ mso-table-lspace: 0pt; mso-table-rspace: 0pt; }}
        img {{ border: 0; height: auto; line-height: 100%; outline: none; text-decoration: none; }}
        
        body {{
            margin: 0;
            padding: 0;
            width: 100% !important;
            height: 100% !important;
            background-color: #f4f7f9;
            font-family: 'PingFang TC', 'Microsoft JhengHei', Helvetica, Arial, sans-serif;
        }}

        /* 主容器樣式 */
        .email-container {{
            max-width: 600px;
            margin: 40px auto;
            background: #ffffff;
            border-radius: 16px;
            overflow: hidden;
            box-shadow: 0 10px 30px rgba(0,0,0,0.1);
        }}

        /* 漸層頂部 */
        .header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 40px 20px;
            text-align: center;
            color: #ffffff;
        }}

        .header h1 {{
            margin: 0;
            font-size: 28px;
            letter-spacing: 2px;
            text-transform: uppercase;
        }}

        /* 內容區域 */
        .content {{
            padding: 40px 30px;
            line-height: 1.8;
            color: #4a5568;
        }}

        .welcome-text {{
            font-size: 20px;
            font-weight: bold;
            color: #2d3748;
            margin-bottom: 20px;
        }}

        /* 驗證碼區塊 */
        .otp-box {{
            background-color: #f8fafc;
            border: 2px dashed #cbd5e0;
            border-radius: 12px;
            padding: 20px;
            text-align: center;
            margin: 30px 0;
        }}

        .otp-code {{
            font-size: 36px;
            font-family: 'Courier New', monospace;
            font-weight: 800;
            color: #4c51bf;
            letter-spacing: 8px;
        }}

        /* CTA 按鈕 */
        .btn-wrapper {{
            text-align: center;
            margin-top: 30px;
        }}

        .cta-button {{
            display: inline-block;
            padding: 16px 36px;
            background-color: #667eea;
            color: #ffffff !important;
            text-decoration: none;
            border-radius: 50px;
            font-weight: bold;
            transition: all 0.3s ease;
            box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4);
        }}

        .cta-button:hover {{
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(102, 126, 234, 0.6);
            background-color: #5a67d8;
        }}

        /* 頁尾樣式 */
        .footer {{
            background-color: #f8fafc;
            padding: 30px;
            text-align: center;
            font-size: 12px;
            color: #a0aec0;
        }}

        .social-links {{
            margin-bottom: 20px;
        }}

        .social-links a {{
            margin: 0 10px;
            color: #718096;
            text-decoration: none;
        }}

        /* 響應式調整 */
        @media screen and (max-width: 600px) {{
            .email-container {{ margin: 0; border-radius: 0; }}
            .header h1 {{ font-size: 24px; }}
            .otp-code {{ font-size: 28px; }}
        }}
    </style>
</head>
<body>

    <div class=""email-container"">
        <div class=""header"">
            <h1>WELCOME</h1>
            <p style=""margin-top: 10px; opacity: 0.9;"">探索無限可能的開始</p>
        </div>

        <div class=""content"">
            <div class=""welcome-text"">親愛的用戶，您好！</div>
            <p>感謝您註冊我們的服務。我們很高興能邀請您加入這個充滿活力的社群。為了確保您的帳號安全並啟用完整功能，請輸入下方的驗證碼，或點擊按鈕完成最後步驟：</p>
            
            <div class=""otp-box"">
                <div style=""font-size: 14px; color: #718096; margin-bottom: 10px;"">您的專屬驗證碼</div>
                <div class=""otp-code"">888-666</div>
                <div style=""font-size: 12px; color: #a0aec0; margin-top: 10px;"">(該驗證碼將於 15 分鐘後失效)</div>
            </div>

            <div class=""btn-wrapper"">
                <a href=""{url}"" class=""cta-button"">立即啟動帳號</a>
            </div>

            <hr style=""border: 0; border-top: 1px solid #edf2f7; margin: 40px 0;"">

            <p style=""font-size: 14px;"">如果您沒有註冊此帳號，請忽略此郵件。若有任何疑問，歡迎隨時聯繫我們的支援團隊。</p>
        </div>

        <div class=""footer"">
            <div class=""social-links"">
                <a href=""#"">Facebook</a> | <a href=""#"">Instagram</a> | <a href=""#"">Twitter</a>
            </div>
            <p>© 2026 您的品牌名稱. All Rights Reserved.</p>
            <p>台北市松山區某某路 123 號</p>
            <p><a href=""#"" style=""color: #a0aec0;"">取消訂閱</a> | <a href=""#"" style=""color: #a0aec0;"">隱私政策</a></p>
        </div>
    </div>

</body>
</html>";


            await sh.SendEmailAsync(model.Email, "Test", body);
            return RedirectToAction("index","home");

        }
    }
}
