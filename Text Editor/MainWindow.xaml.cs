using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

// ReSharper disable once CheckNamespace
namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentFileName, currentFilePath;
        private bool hasChanged = false;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void QuitMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void OpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            var results = OpenFileDialog();
            if (!results.Item1)
            {
                MessageBox.Show("Couldn't Open File");
                return;
            }

            currentFilePath = results.Item2;
            var fileContents = File.ReadAllText(results.Item2, Encoding.UTF8);
            TextBox.Text = fileContents;

            currentFileName = Title = results.Item3;
            hasChanged = false;
        }

        private void SaveFile_OnClick(object sender, RoutedEventArgs e)
        {
            var results = OpenSaveFileDialog();
            if (results.Item1 == false)
                return;

            currentFilePath = results.Item2;
            File.WriteAllText(currentFilePath, TextBox.Text);
            currentFileName = Title = results.Item3;
            hasChanged = false;
        }

        private (bool, string, string) OpenSaveFileDialog()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = currentFileName,
                Filter = "Text Files (.txt)|*.txt"
            };

            var result = dialog.ShowDialog();
            return (result == true, dialog.FileName, dialog.SafeFileName);
        }

        private static (bool, string, string) OpenFileDialog()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                FileName = "Document",
                DefaultExt = ".txt",
                Filter = "Text Files (.txt)|*.txt"
            };

            var result = dialog.ShowDialog();

            return (result == true, dialog.FileName, dialog.SafeFileName);
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!hasChanged)
                Title += "*";
            hasChanged = true;
        }
    }
}