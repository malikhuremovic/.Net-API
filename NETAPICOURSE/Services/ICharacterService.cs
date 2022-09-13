using dotnet_rpg.DTOs.Character;

namespace dotnet_rpg.Services
{
    public interface ICharacterService { 
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int id);
        Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
        Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id);
        Task<ServiceResponse<GetCharacterDto>> ModifyCharacter(ModifyCharacterDto newCharacter);
        Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id);

    }
}
