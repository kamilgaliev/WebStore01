using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;
        private readonly ILogger<AccountController> _Logger;

        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager, ILogger<AccountController> Logger)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
            _Logger = Logger;
        }

        #region Register
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            _Logger.LogInformation("Регистрация пользователя {0}, время: {1}", Model.UserName, DateTime.Now);
            if(!ModelState.IsValid) return View(Model);

            var user = new User
            {
                UserName = Model.UserName
            };

            var registration_result = await _UserManager.CreateAsync(user, Model.Password);

            if(registration_result.Succeeded)
            {
                _Logger.LogInformation("Пользователь {0} успешно зарегистрирован, время: {1}", Model.UserName, DateTime.Now);

                await _UserManager.AddToRoleAsync(user, Role.User);

                _Logger.LogInformation("Пользователь {0} наделен ролью {1}", Model.UserName, Role.User);

                await _SignInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }

            _Logger.LogWarning("В процессе регистрации пользователя {0}, возникли ошибки:  {1} . Время: {2}", Model.UserName, 
                string.Join(',', registration_result.Errors.Select(e => e.Description)),
                DateTime.Now);

            foreach (var error in registration_result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(Model);
        }
        #endregion

        #region Login
        public IActionResult Login(string ReturnUrl) => View( new LoginViewModel { ReturnUrl = ReturnUrl});

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            _Logger.LogInformation("Вход пользователя {0} в систему. Время: {1}", Model.UserName, DateTime.Now);
            if (!ModelState.IsValid) return View(Model);

            var login_result = await _SignInManager.PasswordSignInAsync(
                Model.UserName,
                Model.Password,
                Model.RememberMe,
#if DEBUG 
                false
#else
                true
#endif
                );

            if (login_result.Succeeded)
            {
                _Logger.LogInformation("Пользовтель {0} вошел в систему. Время: {1}", Model.UserName, DateTime.Now);
                return LocalRedirect(Model.ReturnUrl ?? "/");
            }

            
            ModelState.AddModelError("", "Неверное имя пользователя или пароль!");

            return View(Model);
        }
#endregion

        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        } 
    }
}
