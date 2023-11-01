namespace Entity.Concrete
{
    public class Category
    {
        //Category sınıfının propertyleri.
        
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool CategoryStatus { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<SubCategory> Subcategories { get; set; }
    }
}

//Virtual miras alındığı sınıflar tarafından değiştirilebilir.

//ICollection, Verilen sınıf neselerini içerir.