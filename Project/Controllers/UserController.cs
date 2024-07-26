using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;
using System.Security.Claims;

namespace Project.Controllers
{
    public class UserController : Controller
    {
        public StudentDbContext _context;

        public UserController(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user) 
        {
            if (ModelState.IsValid)
            {
                await _context.Set<User>().AddAsync(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return RedirectToAction(nameof(Register));


        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                var result = _context.Set<User>().Where(p => p.Email == user.Email).SingleOrDefault();
                if (result == null)
                {
                    TempData["Error"] = "Wrong Email";
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    if (result.Password == user.Password)
                    {
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, result.Email),
                        new Claim(ClaimTypes.Role, result.Role)
                    };

                        var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrinciple = new ClaimsPrincipal(claimIdentity);
                        var expiration = new AuthenticationProperties()
                        {
                            ExpiresUtc = DateTime.UtcNow.AddDays(5),
                            IsPersistent = true
                        };

                        var cookieoptions = new CookieOptions()
                        {
                            Expires = DateTime.UtcNow.AddDays(5),
                            IsEssential = true,
                            HttpOnly = true,
                            Secure = true
                        };

                        HttpContext.Session.SetString("Email", user.Email);
                        Response.Cookies.Append("Example", "Value", cookieoptions);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrinciple, expiration);
                    }
                }
            }
                return RedirectToAction(nameof(Index), "Student", new { area = "" });
            
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Email");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Register));
        }
    }
}
