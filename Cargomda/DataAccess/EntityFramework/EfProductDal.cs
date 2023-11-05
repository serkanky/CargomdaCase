using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Repositories;
using Entity.Concrete;

namespace DataAccess.EntityFramework
{
    public class EfProductDal : GenericRepository<Product>, IProductDal
    {

        public List<Product> GetProductsByCategory(int categoryId)
        {
            Context context = new Context();
            return context.Products.Where(p => p.CategoryId == categoryId).ToList();
        }
    }
}
