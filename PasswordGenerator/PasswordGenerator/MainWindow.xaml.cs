using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            int length = int.Parse(PasswordLengthTextBox.Text);
            bool symbols = (bool)SymbolsCheckBox.IsChecked;
            bool numbers = (bool)NumbersCheckBox.IsChecked;
            bool lowercase = (bool)LowercaseCheckBox.IsChecked;
            bool uppercase = (bool)UppercaseCheckBox.IsChecked;
            bool similar = (bool)SimilarCheckBox.IsChecked;
            bool ambiguous = (bool)AmbiguousCheckBox.IsChecked;

            ResultTextBlock.Text = generatePassword(length,symbols,numbers,lowercase,uppercase,similar,ambiguous);
        }

        private string generatePassword(int length, bool symbols, bool numbers, bool lowercase, bool uppercase, bool similar, bool ambiguous)
        {
            Random rnd = new Random();
            string password = "";
            for (int i = 0; i < length; i++)
            {
                char c = (char)rnd.Next(33, 127);//generate random ascii symbol

                while(!symbols && Char.IsSymbol(c) || !numbers && Char.IsNumber(c) || !lowercase && Char.IsLower(c) ||
                    !uppercase && Char.IsUpper(c) || similar && "il1Lo0O".Contains(c) || ambiguous && "{}[]()/\\'\"`~,;:.<>".Contains(c))
                {
                    c = (char)rnd.Next(33, 127);
                }

                password += c;
            }
            return password;
        }
    }
}
