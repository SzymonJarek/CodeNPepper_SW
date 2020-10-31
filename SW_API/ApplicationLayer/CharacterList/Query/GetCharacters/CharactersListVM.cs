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
            CharactersList = new List<Domain.CharacterItem>();
        }
        public IList<Domain.CharacterItem> CharactersList { get; set; }
    }
}
