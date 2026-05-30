using System;
using System.Collections.Generic;

using Model.Core.Interfaces;

namespace Model.Core.Pieces
{
    public class Queen : PieceBase
    {
        public Queen(int row, int col, PieceColor color) : base(row, col, color)
        {
        }

        public override bool CanMoveTo(int newRow, int newCol, IPiece[,] board)
        {
            if (newRow == Position.Row && newCol == Position.Col)
            {
                return false;
            }

            bool likeRook =
                Position.Row == newRow ||
                Position.Col == newCol;

            bool likeBishop = Math.Abs(newRow - Position.Row) == Math.Abs(newCol - Position.Col);

            if (!likeRook && !likeBishop)
            {
                return false;
            }

            int stepRow = 0;

            if (newRow > Position.Row)
            {
                stepRow = 1;
            }
            else if (newRow < Position.Row)
            {
                stepRow = -1;
            }

            int stepCol = 0;

            if (newCol > Position.Col)
            {
                stepCol = 1;
            }
            else if (newCol < Position.Col)
            {
                stepCol = -1;
            }

            int row = Position.Row + stepRow;

            int col = Position.Col + stepCol;

            while (row != newRow || col != newCol)
            {
                if (board[row, col] != null)
                {
                    return false;
                }

                row += stepRow;

                col += stepCol;
            }

            if (board[newRow, newCol] == null)
            {
                return true;
            }

            if (board[newRow, newCol].Color != Color)
            {
                return true;
            }

            return false;
        }

        public override List<(int, int)> GetAvailableMoves(IPiece[,] board)
        {
            List<(int, int)> moves = new List<(int, int)>();

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (CanMoveTo(row, col, board))
                    {
                        moves.Add((row, col));
                    }
                }
            }

            return moves;
        }
    }
}