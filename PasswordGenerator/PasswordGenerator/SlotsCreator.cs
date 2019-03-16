using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PasswordGenerator
{
    class SlotsCreator
    {
        public static TextBox[,] dataArray;
        private int numberOfElements;
        private string passWordSet="";
        private int index = 0;

        public SlotsCreator(int numberOfElements, StackPanel parentElement)
        {
            this.numberOfElements = numberOfElements;
            dataArray = new TextBox[numberOfElements, 3];

            for (int i = 0; i < numberOfElements; i++)
            {
                generateSlot(parentElement);
            }
        }

        public void generateSlot(StackPanel parentElement)
        {
            //grid
            Grid newGrid = new Grid();

            //column Definition
            for (int i = 0; i < 5; i++)
            {
                ColumnDefinition newCD = new ColumnDefinition();
                newCD.Width = new GridLength(1, GridUnitType.Star);
                newGrid.ColumnDefinitions.Add(newCD);
            }

            //add elements
            TextBox titleTextBox = new TextBox();
            TextBox userTextBox = new TextBox();
            TextBox passTextBox = new TextBox();
            Button useGeneratedButton = new Button();
            useGeneratedButton.Content = "*";
            useGeneratedButton.Click += delegate
            {
                passTextBox.Text = passWordSet;
            };
            Button copyPassButton = new Button();
            copyPassButton.Content = "*";
            copyPassButton.Click += delegate
            {
                Clipboard.SetText(passTextBox.Text);
            };

            //add data in array
            dataArray[index, 0] = titleTextBox;
            dataArray[index, 1] = userTextBox;
            dataArray[index, 2] = passTextBox;

            //set column for elements
            Grid.SetColumn(titleTextBox, 0);
            Grid.SetColumn(userTextBox, 1);
            Grid.SetColumn(passTextBox, 2);
            Grid.SetColumn(useGeneratedButton, 3);
            Grid.SetColumn(copyPassButton, 4);

            //append elements
            newGrid.Children.Add(titleTextBox);
            newGrid.Children.Add(userTextBox);
            newGrid.Children.Add(passTextBox);
            newGrid.Children.Add(useGeneratedButton);
            newGrid.Children.Add(copyPassButton);

            //add grid to main stackpanel
            parentElement.Children.Add(newGrid);

            index++;
        }

        public void updatePassWordSet(string newString)
        {
            passWordSet = newString;
        }
    }
}
