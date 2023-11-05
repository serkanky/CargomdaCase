using Entity.Concrete;

namespace Business.Abstract
{
    public interface IProductService : IGenericService<Product>
    {
        List<Product> GetProductsByCategory(int categoryId);
    }
}
// IProductService - Product nesneleri ile çalışmak için gerekli metotları içeren bir interface.

//IGenericService<Product>: Product işlemleri için temel metotları sağlar.