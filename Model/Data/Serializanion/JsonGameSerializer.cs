using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using Model.Core.GameLogic;
using Model.Core.Interfaces;
using Model.Core.Pieces;

namespace Model.Data.Serialization
{
    public class JsonGameSerializer : GameSerializerBase
    {
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
                        PieceData pieceData = new PieceData();

                        pieceData.Row = row;

                        pieceData.Col = col;

                        pieceData.Type = piece.GetType().Name;

                        pieceData.Color = piece.Color.ToString();

                        saveData.Pieces.Add(pieceData);
                    }
                }
            }

            JsonSerializerOptions options = new JsonSerializerOptions();

            options.WriteIndented = true;

            string json = JsonSerializer.Serialize(saveData, options);

            File.WriteAllText(filePath, json);
        }

        public override GameState Load(string filePath)
        {
            string json = File.ReadAllText(filePath);

            SaveData saveData = JsonSerializer.Deserialize<SaveData>(json);

            if (saveData == null)
            {
                return new GameState();
            }

            GameState game = new GameState();

            game.Board = new IPiece[8, 8];

            for (int i = 0; i < saveData.Pieces.Count; i++)
            {
                PieceData pieceData = saveData.Pieces[i];

                PieceColor color;

                if (pieceData.Color == "White")
                {
                    color = PieceColor.White;
                }
                else
                {
                    color = PieceColor.Black;
                }

                IPiece piece;

                if (pieceData.Type == "Pawn")
                {
                    piece = new Pawn(pieceData.Row, pieceData.Col, color);
                }
                else if (pieceData.Type == "Rook")
                {
                    piece = new Rook(pieceData.Row, pieceData.Col, color);
                }
                else if (pieceData.Type == "Knight")
                {
                    piece = new Knight(pieceData.Row, pieceData.Col, color);
                }
                else if (pieceData.Type == "Bishop")
                {
                    piece = new Bishop(pieceData.Row, pieceData.Col, color);
                }
                else if (pieceData.Type == "Queen")
                {
                    piece = new Queen(pieceData.Row, pieceData.Col, color);
                }
                else if (pieceData.Type == "King")
                {
                    piece = new King(pieceData.Row, pieceData.Col, color);
                }
                else
                {
                    throw new Exception("Unknown piece");
                }

                game.Board[pieceData.Row, pieceData.Col] = piece;
            }

            if (saveData.CurrentTurn == "White")
            {
                game.CurrentTurn = PieceColor.White;
            }
            else
            {
                game.CurrentTurn = PieceColor.Black;
            }

            game.IsGameOver = saveData.IsGameOver;

            game.Winner = saveData.Winner;

            return game;
        }

        public override bool Validate(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);

                SaveData data = JsonSerializer.Deserialize<SaveData>(json);

                if (data != null && data.Pieces.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }

    // Конвертер не нужный оставила на всякий случай потому что не помню зачем писала его в 1 варианте кода, но может он мне еще понадобится для чего-то другого

    //public class PieceConverter : JsonConverter<IPiece>
    //{
    //    public override IPiece Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        JsonDocument doc = JsonDocument.ParseValue(ref reader);

    //        JsonElement root = doc.RootElement;

    //        string type = root.GetProperty("PieceType").GetString();

    //        int row = root.GetProperty("Row").GetInt32();

    //        int col = root.GetProperty("Col").GetInt32();

    //        string color = root.GetProperty("Color").GetString();

    //        PieceColor pieceColor;

    //        if (color == "White")
    //        {
    //            pieceColor = PieceColor.White;
    //        }
    //        else
    //        {
    //            pieceColor = PieceColor.Black;
    //        }

    //        switch (type)
    //        {
    //            case "Pawn":
    //                return new Pawn(row, col, pieceColor);

    //            case "Rook":
    //                return new Rook(row, col, pieceColor);

    //            case "Knight":
    //                return new Knight(row, col, pieceColor);

    //            case "Bishop":
    //                return new Bishop(row, col, pieceColor);

    //            case "Queen":
    //                return new Queen(row, col, pieceColor);

    //            case "King":
    //                return new King(row, col, pieceColor);

    //            default:
    //                throw new Exception("Unknown piece");
    //        }
    //    }

    //    public override void Write(Utf8JsonWriter writer, IPiece value, JsonSerializerOptions options)
    //    {
    //        writer.WriteStartObject();

    //        writer.WriteString("PieceType", value.GetType().Name);

    //        writer.WriteNumber("Row", value.Position.Row);

    //        writer.WriteNumber("Col", value.Position.Col);

    //        writer.WriteString("Color", value.Color.ToString());

    //        writer.WriteEndObject();
    //    }
    //}
}
