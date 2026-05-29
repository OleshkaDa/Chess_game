using Model.Core.GameLogic;
using Model.Core.Interfaces;
using Model.Core.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Model.Data.Serialization
{
    public class XmlGameSerializer : GameSerializerBase
    {
        private XmlSerializer _serializer;

        public XmlGameSerializer()
        {
            _serializer = new XmlSerializer(typeof(SaveData));
        }

        public override void Save(GameState game, string filePath)
        {
            SaveData saveData = new SaveData();

            saveData.CurrentTurn = game.CurrentTurn.ToString();
            saveData.IsGameOver = game.IsGameOver;
            saveData.Winner = game.Winner;

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    IPiece piece = game.Board[row, col];

                    if (piece != null)
                    {
                        saveData.Pieces.Add(new PieceData
                        {
                            Row = row,
                            Col = col,
                            Type = piece.GetType().Name,
                            Color = piece.Color.ToString()
                        });
                    }
                }
            }

            using var writer = new StreamWriter(filePath);
            _serializer.Serialize(writer, saveData);
        }
        private IPiece CreatePiece(string type, string color, int row, int col)
        {
            PieceColor pieceColor =
                color == "White"
                ? PieceColor.White
                : PieceColor.Black;

            return type switch
            {
                "Pawn" => new Pawn(row, col, pieceColor),
                "Rook" => new Rook(row, col, pieceColor),
                "Knight" => new Knight(row, col, pieceColor),
                "Bishop" => new Bishop(row, col, pieceColor),
                "Queen" => new Queen(row, col, pieceColor),
                "King" => new King(row, col, pieceColor),

                _ => throw new Exception($"Неизвестная фигура: {type}")
            };
        }

        public override GameState Load(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                SaveData data =
                    (SaveData)(_serializer.Deserialize(reader) ?? new SaveData());

                GameState game = new GameState();

                // очищаем стандартную доску
                game.Board = new IPiece[8, 8];

                foreach (var pieceData in data.Pieces)
                {
                    IPiece piece = CreatePiece(
                        pieceData.Type,
                        pieceData.Color,
                        pieceData.Row,
                        pieceData.Col);

                    game.Board[pieceData.Row, pieceData.Col] = piece;
                }

                game.CurrentTurn =
                    data.CurrentTurn == "White"
                    ? PieceColor.White
                    : PieceColor.Black;

                game.IsGameOver = data.IsGameOver;
                game.Winner = data.Winner;

                return game;
            }
        }

        public override bool Validate(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    SaveData data =
                        _serializer.Deserialize(reader) as SaveData;

                    return data != null;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
