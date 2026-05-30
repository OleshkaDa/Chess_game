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
        private GameSerializerBase _serializer;
        private GameState _currentGame;

        public string SaveFolderPath
        {
            get
            {
                return SaveFolderTextBox.Text;
            }
        }

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

            string format = "JSON";

            if (FormatComboBox.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)FormatComboBox.SelectedItem;
                format = selectedItem.Content.ToString();
            }

            string extension;

            if (format == "JSON")
            {
                extension = ".json";
            }
            else
            {
                extension = ".xml";
            }

            _saveFilePath = Path.Combine(SaveFolderTextBox.Text, $"autosave_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}{extension}");

            OpenGameWindow();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string format = "JSON";

                if (FormatComboBox.SelectedItem != null)
                {
                    ComboBoxItem selectedItem = (ComboBoxItem)FormatComboBox.SelectedItem;
                    format = selectedItem.Content.ToString();
                }

                if (format == "JSON")
                {
                    _serializer = new JsonGameSerializer();
                }
                else
                {
                    _serializer = new XmlGameSerializer();
                }

                OpenFileDialog dialog = new OpenFileDialog();

                if (format == "JSON")
                {
                    dialog.Filter = "JSON saves (*.json)|*.json";
                }
                else
                {
                    dialog.Filter = "XML saves (*.xml)|*.xml";
                }

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
                        ErrorTextBlock.Text = "Файл повреждён или не соответствует";
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
            string format = "JSON";

            if (FormatComboBox.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)FormatComboBox.SelectedItem;
                format = selectedItem.Content.ToString();
            }

            string extension;

            if (format == "JSON")
            {
                extension = "*.json";
            }
            else
            {
                extension = "*.xml";
            }

            bool hasSaves = Directory.GetFiles(SaveFolderTextBox.Text, extension).Length > 0;

            ContinueButton.IsEnabled = hasSaves;
        }

        private void ConvertSaveToNewFormat(string newFormat)
        {
            if (string.IsNullOrWhiteSpace(_saveFilePath))
            {
                return;
            }

            if (!File.Exists(_saveFilePath))
            {
                return;
            }

            GameSerializerBase oldSerializer;

            if (_saveFilePath.EndsWith(".json"))
            {
                oldSerializer = new JsonGameSerializer();
            }
            else
            {
                oldSerializer = new XmlGameSerializer();
            }

            GameState game = oldSerializer.Load(_saveFilePath);

            GameSerializerBase newSerializer;

            string newExtension;

            if (newFormat == "JSON")
            {
                newSerializer = new JsonGameSerializer();
                newExtension = ".json";
            }
            else
            {
                newSerializer = new XmlGameSerializer();
                newExtension = ".xml";
            }

            string newFilePath =Path.ChangeExtension(_saveFilePath,newExtension);

            newSerializer.Save(game,newFilePath);

            File.Delete(_saveFilePath);

            _saveFilePath = newFilePath;
        }
        private void FormatComboBox_SelectionChanged(object sender,SelectionChangedEventArgs e)
        {
            try
            {
                string selectedFormat = "JSON";

                if (FormatComboBox.SelectedItem != null)
                {
                    ComboBoxItem selectedItem = (ComboBoxItem)FormatComboBox.SelectedItem;
                    selectedFormat = selectedItem.Content.ToString();
                }

                ConvertSaveToNewFormat(selectedFormat);
            }
            catch
            {
            }
        }

        //private void FormatComboBox_SelectionChanged(object sender,SelectionChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (_currentGame == null)
        //        {
        //            return;
        //        }

        //        if (string.IsNullOrWhiteSpace(_saveFilePath))
        //        {
        //            return;
        //        }

        //        if (!File.Exists(_saveFilePath))
        //        {
        //            return;
        //        }

        //        string selectedFormat =
        //            (FormatComboBox.SelectedItem as ComboBoxItem)?
        //            .Content.ToString() ?? "JSON";

        //        string newExtension =
        //            selectedFormat == "JSON"
        //            ? ".json"
        //            : ".xml";

        //        // уже нужный формат
        //        if (Path.GetExtension(_saveFilePath)
        //            .Equals(newExtension,
        //                    StringComparison.OrdinalIgnoreCase))
        //        {
        //            return;
        //        }

        //        // определяем текущий сериализатор
        //        GameSerializerBase oldSerializer;

        //        if (_saveFilePath.EndsWith(".json"))
        //        {
        //            oldSerializer = new JsonGameSerializer();
        //        }
        //        else
        //        {
        //            oldSerializer = new XmlGameSerializer();
        //        }

        //        // загружаем текущее сохранение
        //        GameState game = oldSerializer.Load(_saveFilePath);

        //        // создаём новый сериализатор
        //        GameSerializerBase newSerializer;

        //        if (selectedFormat == "JSON")
        //        {
        //            newSerializer = new JsonGameSerializer();
        //        }
        //        else
        //        {
        //            newSerializer = new XmlGameSerializer();
        //        }

        //        string newFilePath =Path.ChangeExtension(_saveFilePath, newExtension);
        //        newSerializer.Save(game,newFilePath);
        //        _saveFilePath = newFilePath;
        //    }
        //    catch
        //    {
                
        //    }
        //}


        private void OpenGameWindow()
        {
            ComboBoxItem selectedItem = (ComboBoxItem)FormatComboBox.SelectedItem;

            string format = selectedItem.Content.ToString();

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