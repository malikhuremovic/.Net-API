using dotnet_rpg.DTOs.Fight;
using NETAPICOURSE.DTOs.Fight;

namespace dotnet_rpg.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttack);
        Task<ServiceResponse<SkillAttackResultDto>> SkillAttack(SkillAttackDto skillAttack);
        Task<ServiceResponse<FightResultDto>> FightStart(FightRequestDto fightRequest);
        Task<ServiceResponse<List<HighscoreDto>>> GetHighscore();


    }
}
