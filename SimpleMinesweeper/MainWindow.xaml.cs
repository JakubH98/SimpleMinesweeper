using SimpleMinesweeper.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SimpleMinesweeper
{
    public partial class MainWindow : Window
    {
        private Game game;
        private Button[,] buttons;
        // Define grid size and number of mines
        int rows = 10;
        int columns = 10;
        int totalMines = 10;

        public MainWindow()
        {
            InitializeComponent();
            
            
            buttons = new Button[rows, columns];

            
            game = new Game(rows, columns, totalMines, buttons);
            
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            MineGrid.Children.Clear(); // Clear any existing buttons
            MineGrid.Rows = game.Rows; // Set the grid size
            MineGrid.Columns = game.Columns;

            for (int row = 0; row < game.Rows; row++)
            {
                for (int col = 0; col < game.Columns; col++)
                {
                    Button button = new Button();
                    button.Click += Button_Click;
                    buttons[row, col] = button;
                    MineGrid.Children.Add(button);
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            int row = game.GetRow(clickedButton);
            int col = game.GetCol(clickedButton);

            if (game.Grid[row, col] == -1)
            {
                clickedButton.Background = Brushes.Red;
                MessageBox.Show("Game Over!");
                ResetGame();
            }
            else
            {
                game.RevealCell(row, col);
                if (game.CheckWin())
                {
                    MessageBox.Show("You Win!");
                    ResetGame();
                }
            }
        }

        private void ResetGame()
        {           

            buttons = new Button[rows, columns];
            game = new Game(rows, columns, totalMines, buttons);
            InitializeGrid();
        }
    }
}