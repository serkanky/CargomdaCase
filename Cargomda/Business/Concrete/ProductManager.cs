using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using Redis.Cache;

namespace Business.Concrete
{
    ////ProductManager : Product nesneleri ile ilgili işlemleri gerçekleştirmekten sorumludur.
    public class ProductManager : IProductService
    {
        //_productDal constructor metodu üzerinden bir IProductDal türünden bir nesne alır. _productDal, db işlemlerini gerçekleştirmek için data access arayüzünü temsil eder.
        private readonly IProductDal _productDal;
        private readonly RedisService _redisCache;

        public ProductManager(IProductDal productDal, RedisService redisCache)
        {
            _productDal = productDal;
            _redisCache = redisCache;
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            var cachedProducts = _redisCache.Get<List<Product>>($"ProductsByCategory:{categoryId}");
            if (cachedProducts != null)
            {
                return cachedProducts;
            }

            var products = _productDal.GetProductsByCategory(categoryId);

            // Redis önbelleğe ekle
            _redisCache.Set($"ProductsByCategory:{categoryId}", products);

            return products;
        }

        //product nesnesini veritabanından silmek için kullanılır.
        public void TDelete(Product t)
        {
            _productDal.Delete(t);

            // Veritabanından ürün silindiğinde önbellekten de kaldır
            _redisCache.Remove($"Product:{t.ProductId}");
            _redisCache.Remove("AllProducts"); // Tüm ürünleri önbellekten temizle
        }

        //belirtilen ID değeri ile bir product nesnesini veritabanından almak için kullanılır
        public Product TGetByID(int id)
        {
            var cachedProduct = _redisCache.Get<Product>($"Product:{id}");
            if (cachedProduct != null)
            {
                return cachedProduct;
            }

            var product = _productDal.GetByID(id);
            if (product != null)
            {
                _redisCache.Set($"Product:{id}", product);
            }

            return product;
        }

        //tüm Product nesnelerinin bir listesini veritabanından almak için kullanılır
        public List<Product> TGetList()
        {
            var cachedProducts = _redisCache.Get<List<Product>>("AllProducts");
            if (cachedProducts != null)
            {
                return cachedProducts;
            }

            var products = _productDal.GetList();

            // Redis önbelleğe ekleyin
            _redisCache.Set("AllProducts", products);

            return products;
        }

        //bir Product nesnesini veritabanına eklemek için kullanılır.
        public void TInsert(Product t)
        {
            _productDal.Insert(t);

            // Yeni eklenen veriyi Redis önbelleğe ekle
            _redisCache.Set($"Product:{t.ProductId}", t);
            _redisCache.Remove("AllProducts");              // Tüm ürünleri önbellekten temizle
        }

        //Product nesnesini veritabanında güncellemek için kullanılır. 
        public void TUpdate(Product t)
        {
            _productDal.Update(t);

            // Güncellenen veriyi Redis önbellekte güncelle
            _redisCache.Set($"Product:{t.ProductId}", t);
            _redisCache.Remove("AllProducts");               // Tüm ürünleri önbellekten temizle
        }
    }
}
