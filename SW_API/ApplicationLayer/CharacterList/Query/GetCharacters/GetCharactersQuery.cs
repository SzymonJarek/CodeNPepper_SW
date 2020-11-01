using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Domain;
using System.Linq;

namespace ApplicationLayer.CharacterList.Query.GetCharacters
{
    public class GetCharactersQuery : IRequest<CharactersListVM>
    {
        public class GetCharactersQueryHandler : IRequestHandler<GetCharactersQuery, CharactersListVM>
        {
            private readonly string _MySQLConnString;
            public GetCharactersQueryHandler(IConfiguration conf)
            {
                _MySQLConnString = conf.GetConnectionString("MySql");
            }
            public async Task<CharactersListVM> Handle(GetCharactersQuery request, CancellationToken cancellationToken)
            {
                var itemList = new CharactersListVM();
                using (var conn = new MySqlConnection(_MySQLConnString))
                {
                    var characters = conn.Query<string>("SELECT Name FROM characters;");

                    foreach (var name in characters)
                    {
                        Domain.CharacterItem item = new Domain.CharacterItem();
                        item.Name = name;
                        var characterId = conn.Query<int>($"SELECT ID FROM characters WHERE Name = '{name}';");
                        var query = $"SELECT Name FROM episodes WHERE ID IN (SELECT epID FROM char_to_ep WHERE charID = {characterId.First()});" +
                            $"SELECT Name FROM characters WHERE ID IN (SELECT FriendID From friends WHERE CharID = {characterId.First()});";

                        using(var multiQuery = conn.QueryMultiple(query))
                        {
                            item.Episodes = multiQuery.Read<string>().ToList();
                            item.Friends = multiQuery.Read<string>().ToList();
                        }
                        itemList.CharactersList.Add(item);
                    }

                }
                return itemList;
            }
        }
    }
}
