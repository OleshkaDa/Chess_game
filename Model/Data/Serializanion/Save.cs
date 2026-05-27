using Model.Core.Interfaces;
using Model.Core.Pieces;

namespace Model.Data.Serialization
{
    // Этот класс только для сохранения (без интерфейсов)
    public class SaveData
    {
        public List<PieceData> Pieces { get; set; } = new();
        public string CurrentTurn { get; set; } = "White";
        public bool IsGameOver { get; set; }
        public string? Winner { get; set; }
        public List<MoveData> MoveHistory { get; set; } = new();
    }

    public class PieceData
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public string Type { get; set; } = "";
        public string Color { get; set; } = "";
    }

    public class MoveData
    {
        public int FromRow { get; set; }
        public int FromCol { get; set; }
        public int ToRow { get; set; }
        public int ToCol { get; set; }
        public string PieceType { get; set; } = "";
        public string PieceColor { get; set; } = "";
    }
}