using AutoMapper;
using dotnet.DTOs.Skill;
using dotnet_rpg;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.DTOs.Fight;
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
            CreateMap<Skill, GetSkillDto>();
            CreateMap<Character, HighscoreDto>();
        }
    }
}
