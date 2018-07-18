using Camposur.BusinessLogic.Logic.Interfaces;
using Camposur.Model.ViewModel;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Camposur.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private IAuthLogic authLogic;

        public AccountController(IAuthLogic authLogic)
        {
            this.authLogic = authLogic;
        }

        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                authLogic.SignOut();
                return RedirectToAction("Login", "Account");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await authLogic.SignInUserAsync(model, true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Intento invalido.");
                    return View(model);
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var exists = authLogic.ExistsUser(model.Email);
            if (ModelState.IsValid)
            {
                var redirectionLink = Url.Action("Login", "Account", null, null, Request.Url.Host);
                var result = await authLogic.RegisterUserAsync(model, redirectionLink);
                if (result.Succeeded)
                {
                    return RedirectToAction("RegisterCompleted");
                }
                else
                {
                    if (result.Errors.ToList().Count() > 1)
                        ModelState.AddModelError("Email en uso", result.Errors.ToList()[1]);
                    else
                        ModelState.AddModelError("Email en uso", result.Errors.ToList()[0]);
                }
            }

            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            authLogic.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RegisterCompleted()
        {
            return View();
        }

        public async Task<ActionResult> EmailValidation(string userID, string code)
        {
            bool validate = await authLogic.ValidateEmailToken(userID, code);
            if (validate)
                return RedirectToAction("EmailValidated");
            else
                return RedirectToAction("ErrorInValidation");
        }

        public ActionResult EmailValidated()
        {
            if (User.Identity.IsAuthenticated)
            {
                authLogic.SignOut();
                return RedirectToAction("EmailValidated", "Account");
            }
            return View();
        }

        public ActionResult ErrorInValidation()
        {
            if (User.Identity.IsAuthenticated)
            {
                authLogic.SignOut();
                return RedirectToAction("ErrorInValidation", "Account");
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var redirectUrl = Url.Action("ResetPassword", "Account", null, null, Request.Url.Host);

                var result = await authLogic.ForgotPassword(model, redirectUrl);
                if (result.Success)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    if (result.ErrorType == ResetPasswordError.InvalidEmail)
                        ModelState.AddModelError("c_InvalidEmail", "El campo Email no es valido ");
                    if (result.ErrorType == ResetPasswordError.Exception)
                        ModelState.AddModelError("c_Exception", "Ha ocurrido un error");
                }
            }

            return View(model);
        }

        public ActionResult ResetPassword(string userID, string code)
        {
            var resetPassword = new ResetPasswordViewModel { Code = code, UserId = userID };
            return View(resetPassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await authLogic.ResetPassword(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (string error in result.Errors)
                        ModelState.AddModelError(error, error);
                }
            }

            return View(model);
        }
    }
}