using Entity.Concrete;

namespace Business.Abstract
{
    public interface ICategoryService : IGenericService<Category>
    {
        List<Category> GetAllCategories();
        List<Category> GetSubCategories(int parentCategoryId);
        List<Category> GetMainCategories();
    }
}

// ICategoryService - Kategori nesneleri ile çalışmak için gerekli metotları içeren bir interface.

//IGenericService<Category>: Kategori işlemleri için temel metotları sağlar.