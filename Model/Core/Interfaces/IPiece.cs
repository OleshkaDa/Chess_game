using System.Collections.Generic;

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

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//namespace Model.Core.Interfaces
//{
//    public interface IPiece
//    {
//        (int Row, int Col) Position { get; set; }
//        PieceColor Color { get; }
//        bool CanMoveTo(int newRow, int newCol, IPiece[,] board);
//        List<(int, int)> GetAvailableMoves(IPiece[,] board);
//        void Move(int toRow, int toCol);
//    }
//}
