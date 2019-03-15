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
                    tw.Flush();

                    for (int i = 0; i < SlotsCreator.dataArray.GetLength(0); i++)
                    {
                        for (int j = 0; j < SlotsCreator.dataArray.GetLength(1); j++)
                        {
                            string original = SlotsCreator.dataArray[i, j].Text;//ResultTextBlock.Text;

                            byte[] encrypted = AesEncrypt.EncryptStringToBytes_Aes(original, key, iv);

                            string str = "";

                            for (int x = 0; x < encrypted.Length; x++)
                            {
                                str += (char)encrypted[x];
                            }
                            //tw.Write(str+"12345", 0, str.Length);
                            tw.Write(str + "12345");
                        }
                    }
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
                aux = File.ReadAllText(@"../../TextFile1.txt");
                string[] encryptedData = aux.Split(new string[] { "12345" }, StringSplitOptions.None);

                for (int i = 0; i < 27; i++)
                {
                    byte[] encryptedDataByteArray = new byte[encryptedData[i].Length];
                    for (int y = 0; y < encryptedData[i].Length; y++)
                    {
                        encryptedDataByteArray[y] = (byte)encryptedData[i][y];
                    }
                    string roundtrip = AesEncrypt.DecryptStringFromBytes_Aes(encryptedDataByteArray, key, iv);
                    lastPasswords.Add(roundtrip);
                }

                tr.Close();

                int index = 0;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        SlotsCreator.dataArray[i, j].Text = lastPasswords[index];
                        index++;
                    }
                }
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