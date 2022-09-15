using dotnet_rpg.Data;
using dotnet_rpg.DTOs.Fight;
using dotnet_rpg.Services.FightService;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;

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

                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                damage -= new Random().Next(opponent.Defense);

                if (damage > 0)
                    opponent.HitPoints -= damage;

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

    }
}
