﻿using dotnet_rpg.DTOs.Character;

namespace dotnet_rpg.Services
{
    public interface ICharacterService { 
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();
        Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
        Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id);
        Task<ServiceResponse<GetCharacterDto>> ModifyCharacter(ModifyCharacterDto newCharacter);
    }
}
