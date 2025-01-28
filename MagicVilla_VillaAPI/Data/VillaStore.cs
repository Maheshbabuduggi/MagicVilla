using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
            {
               new VillaDTO{Id=1,Name="Beach View",Occupancy=3,Sqft=293},
               new VillaDTO{Id=2,Name="House View",Occupancy=6,Sqft=728}
            };
    }
}
