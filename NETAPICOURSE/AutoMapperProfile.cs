using AutoMapper;
using dotnet_rpg.DTOs.Character;

namespace NETAPICOURSE
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<ModifyCharacterDto, Character>();
        }
    }
}
