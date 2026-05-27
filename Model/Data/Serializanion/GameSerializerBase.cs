using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Core.GameLogic;

namespace Model.Data.Serialization
{
    public abstract class GameSerializerBase
    {
        public abstract void Save(GameState game, string filePath);
        public abstract GameState Load(string filePath);
        public abstract bool Validate(string filePath);
    }
}
