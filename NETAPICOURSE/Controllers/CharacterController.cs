﻿using dotnet_rpg.DTOs.Character;
using dotnet_rpg.Services;
using Microsoft.AspNetCore.Mvc;

namespace dotNet_rpg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetAllCharacters()
        {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> CreateSingle(AddCharacterDto character)
        {
            await _characterService.AddCharacter(character);
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> ModifySingle(ModifyCharacterDto character)
        {
            await _characterService.ModifyCharacter(character);
            return Ok(await _characterService.GetAllCharacters());
        }
    }
}