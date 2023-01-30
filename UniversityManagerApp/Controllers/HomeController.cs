using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UniversityManagerApp.Data;
using UniversityManagerApp.Models;

namespace UniversityManagerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<Student> _signInManager;
        private readonly UserManager<Student> _userManager;
        private readonly SystemDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<Student> userManager, SignInManager<Student> signInManager, SystemDbContext context)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {
                if (_userManager.FindByEmailAsync(login.Email)?.Result != null)
                {
                    var user = _context.Students.FirstOrDefault(e => e.Email == login.Email);
                    var result = await _signInManager.PasswordSignInAsync(user, login.Password, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return Redirect(ViewBag.ReturnUrl ?? Url.Action("List", "Courses"));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(login);
                    }
                }
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View(login);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new Student
                {
                    Email = model.Email,
                    UserName = model.StudentName
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false, IdentityConstants.ApplicationScheme);

                    return RedirectToAction("List", "Courses");
                }

                AddErrors(result);
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<ActionResult> Logout(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}