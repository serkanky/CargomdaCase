using Entity.Concrete;

namespace DataAccess.Abstract
{
    public interface ICategoryDal : IGenericDal<Category>
    {
        //IGenericDal<Category> Interface`i genişletir. ICategoryDal'ın IGenericDal<Category>'dan miras aldığını ve Category veritabanı işlemlerini tanımlar.
    }
}
