using System.Collections.Generic;
//положение фигуры, методы движения.
namespace Model.Core.Interfaces
{
    public interface IPiece
    {
        (int Row, int Col) Position { get; set; }

        PieceColor Color { get; }

        bool CanMoveTo(
            int newRow,
            int newCol,
            IPiece[,] board
        );

        List<(int, int)> GetAvailableMoves(
            IPiece[,] board
        );

        void Move(
            int toRow,
            int toCol
        );
    }
}
