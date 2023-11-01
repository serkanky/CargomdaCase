using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cargomda.UI.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        #region Just One Index Page
        public IActionResult Index()
        {
            return View();
        }
    }
}
#endregion