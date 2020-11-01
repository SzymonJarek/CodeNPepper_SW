using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.CharacterList.Query.GetCharacters;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace ApplicationLayer.CharacterItem.Command.UpdateCharacter
{
    public class UpdateCharacterCommand : IRequest<bool>
    {

        public CharacterItemDTO Item { get; set; }

        public class UpdateCharacterCommandHandler : IRequestHandler<UpdateCharacterCommand, bool>
        {
            private readonly string _MySqlConnString;

            public UpdateCharacterCommandHandler(IConfiguration conf)
            {
                _MySqlConnString = conf.GetConnectionString("MySql");
            }
            public async Task<bool> Handle(UpdateCharacterCommand request, CancellationToken cancellationToken)
            {
                //in our case we don't know how many properties we want to update. 
                //Maybe we want to remove some maybe add some.
                //so we have to first remove everything and then add 
                //if we had small entity then we could first read it, change it and save it;
                //this whole thing should go as transaction

                //Delete...
                int affectedRows = 0;
                using (var con = new MySqlConnection(_MySqlConnString))
                {
                    var characterID = con.Query<int>($"SELECT ID FROM characters WHERE Name = '{request.Item.Name}'").First();
                    var deleteCharacterSQLTransaction = $"DELETE FROM char_to_ep WHERE charID = {characterID};" +
                        $"DELETE FROM friends WHERE CharID = {characterID};" +
                        $"DELETE FROM characters WHERE ID = {characterID};";
                    con.Open();
                    using (var transaction = con.BeginTransaction())
                    {
                        affectedRows = con.Execute(deleteCharacterSQLTransaction, transaction: transaction);
                        transaction.Commit();
                    }

                    //insert
                    string insertCharacter = $"INSERT INTO characters (Name) VALUE ('{request.Item.Name}');";
                    con.ExecuteAsync(insertCharacter);
                    var newCharacterID = con.Query<int>("SELECT LAST_INSERT_ID();").First();
                    foreach (var episode in request.Item.Episodes)
                    {
                        try
                        {
                            var epID = con.Query<int>($"SELECT ID FROM episodes WHERE NAME = '{episode}';").First();
                            con.Execute($"INSERT INTO char_to_ep (epID,charID) VALUES ('{epID}','{newCharacterID}');");

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
                            con.Execute($"INSERT INTO friends(CharID,FriendID) VALUES ('{newCharacterID}','{friendID}');");

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
