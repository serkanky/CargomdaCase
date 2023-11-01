using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IProductDal:IGenericDal<Product>
    {
        //IGenericDal<Product> Interface`i genişletir. IProductDal'ın IGenericDal<Product>'tan miras aldığını ve Product veritabanı işlemlerini tanımlar.
    }
}
