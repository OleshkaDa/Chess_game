using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using Model.Core.GameLogic;

namespace Model.Data.Serialization
{
    public class XmlGameSerializer : GameSerializerBase
    {
        public override void Save(GameState game, string filePath)
        {
            var serializer = new XmlSerializer(typeof(GameState));
            using var writer = new StreamWriter(filePath);
            serializer.Serialize(writer, game);
        }

        public override GameState Load(string filePath)
        {
            var serializer = new XmlSerializer(typeof(GameState));
            using var reader = new StreamReader(filePath);
            return (GameState)(serializer.Deserialize(reader) ?? new GameState());
        }

        public override bool Validate(string filePath)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(GameState));
                using var reader = new StreamReader(filePath);
                var game = serializer.Deserialize(reader) as GameState;
                return game != null && game.Board != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
