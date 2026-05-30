using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model.Core.Interfaces;

namespace Model.Core.GameLogic
{
    public class Move
    {
        public int FromRow { get; set; }
        public int FromCol { get; set; }
        public int ToRow { get; set; }
        public int ToCol { get; set; }
        public IPiece Piece { get; set; }
        public IPiece? CapturedPiece { get; set; }

        public Move(int fromRow, int fromCol, int toRow, int toCol, IPiece piece, IPiece? capturedPiece)
        {
            FromRow = fromRow;
            FromCol = fromCol;
            ToRow = toRow;
            ToCol = toCol;
            Piece = piece;
            CapturedPiece = capturedPiece;
        }
        //надо для проверки условия с 6 ходами
        public Move Reverse()
        {
            return new Move(
                ToRow,
                ToCol,
                FromRow,
                FromCol,
                Piece,
                CapturedPiece);
        }

        // Перегрузка оператора
        public static bool operator ==(Move a, Move b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.FromRow == b.FromRow && a.FromCol == b.FromCol &&
                   a.ToRow == b.ToRow && a.ToCol == b.ToCol;
        }

        public static bool operator !=(Move a, Move b) => !(a == b);
        public override bool Equals(object obj) => this == obj as Move;
        public override int GetHashCode() => HashCode.Combine(FromRow, FromCol, ToRow, ToCol);
    }
}
