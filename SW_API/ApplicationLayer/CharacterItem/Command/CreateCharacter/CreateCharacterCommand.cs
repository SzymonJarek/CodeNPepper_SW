using ApplicationLayer.CharacterList.Query.GetCharacters;
using Dapper;
using Domain;
using MediatR;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer
{
    public class CreateCharacterCommand : IRequest<bool>
    {
        public CharacterItemDTO Item { get; set; }
        public class CreateCharacterCommandHandler : IRequestHandler<CreateCharacterCommand, bool>
        {
            private readonly string _MySQLConnString;
            public CreateCharacterCommandHandler(IConfiguration conf)
            {
                _MySQLConnString = conf.GetConnectionString("MySql");

            }
            public async Task<bool> Handle(CreateCharacterCommand request, CancellationToken cancellationToken)
            {
                //probably it would be better to refer here to some Aggregate and access DB only with Aggregation Root
                // it might give us benefit of accessing db only once at a time( ex. within mutex);
                using (var con = new MySqlConnection(_MySQLConnString))
                {
                    string insertCharacter = $"INSERT INTO characters (Name) VALUE ('{request.Item.Name}');";
                    try
                    {
                        var res = con.Execute(insertCharacter);
                    }
                    catch (MySqlException ex)
                    {
                        //this character already exists
                        //should return info that it already exists
                        //here we can log SQL errors
                        return false;
                    }

                    var characterID = con.Query<int>("SELECT LAST_INSERT_ID();").First();
                    foreach (var episode in request.Item.Episodes)
                    {
                        try
                        {
                            var epID = con.Query<int>($"SELECT ID FROM episodes WHERE NAME = '{episode}';").First();
                            con.Execute($"INSERT INTO char_to_ep (epID,charID) VALUES ('{epID}','{characterID}');");

                        }
                        catch (InvalidOperationException ioe)
                        {
                            //in case operation failed //this should go in transaction!!!
                        }
                    }

                    foreach (var friend in request.Item.Friends)
                    {
                        try
                        {
                            var friendID = con.Query<int>($"SELECT ID FROM characters WHERE NAME = '{friend}';").First();
                            con.Execute($"INSERT INTO friends(CharID,FriendID) VALUES ('{characterID}','{friendID}');");

                        }
                        catch (InvalidOperationException ioe)
                        {
                            //in case operation failed //this should go in transaction!!!
                        }
                    }
                }
                return true;
            }
        }
    }
}
