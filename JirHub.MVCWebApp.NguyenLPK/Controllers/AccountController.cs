using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Data;
using JirHub.Services.NguyenLPK.implements;
using JirHub.Services.NguyenLPK;
using JirHub.MVCWebApp.NguyenLPK.Models;

namespace JirHub.MVCWebApp.NguyenLPK.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISystemUserAccountService _userAccountService;

        public AccountController(ISystemUserAccountService systemUserAccountService) => _userAccountService = systemUserAccountService; 

        public IActionResult Index()
        {
            return RedirectToAction("Login");
            //return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var userAccount = await _userAccountService.GetUserAccountAsync(loginRequest.UserName, loginRequest.Password);

                if (userAccount != null)
                {
                    string roleId = userAccount.Role ?? "STUDENT"; // Default if null
                    var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, userAccount.FullName),
                                    new Claim(ClaimTypes.NameIdentifier, userAccount.UserId.ToString()),
                                    new Claim(ClaimTypes.Role, roleId)
                                };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    Response.Cookies.Append("UserName", userAccount.FullName);
                    Response.Cookies.Append("Role", roleId);

                    return RedirectToAction("Index", "ProjectReposNguyenLpks");
                }                
            }
            catch (Exception ex)
            {

            }

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ModelState.AddModelError("", "Login failure");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Forbidden()
        {
            return View();
        }
    }
}
