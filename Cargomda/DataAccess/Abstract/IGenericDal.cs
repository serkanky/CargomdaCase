namespace DataAccess.Abstract
{
    public interface IGenericDal<T> where T : class
    {
        void Insert(T t);        //T tipindeki yeni bir kaydı veritabanına eklemek için kullanılır.
        void Delete(T t);        //T tipindeki bir kaydı veritabanından kaldırmak için kullanılır.
        void Update(T t);        //T tipindeki bir kaydı veritabanında güncellemek veya değiştirmek için kullanılır.
        List<T> GetList();       //T tipindeki bir kaydı ID`ye göre veritabanından getirmek için kullanılır.
        T GetByID(int id);       //T tipindeki tüm kayıtları veritabanından getirmek için kullanılır.
    }
}
