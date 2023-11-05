using System.ComponentModel.DataAnnotations;

namespace Entity.Concrete
{
    public class Product
    {
        //Product sınıfının propertyleri.
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string Description { get; set; }
        public short UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Categories { get; set; }

    }
}

//Virtual ile LazyLoading yaptım. İhtiyaç duyulunca yüklenir.