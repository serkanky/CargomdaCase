using DataAccess.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Cargomda.UI.ViewComponents.Dashboard
{
    public class _DashboardStatisticPartial : ViewComponent
    {
        Context context = new Context();
        public IViewComponentResult Invoke()
        {
            ViewBag.totalProductCount = context.Products.Count();
            ViewBag.totalCategoryCount = context.Categories.Count();
            ViewBag.totalMemberCount = context.Users.Count();
            return View();
        }
    }
}
