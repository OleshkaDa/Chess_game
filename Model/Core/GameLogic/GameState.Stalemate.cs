using Model.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Model.Core.GameLogic
{
    public partial class GameState
    {
        private int _consecutiveReversibleMoves = 0;

        private Move _lastWhiteMove;
        private Move _lastBlackMove;

        public void RecordMove(Move move)
        {
            bool sameMoveBack = false;

            if (move.Piece.Color == PieceColor.White)
            {
                if (_lastWhiteMove != null)
                {
                    //ееееееееееееЕЕЕЕЕЕЕЕ Я НАШЛА ГДЕ ЗАЮЗАТЬ ПЕРЕГРУЗКУ АЛЛИЛУЯ ГОСПОДИ
                    sameMoveBack =
                        move == _lastWhiteMove.Reverse();
                }

                _lastWhiteMove = move;
            }
            else
            {
                if (_lastBlackMove != null)
                {
                    sameMoveBack =
                        move == _lastBlackMove.Reverse();
                }

                _lastBlackMove = move;
            }

            if (sameMoveBack)
            {
                _consecutiveReversibleMoves++;
            }
            else
            {
                _consecutiveReversibleMoves = 0;
            }


            //if (move.FromRow == _lastMove.ToRow)
            //{
            //    if (move.FromCol == _lastMove.ToCol)
            //    {
            //        if (move.ToRow == _lastMove.FromRow)
            //        {
            //            if (move.ToCol == _lastMove.FromCol)
            //            {
            //                sameMoveBack = true;
            //            }
            //        }
            //    }
            //}


            
            //    bool sameMoveBack = move == _lastMove.Reverse();

            //    if (sameMoveBack)
            //    {
            //        _consecutiveReversibleMoves++;
            //    }
            //    else
            //    {
            //        _consecutiveReversibleMoves = 0;
            //    }
            //}
            //else
            //{
            //    _consecutiveReversibleMoves = 0;
            //}

            //_lastMove = move;
        }

        public bool IsStalemateByReversibleMoves()
        {
            //12 типо 6 взаимнообратных ходов обоими игроками, но я не уверена что это правильно, может надо 6 ходов одного игрока, а другой может делать что угодно, я не знаю, в любом случае 12 это много и должно работать
            if (_consecutiveReversibleMoves >= 12)
            {
                return true;
            }

            return false;
        }
        // король не под шахом но ходов нету

        //шах шах падишах ...
            //Я не Шахерезада, сказок не будет, не надо слов, 
            //Нет на свете дороже клада, чем любовь, любовь. 
        //простите
        public bool IsStalemate(PieceColor color)
        {
            if (IsInCheck(color))
            {
                return false;
            }
            //хотя бы одна фигура может сходить
            if (HasAnyLegalMove(color))
            {
                return false;
            }

            return true;
        }


        public bool IsStalemate()
        {
            return IsStalemate(CurrentTurn);
        }
    }
}

