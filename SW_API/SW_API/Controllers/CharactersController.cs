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
using Swashbuckle.AspNetCore.Filters;
using Newtonsoft.Json.Linq;

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
        /// <summary>
        /// Returns list of all characters
        /// </summary>
        /// <returns>list of all characters</returns>
        [HttpGet]
        public async Task<CharactersListVM> Get()
        {
            return await _mediator.Send(new GetCharactersQuery());
        }

        // POST: api/Characters
        /// <summary>
        /// Creates new character
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "name": "Han Solo",
        ///         "episodes": [
        ///             "NEWHOPE",
        ///             "EMPIRE",
        ///             "JEDI"
        ///         ],
        ///         "friends": [
        ///             "Luke Skywalker",
        ///             "Leia Organa",
        ///             "R2-D2"
        ///          ]
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        /// <response code="201">Character created properly</response>
        /// <response code="409">Character already exists, try to update it with PUT command</response>    
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


        /// <summary>
        /// Updates existing character
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     {
        ///         "name": "Han Solo",
        ///         "episodes": [
        ///             "NEWHOPE",
        ///             "EMPIRE",
        ///             "JEDI"
        ///         ],
        ///         "friends": [
        ///             "Luke Skywalker",
        ///             "Leia Organa",
        ///             "R2-D2"
        ///          ]
        ///     }
        ///
        /// </remarks> 
        [HttpPut]
        public async void Update([FromBody] JsonElement item)
        {
            var command = new UpdateCharacterCommand();
            command.Item = JsonConvert.DeserializeObject<CharacterItemDTO>(item.GetRawText());
            var result = await _mediator.Send(command);
        }

        // DELETE: api/Characters/5
        /// <summary>
        /// Deletes existing character
        /// </summary>
        /// <remarks> 
        /// ID must be the same as ID in DB 
        /// </remarks>
        [HttpDelete("{id}")]
        public async void Delete(int id)
        {
            //to delete it properly i should provide ID, for now you can't see it anywhere(just in DB)
            var command = new DeleteCharacterCommand() { characterID = id };
            var result = await _mediator.Send(command);
        }
    }
}
