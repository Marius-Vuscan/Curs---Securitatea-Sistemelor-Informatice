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
using System.IO;
using System.Security.Cryptography;

namespace PasswordGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SlotsCreator s1;
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

            ResultTextBlock.Text = generatePassword(length, symbols, numbers, lowercase, uppercase, similar, ambiguous);
            s1.updatePassWordSet(ResultTextBlock.Text);
        }

        private string generatePassword(int length, bool symbols, bool numbers, bool lowercase, bool uppercase, bool similar, bool ambiguous)
        {
            Random rnd = new Random();
            string password = "";
            for (int i = 0; i < length; i++)
            {
                char c = (char)rnd.Next(33, 127);//generate random ascii symbol

                while (!symbols && Char.IsSymbol(c) || !numbers && Char.IsNumber(c) || !lowercase && Char.IsLower(c) ||
                    !uppercase && Char.IsUpper(c) || similar && "il1Lo0O".Contains(c) || ambiguous && "{}[]()/\\'\"`~,;:.<>".Contains(c))
                {
                    c = (char)rnd.Next(33, 127);
                }

                password += c;
            }
            return password;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ResultTextBlock.Text.Length != int.Parse(PasswordLengthTextBox.Text))
                {
                    MessageBox.Show("Error! Please generate a password first!");
                }
                else
                {
                    TextWriter tw = new StreamWriter(@"../../TextFile1.txt", true);

                    string original = ResultTextBlock.Text;

                    byte[] encrypted = AesEncrypt.EncryptStringToBytes_Aes(original, key, iv);

                    string str = "";

                    for (int i = 0; i < encrypted.Length; i++)
                    {
                        str += (char)encrypted[i];
                    }
                    MessageBox.Show(str);

                    tw.WriteLine(str);
                    
                    tw.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error! File exception!");
            }
        }

        byte[] iv = new byte[] { 1, 1, 43, 6, 23, 76, 34, 67, 23, 65, 24, 65, 23, 56, 34, 65 };
        byte[] key = new byte[] { 1, 1, 43, 6, 23, 76, 34, 67, 23, 65, 24, 65, 23, 56, 34, 65, 1, 1, 43, 6, 23, 76, 34, 67, 23, 65, 24, 65, 23, 56, 34, 65 };

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> lastPasswords = new List<string>();
            try
            {
                TextReader tr = new StreamReader(@"../../TextFile1.txt", true);
                string aux = "";
                while ((aux = tr.ReadLine()) != null)
                {
                    byte[] encryptedData = new byte[aux.Length];
                    for (int i = 0; i < aux.Length; i++)
                    {
                        encryptedData[i] = (byte)aux[i];
                    }
                    string roundtrip = AesEncrypt.DecryptStringFromBytes_Aes(encryptedData, key, iv);
                    lastPasswords.Add(roundtrip);
                    
                    foreach (var item in lastPasswords)
                    {
                        //LoadAndStatusListBox.Items.Add(item);
                    }
                }
                tr.Close();

            }
            catch (IOException)
            {
                MessageBox.Show("Error! File exception!");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s1 = new SlotsCreator(9, DataStackPanel);
        }
    }
}