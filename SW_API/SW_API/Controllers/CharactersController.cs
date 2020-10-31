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

        // GET: api/Characters/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Characters
        [HttpPost]
        public void Create([FromBody] JsonElement item)
        {
            var command = new CreateCharacterCommand();
            command.Item = JsonConvert.DeserializeObject<CharacterItemDTO>(item.GetRawText());
            var result = _mediator.Send(command);


        }

        // PUT: api/Characters/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpPut]
        public async void Update([FromBody] JsonElement value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
