using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Entity.Concrete
{
    public class AppUser : IdentityUser<int>
    {
        [Key]
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}

//int user kimlikleriin tamsayı olduğunu belirtmek için.
//IdentityUser property`lerini içe aktarır. ve Ek iki özellik ekler