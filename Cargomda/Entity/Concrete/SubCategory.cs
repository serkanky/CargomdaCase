using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Concrete
{
    public class SubCategory
    {
        //SubCategory sınıfının propertyleri.

        public int SubcategoryId { get; set; }
        public string SubcategoryName { get; set; }
        public bool Status { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

//Alt - Üst kategori ilişkisi.

//Foreign Kategoriler arası ilişkiyi belirtir.