using System.ComponentModel.DataAnnotations;

namespace Entity.Concrete
{
    public class Category
    {
        //Category sınıfının propertyleri.
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Lütfen boş geçmeyiniz.")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Lütfen boş geçmeyiniz.")]
        public string CategoryDescription { get; set; }

        [Required(ErrorMessage = "Lütfen boş geçmeyiniz.")]
        public bool CategoryStatus { get; set; }

        public int? ParentCategoryId { get; set; }
        public virtual Category? ParentCategory { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}

//Virtual miras alındığı sınıflar tarafından değiştirilebilir.

//ICollection, Verilen sınıf neselerini içerir.