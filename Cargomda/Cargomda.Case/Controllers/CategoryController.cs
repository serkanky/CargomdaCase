using Business.Abstract;
using Cargomda.UI.Models;
using Entity.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

[Authorize]
public class CategoryController : Controller
{
    #region Constructor

    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    #endregion


    #region Crud for all categories list.
    public IActionResult Crud()
    {
        var categories = _categoryService.TGetList();
        return View(categories);
    }

    #endregion


    #region Index
    public IActionResult Index()
    {
        try
        {
            var categories = _categoryService.TGetList();
            _logger.LogInformation("Kategori listesi başarıyla çekildi.");

            List<TreeViewNode> nodes = new List<TreeViewNode>();

            foreach (Category category in categories)
            {
                nodes.Add(new TreeViewNode
                {
                    id = category.CategoryId.ToString(),
                    parent = category.ParentCategoryId.HasValue ? category.ParentCategoryId.ToString() : "#",
                    text = category.CategoryName
                });
            }

            ViewBag.Json = JsonConvert.SerializeObject(nodes);

            return View(categories);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Kategori listesini çekerken bir hata oluştu.");
            return View();
        }
    }
    #endregion


    #region GetSubCategories
    [HttpGet]
    public IActionResult GetSubCategories(int parentCategoryId)
    {
        // Belirli bir üst kategoriye ait alt kategorileri almak için.
        var subCategories = _categoryService.GetSubCategories(parentCategoryId);

        if (subCategories == null)
        {
            return NotFound();
        }

        return Ok(subCategories);
    }

    public IActionResult GetMainCategories()
    {
       return Ok(_categoryService.GetMainCategories());
    }

    #endregion


    #region AddCategory GET
    [HttpGet]
    public IActionResult AddCategory()
    {
        try
        {
            List<SelectListItem> value1 = (from i in _categoryService.TGetList()
                                           select new SelectListItem
                                           {
                                               Text = i.CategoryName,
                                               Value = i.CategoryId.ToString()
                                           }).ToList();
            ViewBag.ktgr = value1;
            _logger.LogInformation("Kategori ekleme sayfası açıldı.");
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Kategori ekleme sayfası açılırken bir hata oluştu.");
            return View();
        }

    }

    #endregion 


    #region AddCategory POST
    [HttpPost]
    public IActionResult AddCategory(Category category)
    {
        try
        {
            _categoryService.TInsert(category);
            _logger.LogInformation("Kategori ekleme işlemi başarıyla tamamlandı.");
            return RedirectToAction("Crud");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kategori ekleme işlemi sırasında bir hata oluştu.");
            ModelState.AddModelError(string.Empty, "Kategori eklenirken bir hata oluştu. Lütfen tekrar deneyin.");
            return View(category);
        }
    }

    #endregion


    #region DeleteCategory
    public IActionResult DeleteCategory(int id)
    {
        try
        {
            var values = _categoryService.TGetByID(id);
            _categoryService.TDelete(values);
            _logger.LogInformation("Kategori silme işlemi başarıyla tamamlandı.");
            return RedirectToAction("Crud");
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Kategori silme işlemi sırasında bir hata oluştu.");
            return View("ErrorView", ex);
        }
    }

    #endregion


    #region UpdateCategory GET
    [HttpGet]
    public IActionResult UpdateCategory(int id)
    {
        try
        {
            List<SelectListItem> categories = _categoryService.TGetList()
        .Select(x => new SelectListItem
        {
            Text = x.CategoryName,
            Value = x.CategoryId.ToString()
        }).ToList();

            ViewBag.ktgr = new SelectList(categories, "Value", "Text");

            var category = _categoryService.TGetList().Where(x => x.CategoryId == id).FirstOrDefault();

            _logger.LogInformation($"Kategori güncelleme sayfası açıldı");
            return View(category);
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, "Kategori güncelleme sayfası açılırken bir hata oluştu.");
            return RedirectToAction("Index");
        }
    }

    #endregion


    #region UpdateCategory POST
    [HttpPost]
    public IActionResult UpdateCategory(Category category)
    {
        var categories = _categoryService.TGetList().Where(x => x.CategoryId == category.CategoryId).FirstOrDefault();
        categories.CategoryName = category.CategoryName;
        categories.ParentCategoryId = category.ParentCategoryId;
        categories.CategoryStatus = true;
        _categoryService.TUpdate(category);
        return RedirectToAction("Crud");
    }
    public List<Category> GetSubCategoriesByCategory(int categoryId)
    {
        return _categoryService.GetAllCategories()
            .Where(s => s.CategoryId == categoryId)
            .ToList();
    }
}
#endregion