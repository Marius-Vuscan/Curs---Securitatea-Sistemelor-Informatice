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
        private SlotsCreator s1;
        private int numberOfSlots = 10;
        private byte[] iv = new byte[] { 1, 1, 43, 6, 23, 76, 34, 67, 23, 65, 24, 65, 23, 56, 34, 65 };
        private byte[] key = new byte[] { 1, 1, 43, 6, 23, 76, 34, 67, 23, 65, 24, 65, 23, 56, 34, 65, 1, 1, 43, 6, 23, 76, 34, 67, 23, 65, 24, 65, 23, 56, 34, 65 };

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
            string password = "";
            if (!symbols && !numbers && !lowercase && !uppercase)
            {
                StatusTextBlock.Text = "Error! You must select at least one option!";
            }
            else
            {
                Random rnd = new Random();
                for (int i = 0; i < length; i++)
                {
                    char c = (char)rnd.Next(33, 127);//generate random ascii character

                    while (!symbols && (Char.IsSymbol(c) || Char.IsPunctuation(c)) || !numbers && Char.IsNumber(c) || !lowercase && Char.IsLower(c) ||
                        !uppercase && Char.IsUpper(c) || similar && "il1Lo0O".Contains(c) || ambiguous && "{}[]()/\\'\"`~,;:.<>".Contains(c))
                    {
                        c = (char)rnd.Next(33, 127);
                    }
                    password += c;
                }
                StatusTextBlock.Text = "Success! Password generated!";
            }
            return password;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.WriteAllText(@"../../Data.txt", string.Empty);//delete file data
                TextWriter tw = new StreamWriter(@"../../Data.txt", true);

                for (int i = 0; i < SlotsCreator.dataArray.GetLength(0); i++)
                {
                    for (int j = 0; j < SlotsCreator.dataArray.GetLength(1); j++)
                    {
                        string original = SlotsCreator.dataArray[i, j].Text;//ResultTextBlock.Text;

                        string str = "";

                        if (original != "")//if the textbox is empty then don't encrypt
                        {
                            byte[] encrypted = AesEncrypt.EncryptStringToBytes_Aes(original, key, iv);

                            for (int x = 0; x < encrypted.Length; x++)
                            {
                                str += (char)encrypted[x];
                            }
                        }
                        tw.Write(str + "12345");//the data will be split be this string
                    }
                }
                tw.Close();

                StatusTextBlock.Text = "Success! Data saved!";
            }
            catch (Exception)
            {
                StatusTextBlock.Text = "Error! File exception!";
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> dataList = new List<string>();
                string aux = "";
                aux = File.ReadAllText(@"../../Data.txt");

                if (aux == "")
                {
                    StatusTextBlock.Text = "Error! No data in file!";
                }
                else
                {
                    string[] encryptedData = aux.Split(new string[] { "12345" }, StringSplitOptions.None);

                    for (int i = 0; i < numberOfSlots * 3; i++)
                    {
                        string roundtrip = "";
                        if (encryptedData[i] != "")//if the data[i] is empty then don't decrypt
                        {
                            byte[] encryptedDataByteArray = new byte[encryptedData[i].Length];
                            for (int y = 0; y < encryptedData[i].Length; y++)
                            {
                                encryptedDataByteArray[y] = (byte)encryptedData[i][y];
                            }
                            roundtrip = AesEncrypt.DecryptStringFromBytes_Aes(encryptedDataByteArray, key, iv);
                        }
                        dataList.Add(roundtrip);
                    }

                    int index = 0;
                    for (int i = 0; i < numberOfSlots; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            SlotsCreator.dataArray[i, j].Text = dataList[index];
                            index++;
                        }
                    }

                    StatusTextBlock.Text = "Success! Data Loaded!";
                }
            }
            catch (IOException)
            {
                StatusTextBlock.Text = "Error! File exception!";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s1 = new SlotsCreator(numberOfSlots, DataStackPanel);
        }
    }
}