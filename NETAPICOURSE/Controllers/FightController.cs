using dotnet_rpg.DTOs.Fight;
using dotnet_rpg.Services.FightService;
using Microsoft.AspNetCore.Mvc;
using NETAPICOURSE.DTOs.Fight;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;
        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto weaponAttack)
        {
            return Ok(await _fightService.WeaponAttack(weaponAttack));
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<SkillAttackResultDto>>> WeaponAttack(SkillAttackDto skillAttack)
        {
            return Ok(await _fightService.SkillAttack(skillAttack));
        }

        [HttpPost("Deathmatch")]
        public async Task<ActionResult<ServiceResponse<SkillAttackResultDto>>> Deathmatch(FightRequestDto fightRequest)
        {
            return Ok(await _fightService.FightStart(fightRequest));
        }

        [HttpGet("Highscore")]
        public async Task<ActionResult<ServiceResponse<SkillAttackResultDto>>> GetHighscore()
        {
            return Ok(await _fightService.GetHighscore());
        }
    }
}
