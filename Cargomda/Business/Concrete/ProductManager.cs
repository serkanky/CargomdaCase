using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;

namespace Business.Concrete
{
    ////ProductManager : Product nesneleri ile ilgili işlemleri gerçekleştirmekten sorumludur.
    public class ProductManager : IProductService
    {
        //_productDal constructor metodu üzerinden bir IProductDal türünden bir nesne alır. _productDal, db işlemlerini gerçekleştirmek için data access arayüzünü temsil eder.
        private readonly IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        //product nesnesini veritabanından silmek için kullanılır.
        public void TDelete(Product t)
        {
            _productDal.Delete(t);
        }

        //belirtilen ID değeri ile bir product nesnesini veritabanından almak için kullanılır
        public Product TGetByID(int id)
        {
            return _productDal.GetByID(id);
        }

        //tüm Product nesnelerinin bir listesini veritabanından almak için kullanılır
        public List<Product> TGetList()
        {
            return _productDal.GetList();
        }

        //bir Product nesnesini veritabanına eklemek için kullanılır.
        public void TInsert(Product t)
        {
            _productDal.Insert(t);
        }

        //Product nesnesini veritabanında güncellemek için kullanılır. 
        public void TUpdate(Product t)
        {
            _productDal.Update(t);
        }
    }
}
