using Model.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Model.Core.GameLogic
{
    public partial class GameState
    {
        private int _consecutiveReversibleMoves = 0;

        private Move _lastMove;

        public void RecordMove(Move move)
        {
            if (_lastMove != null)
            {
                bool sameMoveBack = false;

                if (move.FromRow == _lastMove.ToRow)
                {
                    if (move.FromCol == _lastMove.ToCol)
                    {
                        if (move.ToRow == _lastMove.FromRow)
                        {
                            if (move.ToCol == _lastMove.FromCol)
                            {
                                sameMoveBack = true;
                            }
                        }
                    }
                }

                if (sameMoveBack)
                {
                    _consecutiveReversibleMoves++;
                }
                else
                {
                    _consecutiveReversibleMoves = 0;
                }
            }
            else
            {
                _consecutiveReversibleMoves = 0;
            }

            _lastMove = move;
        }

        public bool IsStalemateByReversibleMoves()
        {
            if (_consecutiveReversibleMoves >= 6)
            {
                return true;
            }

            return false;
        }

        public bool IsStalemate(PieceColor color)
        {
            if (IsInCheck(color))
            {
                return false;
            }

            if (HasAnyLegalMove(color))
            {
                return false;
            }

            return true;
        }
    }
}


//using Model.Core.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Model.Core.GameLogic
//{
//    public partial class GameState
//    {
//        private int _consecutiveReversibleMoves = 0;
//        private Move? _lastMove;

//        public void RecordMove(Move move)
//        {
//            if (_lastMove != null &&
//                move.FromRow == _lastMove.ToRow && move.FromCol == _lastMove.ToCol &&
//                move.ToRow == _lastMove.FromRow && move.ToCol == _lastMove.FromCol)
//            {
//                _consecutiveReversibleMoves++;
//            }
//            else
//            {
//                _consecutiveReversibleMoves = 0;
//            }

//            _lastMove = move;
//        }

//        public bool IsStalemateByReversibleMoves()
//        {
//            return _consecutiveReversibleMoves >= 6;
//        }

//        public bool IsStalemate(PieceColor color)
//        {
//            return !IsInCheck(color) && !HasAnyLegalMove(color);
//        }
//    }
//}
