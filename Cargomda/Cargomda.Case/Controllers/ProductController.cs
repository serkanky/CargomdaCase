using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;
using System;
using System.Linq;

namespace Cargomda.UI.Controllers
{
    public class ProductController : Controller
    {
        #region Constructor
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<ProductController> _logger;
        private readonly ISubCategoryService _subCategoryService;

        public ProductController(IProductService productService, ICategoryService categoryService, ILogger<ProductController> logger, ISubCategoryService subCategoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
            _subCategoryService = subCategoryService;
        }

        #endregion

        #region Index
        public IActionResult Index()
        {
            //_productService.TGetList() ile kategori listesi çekiliyor. Başarılı durumda, bu veri bir görünümle birlikte döndürülür
            try
            {
                var values = _productService.TGetList();
                _logger.LogInformation("Ürün listesi başarıyla çekildi.");
                return View(values);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Ürün listesi çekerken bir hata oluştu.");
                return View();
            }
        }
        #endregion

        # region AddProduct
        [HttpGet]
        //AddProduct ürün ekleme işlemlerimizi yönetiyor.
        public IActionResult AddProduct()
        {
            //ViewBag ile values ve values1 verileri atadım. Bu şekilde ürünü hangi kategoriye ve alt kategoriye atayacağımızı seçebiliyoruz.
            try
            {
                List<SelectListItem> values = (from x in _categoryService.TGetList() 
                                               select new SelectListItem 
                                               { 
                                                   Text = x.CategoryName, 
                                                   Value = x.CategoryId.ToString() 
                                               }).ToList();

                List<SelectListItem> values1 = (from x in _subCategoryService.TGetList()
                                                select new SelectListItem
                                                {
                                                    Text = x.SubcategoryName,
                                                    Value = x.SubcategoryId.ToString()
                                                }).ToList();
                ViewBag.v = values;
                ViewBag.v1 = values1;

                _logger.LogInformation("Ürün ekleme sayfası açıldı.");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Ürün ekleme sayfası açılırken bir hata oluştu.");
                return View();
            }
        }

        [HttpPost]
        //Ürün bilgileri alınıyor ve TInsert ile db`ye ekleniyor.
        public IActionResult AddProduct(Product product)
        {
            //Olumlu - Olumsuz durumda loglar tutulur.
            try
            {
                _productService.TInsert(product);
                _logger.LogInformation($"Ürün eklemesi başarıyla tamamlandı: {product.ProductName}");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Ürün eklemesi sırasında bir hata oluştu.");
                return View();
            }
        }
        #endregion

        #region DeleteProduct
        public IActionResult DeleteProduct(int id)
        {
            //Ürün ID değeri yani TGetbyid ile bulunuyor ve TDelete ile siliniyor.
            //Olumlu - Olumsuz durumda loglar tutulur.
            try
            {
                var values = _productService.TGetByID(id);
                _productService.TDelete(values);
                _logger.LogInformation($"Ürün silme işlemi başarıyla tamamlandı: {values.ProductName}");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Ürün silme işlemi sırasında bir hata oluştu.");
                return RedirectToAction("Index");
            }
        }

        #endregion

        #region UpdateProduct
        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {

            //veriler List<SelectListItem> ile alınıyor ve liste "values" adı altında ViewBag aracılığıyla görünüme taşınıyor. Dropdown oluşturmak için kullandım.
            try
            {
                List<SelectListItem> values = (from x in _productService.TGetList()
                                               select new SelectListItem
                                               {
                                                   Text = x.ProductName,
                                                   Value = x.ProductId.ToString()
                                               }).ToList();
                List<SelectListItem> values1 = (from x in _subCategoryService.TGetList()
                                               select new SelectListItem
                                               {
                                                   Text = x.SubcategoryName,
                                                   Value = x.SubcategoryId.ToString()
                                               }).ToList();

                ViewBag.v = values;
                ViewBag.v1 = values1;
                var value = _productService.TGetByID(id);
                _logger.LogInformation($"Ürün güncelleme sayfası açıldı: {value.ProductName}");
                return View(value);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Ürün güncelleme sayfası açılırken bir hata oluştu.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {

            //veriler List<SelectListItem> ile alınıyor ve liste "values" adı altında ViewBag aracılığıyla görünüme taşınıyor. Dropdown oluşturmak için kullandım.
            try
            {
                List<SelectListItem> values = (from x in _productService.TGetList()
                                               select new SelectListItem
                                               {
                                                   Text = x.ProductName,
                                                   Value = x.ProductId.ToString()
                                               }).ToList();
                ViewBag.v = values;
                _productService.TUpdate(product);
                _logger.LogInformation($"Ürün güncelleme işlemi başarıyla tamamlandı: {product.ProductName}");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Ürün güncelleme işlemi sırasında bir hata oluştu.");
                return RedirectToAction("Index");
            }
        }
    }
}
#endregion