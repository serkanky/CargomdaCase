using Microsoft.AspNetCore.Mvc;

namespace Cargomda.UI.Controllers
{
    public class ErrorPageController : Controller
    {
        #region Just One Page NotFound 
        public IActionResult NotFound(int code)
        {
            return View();
        }
    }
}

//404 sayfasının konrtolleri. Dönen View`de 404 hata sayfam var.
#endregion