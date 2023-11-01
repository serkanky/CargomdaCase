using Entity.Concrete;

namespace Business.Abstract
{
    public interface ISubCategoryService:IGenericService<SubCategory>
    {
        List<SubCategory> TGetListWithCategories();
        List<SubCategory> GetSubCategoriesByCategory(int categoryId);
    }
}


// ISubCategoryService - Alt Kategori nesneleri ile çalışmak için gerekli metotları içeren bir interface.

//IGenericService<SubCategory>: Alt Kategori işlemleri için temel metotları sağlar.

//List<SubCategory> TGetListWithCategories(): alt kategorilerin listesini çekerken her bir alt kategoriye ait ana kategoriyi de getirmek için kullanılır.