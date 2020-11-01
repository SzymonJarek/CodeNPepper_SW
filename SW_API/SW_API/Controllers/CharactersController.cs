using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApplicationLayer.CharacterList.Query.GetCharacters;
using MediatR;
using System.Text.Json;
using ApplicationLayer;
using Newtonsoft.Json;
using ApplicationLayer.CharacterItem.Command.DelecteCharacter;
using ApplicationLayer.CharacterItem.Command.UpdateCharacter;
using System.Net.Http;
using System.Net;

namespace SW_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CharactersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/Characters
        [HttpGet]
        public async Task<CharactersListVM> Get()
        {
            return await _mediator.Send(new GetCharactersQuery());
        }

        // POST: api/Characters
        [HttpPost]
        public StatusCodeResult Create([FromBody] JsonElement item)
        {
            var command = new CreateCharacterCommand();
            command.Item = JsonConvert.DeserializeObject<CharacterItemDTO>(item.GetRawText());
            var result = _mediator.Send(command).Result;
            if (!result)
            {
                //not created, character already exists
                return new StatusCodeResult(409);
            }
            //everything ok
            return new StatusCodeResult(201);
        }

        [HttpPut]
        public async void Update([FromBody] JsonElement item)
        {
            var command = new UpdateCharacterCommand();
            command.Item = JsonConvert.DeserializeObject<CharacterItemDTO>(item.GetRawText());
            var result = await _mediator.Send(command);
        }

        // DELETE: api/Characters/5
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            //to delete it properly i should provide ID, for now you can't see it anywhere(just in DB)
            var command = new DeleteCharacterCommand() { characterID = id };
            var result = await _mediator.Send(command);
        }
    }
}
