using Microsoft.AspNetCore.Identity;

namespace Entity.Concrete
{
    public class AppRole : IdentityRole<int>
    {
        //int ROL Kimlikleriin tamsayı olduğunu belirtmek için.
        //IdentityRole property`lerini içe aktarır.
    }
}
