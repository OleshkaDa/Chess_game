using Model.Core.GameLogic;
using Model.Core.Interfaces;
using Model.Data.Serialization;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace chessTraim
{
    public partial class GameWindow : Window
    {
        private GameState _game;
        private string _saveFilePath;
        private GameSerializerBase _serializer;
        private Button? _selectedButton;
        private (int Row, int Col)? _selectedPosition;

        //private readonly Brush LightCellBrush = new SolidColorBrush(Color.FromRgb(240, 217, 181));
        //private readonly Brush DarkCellBrush = new SolidColorBrush(Color.FromRgb(181, 136, 99));
        private readonly Brush LightCellBrush =
    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DEB887"));

        private readonly Brush DarkCellBrush =
            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A0522D"));
        private readonly Brush HighlightBrush = new SolidColorBrush(Color.FromRgb(120, 200, 120));

        public GameWindow(GameState game, string saveFilePath, GameSerializerBase serializer)
        {
            InitializeComponent();
            _game = game;
            _saveFilePath = saveFilePath;
            _serializer = serializer;
            InitializeBoard();
            UpdateBoard();
            UpdateUI();
        }

        private void InitializeBoard()
        {
            ChessBoardGrid.Children.Clear();
            ChessBoardGrid.RowDefinitions.Clear();
            ChessBoardGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < 8; i++)
            {
                ChessBoardGrid.RowDefinitions.Add(new RowDefinition());
                ChessBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var button = new Button();
                    button.SetValue(Grid.RowProperty, row);
                    button.SetValue(Grid.ColumnProperty, col);
                    button.FontSize = 32;
                    button.FontFamily = new FontFamily("Segoe UI Symbol");

                    button.Padding = new Thickness(0);

                    button.HorizontalContentAlignment = HorizontalAlignment.Stretch;

                    button.VerticalContentAlignment = VerticalAlignment.Stretch;

                    button.BorderThickness = new Thickness(0);

                    //button.Background = Brushes.Transparent;
                    bool isLightCell = (row + col) % 2 == 0;

                    if (isLightCell)
                    {
                        button.Background = LightCellBrush;
                    }
                    else
                    {
                        button.Background = DarkCellBrush;
                    }


                    button.BorderThickness = new Thickness(0);
                    button.Click += Cell_Click;
                    ChessBoardGrid.Children.Add(button);
                }
            }
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            if (_game.IsGameOver)
            {
                return;
            }

            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            int row = Grid.GetRow(button);

            int col = Grid.GetColumn(button);

            IPiece piece = _game.Board[row, col];

            // Ничего не выбрано
            if (_selectedPosition == null)
            {
                if (piece != null)
                {
                    if (piece.Color == _game.CurrentTurn)
                    {
                        _selectedButton = button;

                        _selectedPosition = (row, col);

                        HighlightAvailableMoves(row, col);
                    }
                }

                return;
            }

            // Проверка что позиция ещё существует
            if (_selectedPosition == null)
            {
                return;
            }

            int fromRow = _selectedPosition.Value.Row;

            int fromCol = _selectedPosition.Value.Col;

            IPiece selectedPiece = _game.Board[fromRow, fromCol];

            // Если выбранная фигура исчезла
            if (selectedPiece == null)
            {
                _selectedButton = null;

                _selectedPosition = null;

                ClearHighlights();

                return;
            }

            // Перевыбор своей фигуры
            if (piece != null)
            {
                if (piece.Color == _game.CurrentTurn)
                {
                    _selectedButton = button;

                    _selectedPosition = (row, col);

                    HighlightAvailableMoves(row, col);

                    return;
                }
            }

            bool moved = false;

            try
            {
                moved = _game.TryMove(fromRow, fromCol, row, col);
            }
            catch
            {
                _selectedButton = null;

                _selectedPosition = null;

                ClearHighlights();

                return;
            }

            if (moved)
            {
                _selectedButton = null;

                _selectedPosition = null;

                ClearHighlights();

                UpdateBoard();

                UpdateUI();

                if (_game.IsGameOver)
                {
                    MessageBox.Show(
                        "Игра окончена! " + _game.Winner,
                        "Конец игры",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
            }
        }
        //private void Cell_Click(object sender, RoutedEventArgs e)
        //{
        //    if (_game.IsGameOver)
        //    {
        //        return;
        //    }

        //    Button button = sender as Button;

        //    if (button == null)
        //    {
        //        return;
        //    }

        //    int row = Grid.GetRow(button);

        //    int col = Grid.GetColumn(button);

        //    IPiece piece = _game.Board[row, col];

        //    // Если фигура ещё не выбрана
        //    if (_selectedPosition == null)
        //    {
        //        if (piece != null)
        //        {
        //            if (piece.Color == _game.CurrentTurn)
        //            {
        //                _selectedButton = button;

        //                _selectedPosition = (row, col);

        //                HighlightAvailableMoves(row, col);
        //            }
        //        }

        //        return;
        //    }

        //    int fromRow = _selectedPosition.Value.Row;

        //    int fromCol = _selectedPosition.Value.Col;

        //    // Если нажали на свою фигуру — просто перевыбрать
        //    if (piece != null && piece.Color == _game.CurrentTurn)
        //    {
        //        _selectedButton = button;

        //        _selectedPosition = (row, col);

        //        HighlightAvailableMoves(row, col);

        //        return;
        //    }

        //    bool moved = _game.TryMove(fromRow, fromCol, row, col);

        //    if (moved)
        //    {
        //        ClearHighlights();

        //        _selectedButton = null;

        //        _selectedPosition = null;

        //        UpdateBoard();

        //        UpdateUI();

        //        if (_game.IsGameOver)
        //        {
        //            MessageBox.Show(
        //                "Игра окончена! " + _game.Winner,
        //                "Конец игры",
        //                MessageBoxButton.OK,
        //                MessageBoxImage.Information
        //            );
        //        }
        //    }
        //}

        private void HighlightAvailableMoves(int row, int col)
        {
            ClearHighlights();

            IPiece piece = _game.Board[row, col];

            if (piece == null)
            {
                return;
            }

            List<(int, int)> moves = piece.GetAvailableMoves(_game.Board);

            for (int i = 0; i < ChessBoardGrid.Children.Count; i++)
            {
                Button btn = ChessBoardGrid.Children[i] as Button;

                if (btn == null)
                {
                    continue;
                }

                int currentRow = Grid.GetRow(btn);

                int currentCol = Grid.GetColumn(btn);

                if (moves.Contains((currentRow, currentCol)))
                {
                    btn.Background = HighlightBrush;
                }
            }

            if (_selectedButton != null)
            {
                _selectedButton.Background = Brushes.Gold;
            }
        }
        //private void ClearHighlights()
        //{
        //    for (int i = 0; i < ChessBoardGrid.Children.Count; i++)
        //    {
        //        Button btn = ChessBoardGrid.Children[i] as Button;

        //        if (btn == null)
        //        {
        //            continue;
        //        }

        //        btn.Background = Brushes.Transparent;
        //    }
        //}
        private void ClearHighlights()
        {
            for (int i = 0; i < ChessBoardGrid.Children.Count; i++)
            {
                Button btn = ChessBoardGrid.Children[i] as Button;

                if (btn == null)
                {
                    continue;
                }

                int row = Grid.GetRow(btn);

                int col = Grid.GetColumn(btn);

                bool isLightCell = (row + col) % 2 == 0;

                if (isLightCell)
                {
                    btn.Background = LightCellBrush;
                }
                else
                {
                    btn.Background = DarkCellBrush;
                }
            }
        }

        private void UpdateBoard()
        {
            for (int i = 0; i < ChessBoardGrid.Children.Count; i++)
            {
                Button button = ChessBoardGrid.Children[i] as Button;

                if (button == null)
                {
                    continue;
                }

                int row = Grid.GetRow(button);

                int col = Grid.GetColumn(button);

                IPiece piece = _game.Board[row, col];

                if (piece == null)
                {
                    button.Content = "";
                }
                else
                {
                    button.Content = GetPieceImage(piece);
                }
                //button.Background = Brushes.Transparent;
                bool isLightCell = (row + col) % 2 == 0;

                if (isLightCell)
                {
                    button.Background = LightCellBrush;
                }
                else
                {
                    button.Background = DarkCellBrush;
                }
            }
        }

        private void UpdateUI()
        {
            if (_game.CurrentTurn == PieceColor.White)
            {
                TurnTextBlock.Text = "Белые";
            }
            else
            {
                TurnTextBlock.Text = "Черные";
            }

            if (_game.IsInCheck(_game.CurrentTurn))
            {
                TurnTextBlock.Text += "  -  CHECK!";
            }

            if (_game.IsGameOver)
            {
                if (_game.Winner == "Ничья")
                {
                    MessageBox.Show(
                        "Ничья",
                        "GAME OVER"
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Победитель: " + _game.Winner,
                        "GAME OVER"
                    );
                }

                Close();

                return;
            }
        }
        private Image GetPieceImage(IPiece piece)
        {
            string color = "";

            if (piece.Color == PieceColor.White)
            {
                color = "white";
            }
            else
            {
                color = "black";
            }

            string fileName = "";

            switch (piece.GetType().Name)
            {
                case "Pawn":
                    fileName = color == "white"
                        ? "Pawn_w.png"
                        : "Pawn_b.png";
                    break;

                case "Rook":
                    fileName = color == "white"
                        ? "Rook_w.png"
                        : "Rook_b.png";
                    break;

                case "Knight":
                    fileName = color == "white"
                        ? "Knight_w.png"
                        : "Knight_b.png";
                    break;

                case "Bishop":
                    fileName = color == "white"
                        ? "Bishop_w.png"
                        : "Bishop_b.png";
                    break;

                case "Queen":
                    fileName = color == "white"
                        ? "Queen_w.png"
                        : "Queen_b.png";
                    break;

                case "King":
                    fileName = color == "white"
                        ? "King_w.png"
                        : "King_b.png";
                    break;
            }
            //switch (piece.GetType().Name)
            //{
            //    case "Pawn":
            //        fileName = color == "white"
            //            ? "WhitePawn.png"
            //            : "BlackPawn.png";
            //        break;

            //    case "Rook":
            //        fileName = color == "white"
            //            ? "WhiteRook.png"
            //            : "BlackRook.png";
            //        break;

            //    case "Knight":
            //        fileName = color == "white"
            //            ? "WhiteKnight.png"
            //            : "BlackKnight.png";
            //        break;

            //    case "Bishop":
            //        fileName = color == "white"
            //            ? "WhiteBishop.png"
            //            : "BlackBishop.png";
            //        break;

            //    case "Queen":
            //        fileName = color == "white"
            //            ? "WhiteQueen.png"
            //            : "BlackQueen.png";
            //        break;

            //    case "King":
            //        fileName = color == "white"
            //            ? "WhiteKing.png"
            //            : "BlackKing.png";
            //        break;
            //}

            //switch (piece.GetType().Name)
            //{
            //    case "Pawn":
            //        fileName = "chess-pawn-" + color + ".png";
            //        break;

            //    case "Rook":
            //        fileName = "chess-rook-" + color + ".png";
            //        break;

            //    case "Knight":
            //        fileName = "chess-knight-" + color + ".png";
            //        break;

            //    case "Bishop":
            //        fileName = "chess-bishop-" + color + ".png";
            //        break;

            //    case "Queen":
            //        fileName = "chess-queen-" + color + ".png";
            //        break;

            //    case "King":
            //        fileName = "chess-king-" + color + ".png";
            //        break;
            //}

            Image image = new Image();

            image.Source = new BitmapImage(
                new Uri(
                    "pack://application:,,,/Images/" + fileName
                )
            );

            image.Stretch = Stretch.Uniform;
            image.Margin = new Thickness(0);
            image.HorizontalAlignment = HorizontalAlignment.Center;
            image.VerticalAlignment = VerticalAlignment.Center;
            return image;
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_game.IsGameOver && _game.MoveHistory.Count > 0)
            {
                _game.UndoLastMove();
                _selectedPosition = null;
                _selectedButton = null;
                ClearHighlights();
                UpdateBoard();
                UpdateUI();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string extension =
    _serializer is JsonGameSerializer ? ".json" : ".xml";

                string fileName =
                    $"chess_save_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}{extension}";

                _saveFilePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "ChessSaves",
                    fileName);
                _serializer.Save(_game, _saveFilePath);

                Owner?.Activate();

                if (Owner is MainWindow main)
                {
                    main.CheckContinueButtonState();
                }

                MessageBox.Show("Игра сохранена!", "Сохранение",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GameWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_saveFilePath))
                {
                    return;
                }

                _serializer.Save(_game, _saveFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка сохранения при выходе: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }
    }
}