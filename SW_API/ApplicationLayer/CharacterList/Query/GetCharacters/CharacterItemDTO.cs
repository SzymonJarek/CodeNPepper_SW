using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationLayer.CharacterList.Query.GetCharacters
{
    public class CharacterItemDTO
    {
        public string Name { get; set; }
        public List<string> Episodes { get; set; }
        public List<string> Friends { get; set; }
    }
}
