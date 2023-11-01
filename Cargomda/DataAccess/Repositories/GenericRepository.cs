using DataAccess.Abstract;
using DataAccess.Concrete;

namespace DataAccess.Repositories
{
    //CRUD için genel bir yapı kurdum, her bir modelin bu genel yapı üzerinden o işlemi gerçekleştirmesini sağladım.
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        //Veritabanından bir nesnenin silinmesini sağlar. 
        public void Delete(T t)
        {
            using var context = new Context();
            context.Remove(t);
            context.SaveChanges();
        }

        //ID değerine göre bir nesneyi veritabanından getirir.
        public T GetByID(int id)
        {
            using var context = new Context();
            return context.Set<T>().Find(id);
        }

        //Veritabanındaki tüm nesneleri bir liste olarak getirir.
        public List<T> GetList()
        {
            using var context = new Context();
            return context.Set<T>().ToList();
        }

        //Yeni bir nesnenin veritabanına eklenmesini sağlar. 
        public void Insert(T t)
        {
            using var context = new Context();
            context.Add(t);
            context.SaveChanges();
        }

        // Varolan bir nesnenin güncellenmesini sağlar.
        public void Update(T t)
        {
            using var context = new Context();
            context.Update(t);
            context.SaveChanges();
        }
    }
}
