using Model.Core.Interfaces;
using Model.Core.Pieces;
using System;
using System.Collections.Generic;

namespace Model.Core.GameLogic
{
    public partial class GameState
    {
        public bool IsInCheck(PieceColor color)
        {
            int kingRow = -1;
            int kingCol = -1;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] != null)
                    {
                        if (Board[i, j] is King)
                        {
                            King king = (King)Board[i, j];

                            if (king.Color == color)
                            {
                                kingRow = i;
                                kingCol = j;
                            }
                        }
                    }
                }
            }

            if (kingRow == -1 || kingCol == -1)
            {
                return false;
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] != null)
                    {
                        if (Board[i, j].Color != color)
                        {
                            if (Board[i, j].CanMoveTo(kingRow, kingCol, Board))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public bool IsCheckmate(PieceColor color)
        {
            if (IsInCheck(color) == false)
            {
                return false;
            }

            return !HasAnyLegalMove(color);
        }

        private bool HasAnyLegalMove(PieceColor color)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] != null)
                    {
                        if (Board[i, j].Color == color)
                        {
                            for (int toRow = 0; toRow < 8; toRow++)
                            {
                                for (int toCol = 0; toCol < 8; toCol++)
                                {
                                    if (Board[i, j].CanMoveTo(toRow, toCol, Board))
                                    {
                                        IPiece tempPiece = Board[toRow, toCol];

                                        Board[toRow, toCol] = Board[i, j];
                                        Board[i, j] = null;

                                        bool stillInCheck = IsInCheck(color);

                                        Board[i, j] = Board[toRow, toCol];
                                        Board[toRow, toCol] = tempPiece;

                                        if (stillInCheck == false)
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}

//using Model.Core.Interfaces;
//using Model.Core.Pieces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Model.Core.GameLogic
//{
//    public partial class GameState
//    {
//        public bool IsInCheck(PieceColor color)
//        {
//            (int row, int col)? kingPos = null;
//            for (int i = 0; i < 8; i++)
//                for (int j = 0; j < 8; j++)
//                    if (Board[i, j] is King king && king.Color == color)
//                        kingPos = (i, j);

//            if (kingPos == null) return false;

//            for (int i = 0; i < 8; i++)
//                for (int j = 0; j < 8; j++)
//                    if (Board[i, j] != null && Board[i, j].Color != color)
//                        if (Board[i, j].CanMoveTo(kingPos.Value.row, kingPos.Value.col, Board))
//                            return true;

//            return false;
//        }

//        public bool IsCheckmate(PieceColor color)
//        {
//            if (!IsInCheck(color)) return false;
//            return !HasAnyLegalMove(color);
//        }

//        private bool HasAnyLegalMove(PieceColor color)
//        {
//            for (int i = 0; i < 8; i++)
//                for (int j = 0; j < 8; j++)
//                    if (Board[i, j] != null && Board[i, j].Color == color)
//                        for (int toRow = 0; toRow < 8; toRow++)
//                            for (int toCol = 0; toCol < 8; toCol++)
//                                if (Board[i, j].CanMoveTo(toRow, toCol, Board))
//                                {
//                                    var tempPiece = Board[toRow, toCol];
//                                    var tempCurrentTurn = CurrentTurn;
//                                    Board[toRow, toCol] = Board[i, j];
//                                    Board[i, j] = null;
//                                    bool stillInCheck = IsInCheck(color);
//                                    Board[i, j] = Board[toRow, toCol];
//                                    Board[toRow, toCol] = tempPiece;

//                                    if (!stillInCheck) return true;
//                                }
//            return false;
//        }
//    }
//}

