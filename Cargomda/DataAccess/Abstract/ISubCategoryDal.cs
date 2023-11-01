using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ISubCategoryDal: IGenericDal<SubCategory>
    {
        //IGenericDal<SubCategory> Interface`i genişletir. ISubCategoryDal'ın IGenericDal<SubCategory>'den miras aldığını ve SubCategory veritabanı işlemlerini tanımlar.
    }
}
