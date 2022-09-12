
using AutoMapper;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.Models;
using System.ComponentModel;

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

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto character)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character temp_character = _mapper.Map<Character>(character);
            temp_character.Id = characters.Max(c => c.Id + 1);
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            return new ServiceResponse<List<GetCharacterDto>> { Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList() };
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.Find(c => c.Id == id);
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> ModifyCharacter(ModifyCharacterDto newCharacter)
        {
            int index = characters.FindIndex(c => c.Id == newCharacter.Id);
            characters[index] = new Character
            {
                Id = characters[index].Id,
                Name = String.IsNullOrEmpty(newCharacter.Name) ? characters[index].Name : newCharacter.Name,
                HitPoints = newCharacter.HitPoints > 0 ? newCharacter.HitPoints : characters[index].HitPoints,
                Strength = newCharacter.Strength > 0 ? newCharacter.Strength : characters[index].Strength,
                Defense = newCharacter.Defense > 0 ? newCharacter.Defense : characters[index].Defense,
                Intelligence = newCharacter.Intelligence > 0 ? newCharacter.Intelligence : characters[index].Intelligence,
                Class = ((int)newCharacter.Class) > 0 ? newCharacter.Class : characters[index].Class
            };
            return new ServiceResponse<GetCharacterDto> { Data = _mapper.Map<GetCharacterDto>(characters[index]) };
        }

    }
}
