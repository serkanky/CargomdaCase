using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Entity.Concrete;
using Redis.Cache;

namespace Business.Concrete
{
    //CategoryManager : Category nesneleri ile ilgili işlemleri gerçekleştirmekten sorumludur.

    public class CategoryManager : ICategoryService
    {
        //_categoryDal constructor metodu üzerinden bir ICategoryDal türünden bir nesne alır. _categoryDal, db işlemlerini gerçekleştirmek için data access arayüzünü temsil eder.
        private readonly ICategoryDal _categoryDal;
        private readonly Context _context;
        private readonly RedisService _redisCache;
        public CategoryManager(ICategoryDal categoryDal, Context context, RedisService redisCache)
        {
            _categoryDal = categoryDal;
            _context = context;
            _redisCache = redisCache;
        }

        public List<Category> GetAllCategories()
        {
            var cachedCategories = _redisCache.Get<List<Category>>("AllCategories");
            if (cachedCategories != null)
            {

                var allCategories = _categoryDal.GetList();

                foreach (var category in cachedCategories)
                {
                    if (!allCategories.Any(c => c.CategoryId == category.CategoryId))
                    {
                        // Veritabanında olmayan kategoriyi Redis önbellekten kaldır
                        _redisCache.Remove($"Category:{category.CategoryId}");
                    }
                }

                return cachedCategories;
            }

            // Redis önbellekte veri yoksa veritabanından al
            var categories = _categoryDal.GetList();

            // Alınan veriyi Redis önbelleğe ekle
            _redisCache.Set("AllCategories", categories);

            return categories;
        }

        public List<Category> GetMainCategories()
        {
            var cachedMainCategories = _redisCache.Get<List<Category>>("MainCategories");
            if (cachedMainCategories != null)
            {
                return cachedMainCategories;
            }

            var mainCategories = _categoryDal.GetMainCategories();
            _redisCache.Set("MainCategories", mainCategories);

            return mainCategories;
        }

        public List<Category> GetSubCategories(int parentCategoryId)
        {
            return _context.Categories.Where(c => c.ParentCategoryId == parentCategoryId).ToList();
        }

        //category nesnesini veritabanından silmek için kullanılır.
        public void TDelete(Category t)
        {
            _categoryDal.Delete(t);

            // Redis önbellekten ilgili anahtarları kaldır
            _redisCache.Remove($"Category:{t.CategoryId}");
            _redisCache.Remove("AllCategories"); // Tüm kategorileri önbellekten temizle
        }

        //belirtilen ID değeri ile bir Category nesnesini veritabanından almak için kullanılır
        public Category TGetByID(int id)
        {
            var cachedCategory = _redisCache.Get<Category>($"Category:{id}");
            if (cachedCategory != null)
            {
                return cachedCategory;
            }

            var category = _categoryDal.GetByID(id);
            if (category != null)
            {
                _redisCache.Set($"Category:{id}", category);
            }

            return category;
        }

        //tüm Category nesnelerinin bir listesini veritabanından almak için kullanılır
        public List<Category> TGetList()
        {
            var cachedCategories = _redisCache.Get<List<Category>>("AllCategories");
            if (cachedCategories != null)
            {
                return cachedCategories;
            }

            var categories = _categoryDal.GetList();
            _redisCache.Set("AllCategories", categories);

            return categories;
        }
        //bir category nesnesini veritabanına eklemek için kullanılır.
        public void TInsert(Category t)
        {
            _categoryDal.Insert(t);

            // Yeni eklenen veriyi Redis önbelleğe ekle
            _redisCache.Set($"Category:{t.CategoryId}", t);
            _redisCache.Remove("AllCategories"); // Tüm kategorileri önbellekten temizle
        }

        //category nesnesini veritabanında güncellemek için kullanılır. 
        public void TUpdate(Category t)
        {
            _categoryDal.Update(t);

            // Güncellenen veriyi Redis önbellekte güncelle
            _redisCache.Set($"Category:{t.CategoryId}", t);
            _redisCache.Remove("AllCategories"); // Tüm kategorileri önbellekten temizle
        }
    }
}
