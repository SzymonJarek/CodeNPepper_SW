using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class CharactersList
    {
        public CharactersList()
        {
            Items = new List<CharacterItem>();
        }

        public IList<CharacterItem> Items { get; set; }
    }
}
