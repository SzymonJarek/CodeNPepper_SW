using ApplicationLayer.CharacterList.Query.GetCharacters;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ApplicationLayerTest.Query
{
    public class GetCharactersTest
    {
        [Fact]
        public void Handle_ChecksReturnType()
        {
            //Arrange
            var iconfMock = new Mock<IConfiguration>().Object;
            var query = new GetCharactersQuery();
            var handle = new GetCharactersQuery.GetCharactersQueryHandler(iconfMock);
            //ACT
            var result = handle.Handle(query, CancellationToken.None);
            //Assert
            Assert.IsType<Task<CharactersListVM>>(result);
        }
    }
}
