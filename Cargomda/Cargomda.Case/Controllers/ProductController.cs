using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cargomda.UI.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        #region Constructor
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ICategoryService categoryService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _logger = logger;
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
                _logger.LogInformation(ex, "Ürün listesini çekerken bir hata oluştu.");
                return View();
            }
        }
        #endregion

        #region AddProduct
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
              
                ViewBag.v = values;
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

        #region UpdateProduct GET

        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {

            try
            {
                // Tüm kategoriler
                var allCategories = _categoryService.TGetList();

                // Tüm kategorileri SelectListItem listesine dönüştürdüm.
                List<SelectListItem> categoryList = allCategories.Select(c => new SelectListItem
                {
                    Text = c.CategoryName,
                    Value = c.CategoryId.ToString()
                }).ToList();

                // Ürünü al
                var product = _productService.TGetByID(id);

                // Kategori ID'si ile mevcut kategoriyi seç.
                var selectedCategory = categoryList.FirstOrDefault(c => c.Value == product.CategoryId.ToString());
                if (selectedCategory != null)
                {
                    selectedCategory.Selected = true;
                }

                // Kategori listesini ViewBag ile görünüme taşı.
                ViewBag.CategoryList = categoryList;

                _logger.LogInformation($"Ürün güncelleme sayfası açıldı: {product.ProductName}");
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün güncelleme sayfası açılırken bir hata oluştu.");
                return RedirectToAction("Index");
            }
        }
        #endregion


        #region Update Product POST
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
        public IActionResult GetProductsByCategory(int categoryId)
        {
            try
            {
                var products = _productService.GetProductsByCategory(categoryId);
                return Json(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürünler getirilirken bir hata oluştu.");
                return StatusCode(500);
            }
        }
    }
}
#endregion
