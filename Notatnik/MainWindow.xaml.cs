using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Notatnik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private string filepath = null; //path
        private OpenFileDialog openfd; //Object of OpenDialog
        private SaveFileDialog savefd; //Object of SaveDialog
        private bool IsChanged = false; //Is anything changed in textbox

        public MainWindow()
        {
            InitializeComponent();

            //OpenFileDialog class
            openfd = new OpenFileDialog();
            openfd.Title = "Wybierz plik";
            openfd.DefaultExt = "txt";
            openfd.Filter = "*.txt | *.*";

            //SaveFileDialog class
            savefd = new SaveFileDialog();
            savefd.Title = "Zapisz plik tekstowy";
            savefd.DefaultExt = "txt";
            savefd.Filter = "All files (*.*) | Text files (*.txt)";
            savefd.FilterIndex = 1;
        }
        private void MenuItem_Click_Open(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(filepath))
            {
                openfd.InitialDirectory = Path.GetDirectoryName(filepath);
                openfd.FileName = Path.GetFileName(filepath);
            }

            bool? result = openfd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                filepath = openfd.FileName;
                TextBox.Text = File.ReadAllText(filepath);
                StatusBarText.Text = Path.GetFileName(filepath);
                IsChanged = false;
            }
        }

        private void MenuItem_Click_SaveAs(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(filepath))
            {
                savefd.InitialDirectory = Path.GetDirectoryName(filepath);
                savefd.FileName = Path.GetFileName(filepath);
            }

            bool? result = savefd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                filepath = savefd.FileName;
                File.WriteAllText(filepath, TextBox.Text);
                StatusBarText.Text = Path.GetFileName(filepath);
                IsChanged = false;
            }
        }

        private void MenuItem_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            IsChanged = true;
        }

        private void MenuItem_Click_Save(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(filepath))
            {
                File.WriteAllText(filepath, TextBox.Text);
                IsChanged = false;
            }
            else MenuItem_Click_SaveAs(sender, e);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool cancel;
            Window_Closing_Ask(sender, out cancel);
            e.Cancel = cancel;
        }

        private void Window_Closing_Ask(object sender, out bool cancel)
        {
            cancel = false;
            if (IsChanged)
            {
                MessageBoxResult result =
                    MessageBox.Show("Czy zapisac zmiany w dokumencie?",
                    this.Title,
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question,
                    MessageBoxResult.Cancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MenuItem_Click_Save(sender, null);
                        break;
                    case MessageBoxResult.No: break;
                    case MessageBoxResult.Cancel:
                    default:
                        cancel = true;
                        break;
                }
            }
        }

        private void MenuItem_Click_New(object sender, RoutedEventArgs e)
        {
            bool cancel;
            Window_Closing_Ask(sender, out cancel);
            if (!cancel) TextBox.Text = "";
        }

        private void MenuItem_Click_Undo(object sender, RoutedEventArgs e)
        {
            TextBox.Undo();
        }

        private void MenuItem_Click_Redo(object sender, RoutedEventArgs e)
        {
            TextBox.Redo();
        }

        private void MenuItem_Click_Cut(object sender, RoutedEventArgs e)
        {
            TextBox.Cut();
        }

        private void MenuItem_Click_Copy(object sender, RoutedEventArgs e)
        {
            TextBox.Copy();
        }

        private void MenuItem_Click_Paste(object sender, RoutedEventArgs e)
        {
            TextBox.Paste();
        }

        private void MenuItem_Click_Date(object sender, RoutedEventArgs e)
        {
            TextBox.SelectedText = System.DateTime.Now.ToString();
        }

        private void MenuItem_Click_ZawijanieWierszy(object sender, RoutedEventArgs e)
        {
            bool IsChecked = (sender as MenuItem).IsChecked; // - true/false/null Me method: bool IsSelected = ((MenuItem)sender).IsChecked; 
            TextBox.TextWrapping = IsChecked ? TextWrapping.Wrap : TextWrapping.NoWrap;
        }

        private void MenuItem_Click_toolBar(object sender, RoutedEventArgs e)
        {
            bool IsChecked = ((MenuItem)sender).IsChecked;
            toolBar.Visibility = IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MenuItem_Click_statusBar(object sender, RoutedEventArgs e)
        {
            bool IsChecked = ((MenuItem)sender).IsChecked;
            statusBar.Visibility = IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }

        private void MenuItem_Click_BackgroundColor(object sender, RoutedEventArgs e)
        {
            Color background = Colors.White;
            if (TextBox.Background is SolidColorBrush)
                background = (TextBox.Background as SolidColorBrush).Color;
            if (NoteUtils.WindowsFormsHelper.ChooseColor(ref background))
                TextBox.Background = new SolidColorBrush(background);
        }
    }
}
