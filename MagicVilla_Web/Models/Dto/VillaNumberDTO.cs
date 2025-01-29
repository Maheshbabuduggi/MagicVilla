using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaNumberDTO
    {
        [Required]
        public int VillaNO { get; set; }
        //[Required]
        public string SpecialDetails { get; set; }

        [Required]
        public int VillaId { get; set; }

    }
}
