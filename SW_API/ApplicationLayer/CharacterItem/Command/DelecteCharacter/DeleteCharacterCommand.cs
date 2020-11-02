using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.CharacterList.Query.GetCharacters;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace ApplicationLayer.CharacterItem.Command.DelecteCharacter
{
    public class DeleteCharacterCommand : IRequest<int>
    {
        public int characterID { get; set; }

        public class DeleteCharacterCommandHandler : IRequestHandler<DeleteCharacterCommand, int>
        {
            private readonly string _MySQLConnString;

            public DeleteCharacterCommandHandler(IConfiguration conf)
            {
                _MySQLConnString = conf.GetConnectionString("MySql");
            }

            public async Task<int> Handle(DeleteCharacterCommand request, CancellationToken cancellationToken)
            {
                int affectedRows = 0;
                using (var con = new MySqlConnection(_MySQLConnString))
                {
                    var deleteCharacterSQLTransaction = $"DELETE FROM char_to_ep WHERE charID = @characterIDp;" +
                        $"DELETE FROM friends WHERE CharID = @characterIDp;" +
                        $"DELETE FROM characters WHERE ID = @characterIDp;";
                    con.Open();
                    using (var transaction = con.BeginTransaction())
                    {
                        affectedRows = con.Execute(deleteCharacterSQLTransaction, new { characterIDp = request.characterID }, transaction: transaction);
                        transaction.Commit();
                    }
                }
                return affectedRows;
            }
        }
    }
}
