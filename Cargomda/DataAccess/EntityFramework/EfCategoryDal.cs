using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Repositories;
using Entity.Concrete;

namespace DataAccess.EntityFramework
{
    public class EfCategoryDal : GenericRepository<Category>, ICategoryDal
    {
        public List<Category> GetMainCategories()
        {
            using var context = new Context();
            return context.Categories.Where(x => x.ParentCategoryId == null).ToList();
        }
    }
}
