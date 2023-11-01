using Business.Abstract;
using Cargomda.UI.Models;
using DataAccess.Concrete;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cargomda.UI.Controllers
{
    public class SubCategoryController : Controller
    {
        #region Constructor

        private readonly ISubCategoryService _subCategoryService;
        private readonly ILogger<SubCategoryController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly Context _context;

        public SubCategoryController(ISubCategoryService subCategoryService, ILogger<SubCategoryController> logger, ICategoryService categoryService, Context context)
        {
            _subCategoryService = subCategoryService;
            _logger = logger;
            _categoryService = categoryService;
            _context = context;
        }


        #endregion


        #region GetSubCategoriesByCategory
        public List<SubCategory> GetSubCategoriesByCategory(int categoryId)
        {
            return _context.SubCategories
                .Where(s => s.CategoryId == categoryId)
                .ToList();
        }
        public List<SelectListItem> GetCategoryList()
        {
            var categories = _categoryService.TGetList(); // Bu, kategorileri veritabanından alır.
            var categoryList = categories.Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryId.ToString()
            }).ToList();

            return categoryList;
        }
        #endregion

        #region Index
        public IActionResult Index(int selectedCategoryId = 0)
        {
            try
            {
                var categoryViewModel = new CategoryViewModel
                {
                    CategoryList = GetCategoryList(),
                    SubCategories = GetSubCategoriesByCategory(selectedCategoryId)
                };

                _logger.LogInformation("Alt Kategori başarıyla çekildi.");
                return View(categoryViewModel);
            }
            catch (Exception ex)
            {
                // hata kaydı _logger ile tutulur.
                _logger.LogError(ex, "Alt Kategori çekerken bir hata oluştu.");
                return View();
            }
        }
        #endregion

        #region AddSubCategory
        [HttpGet]
        //AddCategory kategori ekleme işlemlerimizi yönetiyor.
        //GET isteği ile sadece kategori ekleme formunu görüntüleriz.
        public IActionResult AddSubCategory()
        {
            SubCategoryViewModel viewModel = new SubCategoryViewModel();
            viewModel.Categories = _categoryService.GetAllCategories();
            return View(viewModel);

        }

        //POST isteği ile yeni bir alt kategori ekleriz. başarılıysa, Index`e yönlendirilir ve _logger ile bir bilgilendirme kaydı oluşturulur. Hata olursa aynı sayfa geri döner.
        [HttpPost]
        public IActionResult AddSubCategory(SubCategoryViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SubCategory subCategory = new SubCategory
                    {
                        SubcategoryId = viewModel.SubcategoryId,
                        SubcategoryName = viewModel.SubcategoryName,
                        CategoryId = viewModel.CategoryId // Kategori ID'sini SubCategory'e ata.
                    };
                    _subCategoryService.TInsert(subCategory);
                    _logger.LogInformation("Alt Kategori ekleme işlemi başarıyla tamamlandı.");
                    return RedirectToAction("Index");
                }
                else
                {
                    viewModel.Categories = _categoryService.GetAllCategories(); // Kategorileri doldur.
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Alt Kategori ekleme işlemi sırasında bir hata oluştu.");
                return View(viewModel);
            }
        }

        #endregion

        #region DeleteSubCategory

        //Alt kategoriyi siler ve Index yönlendirir. Silme başarılıysa, _logger ile bir bilgilendirme kaydı oluşturulur. Hata durumunda ise, sayfaya dönülür.
        public IActionResult DeleteSubCategory(int id)
        {
            try
            {
                var subCategory = _subCategoryService.TGetByID(id);
                _subCategoryService.TDelete(subCategory);
                _logger.LogInformation("Alt Kategori silme işlemi başarıyla tamamlandı.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Alt Kategori silme işlemi sırasında bir hata oluştu.");
                return View();
            }
        }
        #endregion

        #region UpdateSubCategory 
        [HttpGet]
        public IActionResult UpdateSubCategory(int id)
        {
            //veriler List<SelectListItem> ile alınıyor ve liste "values" adı altında ViewBag aracılığıyla görünüme taşınıyor. Dropdown oluşturmak için kullandım.
            try
            {
                List<SelectListItem> categories = (from x in _subCategoryService.TGetList()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.SubcategoryName,
                                                       Value = x.CategoryId.ToString()
                                                   }).ToList();
                ViewBag.Categories = categories;
                //ID İle alt kategori bulunur 
                var subCategory = _subCategoryService.TGetByID(id);
                _logger.LogInformation($"Alt Kategori güncelleme sayfası açıldı: {subCategory.SubcategoryName}");
                return View(subCategory);
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError ile hata mesajı eklenir ve hata kaydı _logger ile tutulıur.
                _logger.LogError(ex, "Alt Kategori güncelleme sayfası açılırken bir hata oluştu.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult UpdateSubCategory(SubCategory subCategory)
        {
            //veriler List<SelectListItem> ile alınıyor ve liste "values" adı altında ViewBag aracılığıyla görünüme taşınıyor. Dropdown oluşturmak için kullandım.
            try
            {
                List<SelectListItem> categories = (from x in _subCategoryService.TGetList()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.SubcategoryName,
                                                       Value = x.CategoryId.ToString()
                                                   }).ToList();
                ViewBag.Categories = categories;
                //Bulunan alt kategori güncellenir
                _subCategoryService.TUpdate(subCategory);
                _logger.LogInformation("Alt Kategori güncelleme işlemi başarıyla tamamlandı.");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Alt Kategori güncelleme işlemi sırasında bir hata oluştu.");
                return View(subCategory);
            }
        }
    }
}
#endregion