using System;
using System.Collections.Generic;
using Model.Core.Interfaces;
using Model.Core.Pieces;

namespace Model.Core.GameLogic
{
    public partial class GameState
    {
        public IPiece[,] Board { get; set; }

        public PieceColor CurrentTurn { get; set; }

        public bool IsGameOver { get; set; }

        public string Winner { get; set; }

        public List<Move> MoveHistory { get; set; }

        public GameState()
        {
            Board = new IPiece[8, 8];

            MoveHistory = new List<Move>();

            SetupInitialBoard();

            CurrentTurn = PieceColor.White;

            IsGameOver = false;

            Winner = "";
        }

        private void SetupInitialBoard()
        {
            // Пешки
            for (int col = 0; col < 8; col++)
            {
                Board[6, col] = new Pawn(6, col, PieceColor.White);

                Board[1, col] = new Pawn(1, col, PieceColor.Black);
            }

            // Ладьи
            Board[7, 0] = new Rook(7, 0, PieceColor.White);
            Board[7, 7] = new Rook(7, 7, PieceColor.White);

            Board[0, 0] = new Rook(0, 0, PieceColor.Black);
            Board[0, 7] = new Rook(0, 7, PieceColor.Black);

            // Кони
            Board[7, 1] = new Knight(7, 1, PieceColor.White);
            Board[7, 6] = new Knight(7, 6, PieceColor.White);

            Board[0, 1] = new Knight(0, 1, PieceColor.Black);
            Board[0, 6] = new Knight(0, 6, PieceColor.Black);

            // Слоны
            Board[7, 2] = new Bishop(7, 2, PieceColor.White);
            Board[7, 5] = new Bishop(7, 5, PieceColor.White);

            Board[0, 2] = new Bishop(0, 2, PieceColor.Black);
            Board[0, 5] = new Bishop(0, 5, PieceColor.Black);

            // Ферзи
            Board[7, 3] = new Queen(7, 3, PieceColor.White);

            Board[0, 3] = new Queen(0, 3, PieceColor.Black);

            // Короли
            Board[7, 4] = new King(7, 4, PieceColor.White);

            Board[0, 4] = new King(0, 4, PieceColor.Black);
        }

        public bool TryMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            IPiece piece = Board[fromRow, fromCol];

            if (piece == null)
            {
                return false;
            }

            if (piece.Color != CurrentTurn)
            {
                return false;
            }

            if (piece.CanMoveTo(toRow, toCol, Board))
            {
                IPiece targetPiece = Board[toRow, toCol];

                if (targetPiece is King)
                {
                    IsGameOver = true;

                    if (piece.Color == PieceColor.White)
                    {
                        Winner = "Белые";
                    }
                    else
                    {
                        Winner = "Черные";
                    }

                    return true;
                }
                Move move = new Move(
                    fromRow,
                    fromCol,
                    toRow,
                    toCol,
                    piece,
                    Board[toRow, toCol]
                );

                MoveHistory.Add(move);

                RecordMove(move);

                Board[toRow, toCol] = piece;

                Board[fromRow, fromCol] = null;

                piece.Move(toRow, toCol);

                if (CurrentTurn == PieceColor.White)
                {
                    CurrentTurn = PieceColor.Black;
                }
                else
                {
                    CurrentTurn = PieceColor.White;
                }

                if (IsCheckmate(CurrentTurn))
                {
                    IsGameOver = true;

                    if (CurrentTurn == PieceColor.White)
                    {
                        Winner = "Black";
                    }
                    else
                    {
                        Winner = "White";
                    }
                }
                else
                {
                    if (IsStalemate(CurrentTurn))
                    {
                        IsGameOver = true;

                        Winner = "Ничья";
                    }
                }

                return true;
            }

            return false;
        }

        public void UndoLastMove()
        {
            if (MoveHistory.Count == 0)
            {
                return;
            }

            Move lastMove = MoveHistory[MoveHistory.Count - 1];

            Board[lastMove.FromRow, lastMove.FromCol] = lastMove.Piece;

            Board[lastMove.ToRow, lastMove.ToCol] =
                lastMove.CapturedPiece;

            lastMove.Piece.Move(
                lastMove.FromRow,
                lastMove.FromCol
            );

            if (CurrentTurn == PieceColor.White)
            {
                CurrentTurn = PieceColor.Black;
            }
            else
            {
                CurrentTurn = PieceColor.White;
            }

            MoveHistory.RemoveAt(MoveHistory.Count - 1);
        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Model.Core.Interfaces;
//using Model.Core.Pieces;


//namespace Model.Core.GameLogic
//{
//    public partial class GameState
//    {
//        public IPiece?[,] Board { get; set; }
//        public PieceColor CurrentTurn { get; set; }
//        public bool IsGameOver { get; set; }
//        public string? Winner { get; set; }
//        public List<Move> MoveHistory { get; set; }

//        public GameState()
//        {
//            Board = new IPiece[8, 8];
//            MoveHistory = new List<Move>();
//            SetupInitialBoard();
//            CurrentTurn = PieceColor.White;
//            IsGameOver = false;
//        }

//        private void SetupInitialBoard()
//        {
//            // Пешки
//            for (int col = 0; col < 8; col++)
//            {
//                Board[6, col] = new Pawn(6, col, PieceColor.White);
//                Board[1, col] = new Pawn(1, col, PieceColor.Black);
//            }

//            // Ладьи
//            Board[7, 0] = new Rook(7, 0, PieceColor.White);
//            Board[7, 7] = new Rook(7, 7, PieceColor.White);
//            Board[0, 0] = new Rook(0, 0, PieceColor.Black);
//            Board[0, 7] = new Rook(0, 7, PieceColor.Black);

//            // Кони
//            Board[7, 1] = new Knight(7, 1, PieceColor.White);
//            Board[7, 6] = new Knight(7, 6, PieceColor.White);
//            Board[0, 1] = new Knight(0, 1, PieceColor.Black);
//            Board[0, 6] = new Knight(0, 6, PieceColor.Black);

//            // Слоны
//            Board[7, 2] = new Bishop(7, 2, PieceColor.White);
//            Board[7, 5] = new Bishop(7, 5, PieceColor.White);
//            Board[0, 2] = new Bishop(0, 2, PieceColor.Black);
//            Board[0, 5] = new Bishop(0, 5, PieceColor.Black);

//            // Ферзи
//            Board[7, 3] = new Queen(7, 3, PieceColor.White);
//            Board[0, 3] = new Queen(0, 3, PieceColor.Black);

//            // Короли
//            Board[7, 4] = new King(7, 4, PieceColor.White);
//            Board[0, 4] = new King(0, 4, PieceColor.Black);
//        }

//        public bool TryMove(int fromRow, int fromCol, int toRow, int toCol)
//        {
//            var piece = Board[fromRow, fromCol];
//            if (piece == null || piece.Color != CurrentTurn) return false;

//            if (piece.CanMoveTo(toRow, toCol, Board))
//            {
//                var move = new Move(fromRow, fromCol, toRow, toCol, piece, Board[toRow, toCol]);
//                MoveHistory.Add(move);
//                RecordMove(move); // для пата

//                Board[toRow, toCol] = piece;
//                Board[fromRow, fromCol] = null;
//                piece.Move(toRow, toCol);

//                // Проверка на мат
//                if (IsCheckmate(CurrentTurn))
//                {
//                    IsGameOver = true;
//                    Winner = CurrentTurn == PieceColor.White ? "Black" : "White";
//                }
//                else if (IsStalemate(CurrentTurn))
//                {
//                    IsGameOver = true;
//                    Winner = "Ничья";
//                }

//                CurrentTurn = CurrentTurn == PieceColor.White ? PieceColor.Black : PieceColor.White;
//                return true;
//            }
//            return false;
//        }

//        public void UndoLastMove()
//        {
//            if (MoveHistory.Count == 0) return;

//            var lastMove = MoveHistory[^1];
//            Board[lastMove.FromRow, lastMove.FromCol] = lastMove.Piece;
//            Board[lastMove.ToRow, lastMove.ToCol] = lastMove.CapturedPiece;
//            lastMove.Piece.Move(lastMove.FromRow, lastMove.FromCol);
//            CurrentTurn = CurrentTurn == PieceColor.White ? PieceColor.Black : PieceColor.White;
//            MoveHistory.RemoveAt(MoveHistory.Count - 1);
//        }
//    }
//}
