using Business.Abstract;
using DataAccess.Abstract;
using Entity.Concrete;
using System.Collections.Generic;

namespace Business.Concrete
{
    //CategoryManager : Category nesneleri ile ilgili işlemleri gerçekleştirmekten sorumludur.

    public class CategoryManager : ICategoryService
    {
        //_categoryDal constructor metodu üzerinden bir ICategoryDal türünden bir nesne alır. _categoryDal, db işlemlerini gerçekleştirmek için data access arayüzünü temsil eder.
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public List<Category> GetAllCategories()
        {
            return _categoryDal.GetList();
        }

        //category nesnesini veritabanından silmek için kullanılır.
        public void TDelete(Category t)
        {
            _categoryDal.Delete(t);
        }

        //belirtilen ID değeri ile bir Category nesnesini veritabanından almak için kullanılır
        public Category TGetByID(int id)
        {
            return _categoryDal.GetByID(id);
        }

        //tüm Category nesnelerinin bir listesini veritabanından almak için kullanılır
        public List<Category> TGetList()
        {
            return _categoryDal.GetList();
        }
        //bir category nesnesini veritabanına eklemek için kullanılır.
        public void TInsert(Category t)
        {
            _categoryDal.Insert(t);
        }

        //category nesnesini veritabanında güncellemek için kullanılır. 
        public void TUpdate(Category t)
        {
            _categoryDal.Update(t);
        }
    }
}
