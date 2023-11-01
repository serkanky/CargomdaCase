using Cargomda.UI.Models;
using Entity.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cargomda.UI.Controllers
{
    public class RegisterController : Controller
    {
        #region Constructor
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(UserManager<AppUser> userManager, ILogger<RegisterController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        #endregion

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kayıt sayfası açılırken bir hata oluştu.");
                return View();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            //Kullanıcının kullanıcının adı,soyadı,e-posta,kullanıcı adı ve şifresi AppUser aktarılıyor
            try
            {
                AppUser appUser = new AppUser()
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    UserName = model.UserName,
                };

                //Burada kayıt oluşuyor. eğer başarılı olursa giriş sayfasına yönlendiriyor.
                var result = await _userManager.CreateAsync(appUser, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Kullanıcı kaydı başarıyla oluşturuldu: {model.UserName}");
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    //Başarısız olursa hata kullanıcıya gösterir ve hataları düzeltmesi beklenir.
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    _logger.LogInformation("Kullanıcı kaydı oluşturulamadı.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kullanıcı kaydı sırasında bir hata oluştu.");
                return View();
            }
        }
    }
}
#endregion