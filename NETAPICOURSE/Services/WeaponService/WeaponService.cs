using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.DTOs.Weapon;
using dotnet_rpg.Utils;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IGetUserUtil _getUserUtil;

        public WeaponService (DataContext context, IMapper mapper, IGetUserUtil getUserUtil)
        {
            _context = context;
            _mapper = mapper;
            _getUserUtil = getUserUtil;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddNewWeapon(AddWeaponDto newWeapon)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && c.User.Id == _getUserUtil.GetUserId());

                if (character == null)
                {
                    throw new Exception("You do not possess a character with ID of: " + newWeapon.CharacterId);
                }

                Weapon weapon = new Weapon
                {
                    Character = character,
                    Damage = newWeapon.Damage,
                    Name = newWeapon.Name,
                };

                _context.Weapons.Add(weapon);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetCharacterDto>(character);
                response.Message = "Weapon " + newWeapon.Name + " is added to " + character.Name + " with the ID of: " + character.Id + ".";
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
