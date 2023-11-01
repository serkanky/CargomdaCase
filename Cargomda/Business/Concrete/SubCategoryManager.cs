using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    //SubCategoryManager : SubCategory nesneleri ile ilgili işlemleri gerçekleştirmekten sorumludur.
    public class SubCategoryManager : ISubCategoryService
    {
        //_subcategoryDal / _categoryService constructor metodu üzerinden bir ISubCategoryDal ve ICategoryService türünden bir nesne alır. _subcategoryDal - _categoryService db işlemlerini gerçekleştirmek için data access arayüzünü temsil eder.
        private readonly ISubCategoryDal _subcategoryDal;
        private readonly ICategoryService _categoryService;
        private readonly Context _context;

        public SubCategoryManager(ISubCategoryDal subcategoryDal, ICategoryService categoryService, Context context)
        {
            _subcategoryDal = subcategoryDal;
            _categoryService = categoryService;
            _context = context;
        }

        public List<SubCategory> GetSubCategoriesByCategory(int categoryId)
        {
            return _context.SubCategories
            .Where(s => s.CategoryId == categoryId)
            .ToList();
        }

        //subcategory nesnesini veritabanından silmek için kullanılır.
        public void TDelete(SubCategory t)
        {
            _subcategoryDal.Delete(t);
        }

        //belirtilen ID değeri ile bir Category nesnesini veritabanından almak için kullanılır
        public SubCategory TGetByID(int id)
        {
            return _subcategoryDal.GetByID(id);
        }

        //tüm subcategory nesnelerinin bir listesini veritabanından almak için kullanılır
        public List<SubCategory> TGetList()
        {
            return _subcategoryDal.GetList();
        }

        //alt kategorileri ve bunlara ait üst kategorileri içeren bir liste döndürmek için kullanılır. 
        public List<SubCategory> TGetListWithCategories()
        {
            // veritabanından tüm alt kategoriler alınır ve liste içerisine saklanır.
            var subCategories = _subcategoryDal.GetList();

            // subCategories listesinden her alt kategorinin CategoryId özelliği seçiliyo ve kimlikleri benzersiz hale getiriliyor (Distinct ile) ve bir liste haline getiriliyo.
            var categoryIds = subCategories.Select(subCategory => subCategory.CategoryId).Distinct().ToList();

            //tüm üst kategoriler alınıyor. categoryIds listesinde bulunan üst kategori kimliklerini içeren üst kategorileri filtrelemek için Where kullandım. Sonucu bir Dictionary'ye dönüştürdüm, bu şekilde her üst kategoriye ait kimlik ile birlikte üst kategorilere kolayca erişilebildim.
            var categories = _categoryService.TGetList().Where(category => categoryIds.Contains(category.CategoryId)).ToDictionary(category => category.CategoryId);

            // alt kategorinin CategoryId değeri, Dictionary`de bulunup bulunmadığı kontrol ediliyor. bulunursa bu alt kategoriye ait üst kategori ,category değişkenine atanıyor.
            foreach (var subCategory in subCategories)
            {
                if (categories.TryGetValue(subCategory.CategoryId, out var category))
                {
                    subCategory.Category = category;
                }

                // üst kategori yoksa bir varsayılan üst kategori oluşturuyorum ve alt kategoriye atadım.Bölyece her alt kategoriye mutlaka bir üst kategori atanmış oldu.
                else
                {

                    subCategory.Category = new Category { CategoryName = "Üst Kategori Yok" };
                }
            }

            return subCategories;
        }
        //bir subcategory nesnesini veritabanına eklemek için kullanılır.
        public void TInsert(SubCategory t)
        {
            _subcategoryDal.Insert(t);
        }

        //subcategory nesnesini veritabanında güncellemek için kullanılır. 
        public void TUpdate(SubCategory t)
        {
           _subcategoryDal.Update(t);
        }
    }
}
