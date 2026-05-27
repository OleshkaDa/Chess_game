using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Core.Interfaces;

namespace Model.Core.Pieces
{
    public class King : PieceBase
    {
        public King(int row, int col, PieceColor color) : base(row, col, color) { }

        public override bool CanMoveTo(int newRow, int newCol, IPiece[,] board)
        {
            if (newRow == Position.Row && newCol == Position.Col)
            {
                return false;
            }

            int dRow = Math.Abs(newRow - Position.Row);
            int dCol = Math.Abs(newCol - Position.Col);

            // Король ходит на 1 клетку в любую сторону
            if (dRow > 1 || dCol > 1) return false;

            return board[newRow, newCol] == null || board[newRow, newCol].Color != Color;
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
