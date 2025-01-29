using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_VillaAPI.Models.Dto
{
    public class VillaNumberCreateDTO
    {
        [Required]
        public int VillaNO { get; set; }
        //[Required]
        public string SpecialDetails { get; set; }

        [Required]
        public int VillaId { get; set; }

    }
}
