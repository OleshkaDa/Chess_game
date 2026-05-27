using System;
using System.Collections.Generic;

using Model.Core.Interfaces;

namespace Model.Core.Pieces
{
    public abstract class PieceBase : IPiece, IMovable
    {
        public (int Row, int Col) Position { get; set; }

        public PieceColor Color { get; set; }

        protected PieceBase(int row, int col, PieceColor color)
        {
            Position = (row, col);

            Color = color;
        }

        public abstract bool CanMoveTo(int newRow, int newCol, IPiece[,] board);

        public abstract List<(int, int)> GetAvailableMoves(IPiece[,] board);

        public virtual void Move(int toRow, int toCol)
        {
            Position = (toRow, toCol);
        }

        public virtual bool IsValidMove(int toRow, int toCol, IPiece[,] board)
        {
            return CanMoveTo(toRow, toCol, board);
        }

        // Перегрузка оператора ==
        public static bool operator ==(PieceBase a, PieceBase b)
        {
            if (ReferenceEquals(a, null))
            {
                if (ReferenceEquals(b, null))
                {
                    return true;
                }

                return false;
            }

            if (ReferenceEquals(b, null))
            {
                return false;
            }

            if (a.Position != b.Position)
            {
                return false;
            }

            if (a.Color != b.Color)
            {
                return false;
            }

            return true;
        }

        // Перегрузка оператора !=
        public static bool operator !=(PieceBase a, PieceBase b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            PieceBase piece = obj as PieceBase;

            return this == piece;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Color);
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
//    public abstract class PieceBase : IPiece, IMovable
//    {
//        public (int Row, int Col) Position { get; set; }
//        public PieceColor Color { get; set; }

//        protected PieceBase(int row, int col, PieceColor color)
//        {
//            Position = (row, col);
//            Color = color;
//        }

//        public abstract bool CanMoveTo(int newRow, int newCol, IPiece[,] board);
//        public abstract List<(int, int)> GetAvailableMoves(IPiece[,] board);

//        public virtual void Move(int toRow, int toCol)
//        {
//            Position = (toRow, toCol);
//        }

//        public virtual bool IsValidMove(int toRow, int toCol, IPiece[,] board)
//        {
//            return CanMoveTo(toRow, toCol, board);
//        }

//        // Перегрузка оператора == и !=
//        public static bool operator ==(PieceBase a, PieceBase b)
//        {
//            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
//            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
//            return a.Position == b.Position && a.Color == b.Color;
//        }

//        public static bool operator !=(PieceBase a, PieceBase b)
//        {
//            return !(a == b);
//        }

//        public override bool Equals(object obj) => this == obj as PieceBase;
//        public override int GetHashCode() => HashCode.Combine(Position, Color);
//    }
//}
