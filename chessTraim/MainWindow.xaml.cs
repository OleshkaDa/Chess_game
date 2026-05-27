using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Model.Core.GameLogic;
using Model.Data.Serialization;

namespace chessTraim
{
    public partial class MainWindow : Window
    {
        private string _saveFilePath = "";
        private GameSerializerBase? _serializer;
        private GameState? _currentGame;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string defaultFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ChessSaves");
            if (!Directory.Exists(defaultFolder))
                Directory.CreateDirectory(defaultFolder);

            SaveFolderTextBox.Text = defaultFolder;
            CheckContinueButtonState();
        }

        private void BrowseFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog();
            dialog.Title = "Выберите папку для сохранения игр";

            if (dialog.ShowDialog() == true)
            {
                SaveFolderTextBox.Text = dialog.FolderName;
                CheckContinueButtonState();
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            _currentGame = new GameState();
            OpenGameWindow();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string format =
                    (FormatComboBox.SelectedItem as ComboBoxItem)?
                    .Content.ToString() ?? "JSON";

                _serializer =
                    format == "JSON"
                    ? new JsonGameSerializer()
                    : new XmlGameSerializer();

                var dialog = new OpenFileDialog();

                dialog.Filter = format == "JSON"
                    ? "JSON saves (*.json)|*.json"
                    : "XML saves (*.xml)|*.xml";

                dialog.InitialDirectory = SaveFolderTextBox.Text;

                if (dialog.ShowDialog() == true)
                {
                    _saveFilePath = dialog.FileName;

                    if (_serializer.Validate(_saveFilePath))
                    {
                        _currentGame = _serializer.Load(_saveFilePath);
                        OpenGameWindow();
                    }
                    else
                    {
                        ErrorTextBlock.Text = "Файл повреждён";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = $"Ошибка загрузки: {ex.Message}";
            }
        }

        public void CheckContinueButtonState()
        {
            string format =
                (FormatComboBox.SelectedItem as ComboBoxItem)?
                .Content.ToString() ?? "JSON";

            string extension = format == "JSON"
                ? "*.json"
                : "*.xml";

            bool hasSaves =
                Directory.GetFiles(SaveFolderTextBox.Text, extension).Length > 0;

            ContinueButton.IsEnabled = hasSaves;
        }

        private void OpenGameWindow()
        {
            string format = (FormatComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "JSON";
            GameSerializerBase serializer;

            if (format == "JSON")
                serializer = new JsonGameSerializer();
            else
                serializer = new XmlGameSerializer();

            //var gameWindow = new GameWindow(_currentGame!, _saveFilePath, serializer);
            //gameWindow.Owner = this;
            //gameWindow.ShowDialog();

            //CheckContinueButtonState();

            var gameWindow = new GameWindow(_currentGame!, _saveFilePath, serializer);

            gameWindow.Owner = this;

            this.Hide();

            gameWindow.ShowDialog();

            this.Show();

            CheckContinueButtonState();
        }
    }
}