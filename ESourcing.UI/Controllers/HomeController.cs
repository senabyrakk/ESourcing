using ESourcing.Core.Entities;
using ESourcing.UI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ESourcing.UI.Controllers
{
    public class HomeController : Controller
    {
        public UserManager<User> _userManager { get; }
        public SignInManager<User> _signInManager { get; }

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserDto loginModel, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginModel.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);

                    if (result.Succeeded)
                    {
                        HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
                        return LocalRedirect(returnUrl);
                    }
                    else
                        ModelState.AddModelError("", "Email address is not valid or password");
                }
                else
                    ModelState.AddModelError("", "Email address is not valid or password");
            }
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SingUpUserDto signupModel)
        {
            if (ModelState.IsValid)
            {
                User usr = new User();
                usr.FirstName = signupModel.FirstName;
                usr.Email = signupModel.Email;
                usr.LastName = signupModel.LastName;
                usr.PhoneNumber = signupModel.PhoneNumber;
                usr.UserName = signupModel.UserName;
                if (signupModel.UserSelectTypeId == 1)
                {
                    usr.IsBuyer = true;
                    usr.IsSeller = false;
                }
                else
                {
                    usr.IsSeller = true;
                    usr.IsBuyer = false;
                }

                var result = await _userManager.CreateAsync(usr, signupModel.Password);

                if (result.Succeeded)
                    return RedirectToAction("Login");
                else
                {
                    foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(signupModel);
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
