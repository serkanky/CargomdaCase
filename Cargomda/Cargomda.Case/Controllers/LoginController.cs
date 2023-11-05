using Cargomda.UI.Models;
using Entity.Concrete;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cargomda.UI.Controllers
{
    public class LoginController : Controller
    {
        #region Constructor
        //bağımlılık enjeksiyonu UserManager<AppUser> ve ILogger<UsersController>.
        //ILogger, hata kayıtlarının tutulmasına ve izlenmesine yardımcı olur.
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(SignInManager<AppUser> signInManager, ILogger<LoginController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }
        #endregion

        #region Index
        [HttpGet]
        //Index login sayfası.
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //user bilgileri, LoginViewModel sınıfından alınıyor.
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            // giriş için await _signInManager.PasswordSignInAsync metodu kullanılıyor. Giriş başarılıysa, Dashboard sayfasına yönlendiriliyor
            //Başarılı - Başarısız olması durumunda kayıt(log) tutulur.
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Kullanıcı girişi başarıyla gerçekleşti: {loginViewModel.UserName}");
                    return RedirectToAction("Index", "Dashboard");
                }

                _logger.LogInformation($"Kullanıcı girişi başarısız: {loginViewModel.UserName}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kullanıcı girişi sırasında bir hata oluştu");
            }

            return View();
        }
        #endregion

        #region LogOut
        [HttpGet]
        //LogOut işlemi gerçekleşiyor. 
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                _logger.LogInformation("Kullanıcı başarıyla çıkış yaptı");
                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kullanıcı çıkışı sırasında bir hata oluştu");
            }

            return RedirectToAction("Index", "Login");
        }
    }
}
#endregion