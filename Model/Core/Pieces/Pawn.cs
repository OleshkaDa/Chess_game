using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Core.Interfaces;

namespace Model.Core.Pieces
{
    public class Pawn : PieceBase
    {
        public Pawn(int row, int col, PieceColor color) : base(row, col, color) { }

        public override bool CanMoveTo(int newRow, int newCol, IPiece[,] board)
        {
            if (newRow == Position.Row && newCol == Position.Col)
            {
                return false;
            }
            int direction = Color == PieceColor.White ? -1 : 1;
            int startRow = Color == PieceColor.White ? 6 : 1;

            //ход на 1 клетку вперёд
            if (newCol == Position.Col && newRow == Position.Row + direction && board[newRow, newCol] == null)
                return true;

            // зод на 2 клетки с начальной позиции
            if (newCol == Position.Col && newRow == Position.Row + 2 * direction &&
                Position.Row == startRow && board[Position.Row + direction, Position.Col] == null && board[newRow, newCol] == null)
                return true;

            //атака по диагонали
            if (Math.Abs(newCol - Position.Col) == 1 && newRow == Position.Row + direction &&
                board[newRow, newCol] != null && board[newRow, newCol].Color != Color)
                return true;

            return false;
        }

        public override List<(int, int)> GetAvailableMoves(IPiece[,] board)
        {
            var moves = new List<(int, int)>();
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                    if (CanMoveTo(row, col, board))
                        moves.Add((row, col));
            return moves;
        }
    }
}