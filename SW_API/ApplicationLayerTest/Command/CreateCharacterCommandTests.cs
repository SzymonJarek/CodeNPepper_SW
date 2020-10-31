using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace ApplicationLayerTest.Command
{
    public class CreateCharacterCommandTests
    {
        [Fact]
        public void Handle_checkIfItemToInsertIsNotNull()
        {
            //Assert
            var iconfMoq = new Mock<IConfiguration>().Object;

            var command = new CreateCharacterCommand()
            {
                Item = new ApplicationLayer.CharacterList.Query.GetCharacters.CharacterItemDTO()
                {
                    Name = "Luke Skywalker",
                    Episodes = new List<string> { "NEWHOPE", "EPIRE" },
                    Friends = new List<string> { "Han Solo", "R2-D2" }
                }
            };

            //ACT - add and read it in the next step - see if item was added
            var handler = new CreateCharacterCommand.CreateCharacterCommandHandler(iconfMoq);
            var result = handler.Handle(command,CancellationToken.None);


            Assert.NotNull(command.Item);


        }
    }
}
