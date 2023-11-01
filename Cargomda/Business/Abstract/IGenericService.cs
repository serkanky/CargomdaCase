namespace Business.Abstract
{
    public interface IGenericService<T>
    {
        void TInsert(T t);             //T tipindeki yeni bir kaydı veritabanına eklemek için kullanılır.
        void TDelete(T t);            //T tipindeki bir kaydı veritabanından kaldırmak için kullanılır.
        void TUpdate(T t);           //T tipindeki bir kaydı veritabanında güncellemek veya değiştirmek için kullanılır.
        T TGetByID(int id);         //T tipindeki bir kaydı ID`ye göre veritabanından getirmek için kullanılır.
        List<T> TGetList();        //T tipindeki tüm kayıtları veritabanından getirmek için kullanılır.
    }
}
