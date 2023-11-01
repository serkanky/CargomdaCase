using Business.Abstract;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Cargomda.UI.ViewComponents.Dashboard
{
    public class _DashboardLastProductPartial : ViewComponent
    {
        private readonly IProductService _productService;

        public _DashboardLastProductPartial(IProductService productService)
        {
            _productService = productService;
        }
        public IViewComponentResult Invoke()
        {
            var values = _productService.TGetList();
            return View(values);
        }
    }
}

//ViewComponent Controller Mantığı.

