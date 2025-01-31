using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.VM
{
    public class VillaNumberVM
    {
        public VillaNumberVM()
        {
            
            CreateDTO=new VillaNumberCreateDTO();
        }

        public VillaNumberCreateDTO CreateDTO { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList {  get; set; }
    }
}
