using Entity.Concrete;

namespace Cargomda.UI.Models
{
    public class SubCategoryViewModel
    {
        public int SubcategoryId { get; set; }
        public string SubcategoryName { get; set; }
        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; }
    }
}
