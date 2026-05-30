using System;
using System.Collections.Generic;

using Model.Core.Interfaces;

namespace Model.Core.Pieces
{
    public class Bishop : PieceBase
    {
        public Bishop(int row, int col, PieceColor color) : base(row, col, color)
        {
        }

        public override bool CanMoveTo(int newRow, int newCol, IPiece[,] board)
        {
            if (newRow == Position.Row &&
                newCol == Position.Col)
            {
                return false;
            }

            int rowDifference = Math.Abs(newRow - Position.Row);
            int colDifference = Math.Abs(newCol - Position.Col);

            if (rowDifference != colDifference)
            {
                return false;
            }

            int stepRow;

            if (newRow > Position.Row)
            {
                stepRow = 1;
            }
            else
            {
                stepRow = -1;
            }

            int stepCol;

            if (newCol > Position.Col)
            {
                stepCol = 1;
            }
            else
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

                row = row + stepRow;
                col = col + stepCol;
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

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Model.Core.Interfaces;

//namespace Model.Core.Pieces
//{
//    public class Bishop : PieceBase
//    {
//        public Bishop(int row, int col, PieceColor color) : base(row, col, color) { }

//        public override bool CanMoveTo(int newRow, int newCol, IPiece[,] board)
//        {
//            if (Math.Abs(newRow - Position.Row) != Math.Abs(newCol - Position.Col)) return false;

//            int stepRow = newRow > Position.Row ? 1 : -1;
//            int stepCol = newCol > Position.Col ? 1 : -1;

//            int row = Position.Row + stepRow;
//            int col = Position.Col + stepCol;

//            while (!(row == newRow && col == newCol))
//            {
//                if (board[row, col] != null) return false;
//                row += stepRow;
//                col += stepCol;
//            }

//            return board[newRow, newCol] == null || board[newRow, newCol].Color != Color;
//        }

//        public override List<(int, int)> GetAvailableMoves(IPiece[,] board)
//        {
//            var moves = new List<(int, int)>();
//            for (int row = 0; row < 8; row++)
//                for (int col = 0; col < 8; col++)
//                    if (CanMoveTo(row, col, board))
//                        moves.Add((row, col));
//            return moves;
//        }
//    }
//}
