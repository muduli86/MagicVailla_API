using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
            new VillaDTO { Id = 1, Name = "Mariot0" },
            new VillaDTO { Id = 2, Name = "Mariot1" },
            new VillaDTO { Id = 3, Name = "Mariot2" },
            new VillaDTO { Id = 4, Name = "Mariot3" },
        };
    }
}
