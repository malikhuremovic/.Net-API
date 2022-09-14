using AutoMapper;
using dotnet_rpg;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.DTOs.Weapon;

namespace NETAPICOURSE
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>().ForMember(dest => dest.Weapon, act => act.MapFrom(src => src.Weapon)); ;
            CreateMap<AddCharacterDto, Character>();
            CreateMap<Weapon, GetWeaponDto>();
        }
    }
}
