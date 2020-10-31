using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationLayer.CharacterList.Query.GetCharacters
{
    public class CharactersListVM
    {
        public CharactersListVM()
        {
            CharactersList = new List<CharacterItem>();
        }
        public IList<CharacterItem> CharactersList { get; set; }
    }
}
