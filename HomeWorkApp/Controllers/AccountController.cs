using HomeWorkApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HomeWorkApp.Controllers
{
    public class AccountController : Controller
    {
        // Получить объект UserManager для взаимодействия с хранилищем пользователей
        private UserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
        }

        // Свойство для управления входом на сайт. Управялет аутентификационными куками
        //IAuthenticationManager.SignIn() создает аутентификационные куки
        // IAuthenticationManager.SignOut() удаляет аутентификационные куки
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public object ClasimsIdentity { get; private set; }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = userModel.UserName };
                // Создать пользователя
                IdentityResult result = await UserManager.CreateAsync(user, userModel.Password);
                // Если создание прошло успешно
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (string error in result.Errors)
                        ModelState.AddModelError("", error);
                }
            }
            return View(userModel);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                // Найти пользователя в базе
                User user = await UserManager.FindAsync(userModel.UserName, userModel.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                }
                else
                {
                    // Провайдер AspNet Identity использует аутентификацию на основе Claim объектов
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user, 
                        DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    //SignIn(объект конфигурации аутентификации, ClaimsIdentity)
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        // позволяет сохранить аутентификационные данные в браузере после закрытия его пользователем
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(userModel);
        }

        public ActionResult LogOut()
        {
            // Удалить аутентификационные куки в браузере
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }
    }
}