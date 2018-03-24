using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingWWW
{
    public class BoardGameDB
    {
        public int gameID;
        public string gameName;

        public BoardGameDB(int id, string name)
        {
            this.gameID = id;
            this.gameName = name;
        }

        public override string ToString()
        {
            return $"{gameID}, {gameName}";
        }
    }
}
