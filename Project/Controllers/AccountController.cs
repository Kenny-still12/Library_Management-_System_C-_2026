using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Project.Models;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set { _userManager = value; }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext()
                    .Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await SignInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    ModelState.AddModelError("",
                        "Account locked after too many failed attempts. Try again in 5 minutes.");
                    return View(model);

                default:
                    ModelState.AddModelError("",
                        "Invalid email or password.");
                    return View(model);
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName
                };

                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, model.Role);

                    if (model.Role == "Member")
                    {
                        try
                        {
                            var memberDb = new LibraryDbContext();
                            var nameParts = model.FullName.Trim().Split(
                                                new char[] { ' ' }, 2,
                                                StringSplitOptions.RemoveEmptyEntries);

                            var member = new Member
                            {
                                FirstName = nameParts[0],
                                LastName = nameParts.Length > 1 ? nameParts[1] : "N/A",
                                Email = model.Email,
                                PhoneNumber = "Not provided",
                                Address = "Not provided",
                                DateOfBirth = new DateTime(1990, 1, 1),
                                MembershipDate = DateTime.Now,
                                UserId = user.Id
                            };

                            memberDb.Members.Add(member);
                            memberDb.SaveChanges();
                            memberDb.Dispose();
                        }
                        catch (Exception ex)
                        {
                            // Show exact error on the form instead of yellow crash page
                            ModelState.AddModelError("",
                                "Profile save failed: " + ex.InnerException?.Message ?? ex.Message);
                            return View(model);
                        }
                    }

                    await SignInManager.SignInAsync(
                        user, isPersistent: false, rememberBrowser: false);

                    TempData["Success"] = "Welcome, " + model.FullName + "!";
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
            }

            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(
                DefaultAuthenticationTypes.ApplicationCookie);
            TempData["Success"] = "You have been logged out.";
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
    }
}