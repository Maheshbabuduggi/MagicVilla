using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models
{
    public class LocalUser
    {
        public int Id { get; set; }
        [Required]
        public string UserName {  get; set; }
        [Required]
        public string Name
        {
            get; set;
        }
        public string Password {  get; set; }
        public string Role {  get; set; }

    }
}
