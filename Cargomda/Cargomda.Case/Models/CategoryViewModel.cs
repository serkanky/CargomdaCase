using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cargomda.UI.Models
{
    public class CategoryViewModel
    {
        public int SelectedCategoryId { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
    }

}
