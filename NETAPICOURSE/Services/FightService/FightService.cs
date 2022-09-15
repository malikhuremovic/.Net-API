using dotnet_rpg.Data;
using dotnet_rpg.DTOs.Fight;
using dotnet_rpg.Services.FightService;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using NETAPICOURSE.DTOs.Fight;

namespace dotnet.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        public FightService(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttack)
        {
            ServiceResponse<AttackResultDto> response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                    .Include("Weapon")
                    .FirstOrDefaultAsync(c => c.Id == weaponAttack.AttackerId);

                var opponent = await _context.Characters
                    .Include("Weapon")
                    .FirstOrDefaultAsync(c => c.Id == weaponAttack.OpponentId);

                if (attacker == null || opponent == null) throw new Exception("Character(s) do not exist");

                int damage = DoWeaponAttack(attacker, opponent);

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHp = attacker.HitPoints,
                    OpponentHp = opponent.HitPoints,
                    Damage = damage
                };

                if (opponent.HitPoints <= 0)
                    throw new Exception(opponent.Name + " has been defeated.");

                await _context.SaveChangesAsync();

            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
                response.Success = false;
            }
 

            return response;
        }

        private static int DoWeaponAttack(Character? attacker, Character? opponent)
        {
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defense);

            if (damage > 0)
                opponent.HitPoints -= damage;
            return damage;
        }
        private static int DoSkillAttack(Character? attacker, Character? opponent, Skill? skill)
        {
            int damage = skill.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defense);

            if (damage > 0)
                opponent.HitPoints -= damage;
            return damage;
        }

        public async Task<ServiceResponse<SkillAttackResultDto>> SkillAttack(SkillAttackDto skillAttack)
        {
            ServiceResponse<SkillAttackResultDto> response = new ServiceResponse<SkillAttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                    .Include("Skills")
                    .FirstOrDefaultAsync(c => c.Id == skillAttack.AttackerId);

                var opponent = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == skillAttack.OpponentId);

                if (attacker == null || opponent == null) throw new Exception("Character(s) do not exist");

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == skillAttack.SkillId);
                if (skill == null) throw new Exception("Skill does not exist.");
                int damage = DoSkillAttack(attacker, opponent, skill);

                response.Data = new SkillAttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    Skill = skill.Name,
                    AttackerHp = attacker.HitPoints,
                    OpponentHp = opponent.HitPoints,
                    Damage = damage
                };
                if (opponent.HitPoints <= 0)
                    throw new Exception(opponent.Name + " has been defeated.");
                await _context.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<ServiceResponse<FightResultDto>> FightStart(FightRequestDto fightRequest)
        {
            ServiceResponse<FightResultDto> response = new ServiceResponse<FightResultDto>
            {
                Data = new FightResultDto
                {
                    FightLog = new List<string>()
                }
            }; 

            try
            {
                var characters = await _context.Characters
                .Include("Weapon")
                .Include("Skills")
                .Where(c => fightRequest.CharacterIds.Contains(c.Id))
                .ToListAsync();

                bool defeated = false;

                while (!defeated)
                {
                    foreach (Character attacker in characters)
                    {
                        var opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = String.Empty;

                        bool useWeapon = new Random().Next(2) == 0;

                        if (useWeapon)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else
                        {
                            var skill = attacker.Skills[new Random().Next(2)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }
                        string logMsg = $"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage.";
                        response.Data.FightLog.Add(logMsg);

                        if(opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            logMsg = $"{opponent.Name} has been defeated!";
                            response.Data.FightLog.Add(logMsg);
                            logMsg = $"{attacker.Name} wins with {attacker.HitPoints} HP left!";
                            response.Data.FightLog.Add(logMsg);
                            break;
                        }
                    }
                }
                characters.ForEach(c =>
                {
                    c.Fights++;
                    c.HitPoints = 100;
                });

                await _context.SaveChangesAsync();
                response.Message = "Deathmatch has been played.";
            }
            catch (Exception exc)
            {
                response.Message = exc.Message;
                response.Success = false;
            }
            return response;
        }


    }
}
