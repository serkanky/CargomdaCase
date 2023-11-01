using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace Cargomda.UI.Controllers
{

    public class CategoryController : Controller
    {
        #region Consturctor
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        #endregion

        #region Index
        public IActionResult Index()
        {
            //_categoryService.TGetList() ile kategori listesi çekiliyor. Başarılı durumda, bu veri bir görünümle birlikte döndürülür
            try
            {
                var values = _categoryService.TGetList();
                _logger.LogInformation("Kategori listesi başarıyla çekildi.");
                return View(values);
            }
            //ModelState.AddModelError ile hata mesajı eklenir ve hata kaydı _logger ile tutulıur.
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kategori listesi çekerken bir hata oluştu.");
                return View();
            }
        }
        #endregion

        #region AddCategory
        [HttpGet]
        //AddCategory kategori ekleme işlemlerimizi yönetiyor.
        //GET isteği ile sadece kategori ekleme formunu görüntüleriz.
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        //POST isteği ile yeni bir kategori ekleriz. başarılıysa, Index`e yönlendirilir ve _logger ile bir bilgilendirme kaydı oluşturulur. Hata olursa aynı sayfa geri döner.
        public IActionResult AddCategory(Category category)
        {
            try
            {
                _categoryService.TInsert(category);
                _logger.LogInformation("Kategori ekleme işlemi başarıyla tamamlandı.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kategori ekleme işlemi sırasında bir hata oluştu.");
                return View(category);
            }
        }
        #endregion

        #region DeleteCategory
        // kategoriyi siler ve Index yönlendirir. Silme başarılıysa, _logger ile bir bilgilendirme kaydı oluşturulur. Hata durumunda ise, sayfaya dönülür.
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var values = _categoryService.TGetByID(id);
                _categoryService.TDelete(values);
                _logger.LogInformation("Kategori silme işlemi başarıyla tamamlandı.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kategori silme işlemi sırasında bir hata oluştu.");
                return View();
            }
        }
        #endregion

        #region UpdateCategory
        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            //veriler List<SelectListItem> ile alınıyor ve liste "values" adı altında ViewBag aracılığıyla görünüme taşınıyor. Dropdown oluşturmak için kullandım.
            try
            {
                List<SelectListItem> values = (from x in _categoryService.TGetList()
                                               select new SelectListItem
                                               {
                                                   Text = x.CategoryName,
                                                   Value = x.CategoryId.ToString()
                                               }).ToList();
                ViewBag.v = values;
                var value = _categoryService.TGetByID(id);
                _logger.LogInformation($"Kategori güncelleme sayfası açıldı: {value.CategoryName}");
                return View(value);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kategori güncelleme sayfası açılırken bir hata oluştu.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            //veriler List<SelectListItem> ile alınıyor ve liste "values" adı altında ViewBag aracılığıyla görünüme taşınıyor. Dropdown oluşturmak için kullandım.
            try
            {
                List<SelectListItem> values = (from x in _categoryService.TGetList()
                                               select new SelectListItem
                                               {
                                                   Text = x.CategoryName,
                                                   Value = x.CategoryId.ToString()
                                               }).ToList();
                ViewBag.v = values;

                //Bulunan alt kategori güncellenir
                _categoryService.TUpdate(category);
                _logger.LogInformation("Kategori güncelleme işlemi başarıyla tamamlandı.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Kategori güncelleme işlemi sırasında bir hata oluştu.");
                return View(category);
            }
        }
    }
}
#endregion

