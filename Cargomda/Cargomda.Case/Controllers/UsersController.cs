using Entity.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Cargomda.UI.Controllers
{
    public class UsersController : Controller
    {
        #region Constructor

        //bağımlılık enjeksiyonu UserManager<AppUser> ve ILogger<UsersController>.
        //ILogger, hata kayıtlarının tutulmasına ve izlenmesine yardımcı olur.
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<AppUser> userManager, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        #endregion


        #region Index
        public IActionResult Index()
        {
            //_userManager.Users.ToList() ile tüm kullanıcılar alınır ve bir view ile  gösterilir.
            try
            {
                var values = _userManager.Users.ToList();
                _logger.LogInformation("Kullanıcılar başarıyla listelendi.");
                return View(values);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kullanıcılar listelenirken bir hata oluştu.");
                return View();
            }
        }
    }
}
#endregion