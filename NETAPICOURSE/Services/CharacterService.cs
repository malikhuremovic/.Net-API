
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Claims;

namespace dotnet_rpg.Services
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>
        {
            new Character(),
            new Character { Id = 1, Name = "Sam" }
        };
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor contextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character tempCharacter = _mapper.Map<Character>(character);
            tempCharacter.User = await _context.Users.FirstOrDefaultAsync(c => c.Id == GetUserId());
            _context.Characters.Add(tempCharacter);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        private int GetUserId() => int.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.Where(c => c.User.Id == GetUserId()).ToListAsync();
            if(dbCharacters.Count == 0)
            {
                response.Message = "No characters found for user with ID: " + GetUserId();
            }
            response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
                if(character != null)
                {
                    serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
                }
                else
                {
                    throw new Exception("The character of yours with ID: " + id + " does not exist.");
                }
            }
            catch(Exception exc)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = exc.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> ModifyCharacter(ModifyCharacterDto newCharacter)
        {
            var serviceResponse= new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == newCharacter.Id && c.User.Id == GetUserId());
                if (character != null)
                {
                    character.Name = String.IsNullOrEmpty(newCharacter.Name) ? character.Name : newCharacter.Name;
                    character.HitPoints = newCharacter.HitPoints >= 0 ? newCharacter.HitPoints : character.HitPoints;
                    character.Strength = newCharacter.Strength >= 0 ? newCharacter.Strength : character.Strength;
                    character.Defense = newCharacter.Defense >= 0 ? newCharacter.Defense : character.Defense;
                    character.Intelligence = newCharacter.Intelligence >= 0 ? newCharacter.Intelligence : character.Intelligence;
                    character.Class = ((int)newCharacter.Class) > 0 ? newCharacter.Class : character.Class;
                }
                else
                {
                    throw new Exception("You do not possess a character with the ID of: " + newCharacter.Id + ".");
                }
                try {
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = await _context.Characters
                        .Where(c => c.User.Id == GetUserId())
                        .Select(c => _mapper.Map<GetCharacterDto>(c))
                        .ToListAsync();
                    serviceResponse.Message = "You have successfully modified a character with ID: " + newCharacter.Id;
                }
                catch(Exception exc)
                {
                    serviceResponse.Message = exc.Message;
                    serviceResponse.Success = false;
                }
            }
            catch(Exception exc)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = exc.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
                if (character != null)
                {
                    _context.Characters.Remove(character);
                    await _context.SaveChangesAsync();
                    serviceResponse.Data = serviceResponse.Data = await _context.Characters
                            .Where(c => c.User.Id == GetUserId())
                            .Select(c => _mapper.Map<GetCharacterDto>(c))
                            .ToListAsync();
                    serviceResponse.Message = "You have successfully deleted a characted with ID: " + id;
                }
                else
                {
                    throw new Exception("You do not posess a character with the ID of: " + id);
                }
            }catch(Exception exc)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = exc.Message;
            }
            return serviceResponse;
        }


    }
}
