using System;
using System.Windows.Controls;

namespace SimpleMinesweeper.Model
{
    public class Game
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int TotalMines { get; set; }

        private Button[,] buttons;
        public int[,] Grid;

        public Game(int rows, int columns, int totalMines, Button[,] buttons)
        {
            this.Rows = rows;
            this.Columns = columns;
            this.TotalMines = totalMines;
            this.buttons = buttons;

            Grid = new int[Rows, Columns];
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            Random random = new Random();
            int minesPlaced = 0;

            // Place mines randomly
            while (minesPlaced < TotalMines)
            {
                int row = random.Next(Rows);
                int col = random.Next(Columns);

                if (Grid[row, col] != -1)
                {
                    Grid[row, col] = -1; 
                    minesPlaced++;
                }
            }

            // Calculate adjacent mine counts
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    if (Grid[row, col] != -1)
                    {
                        Grid[row, col] = CountAdjacentMines(row, col);
                    }
                }
            }
        }
        private int CountAdjacentMines(int row, int col)
        {
            int count = 0;
            for (int r = row - 1; r <= row + 1; r++)
            {
                for (int c = col - 1; c <= col + 1; c++)
                {
                    if (r >= 0 && r < Rows && c >= 0 && c < Columns && Grid[r, c] == -1)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        public void RevealCell(int row, int col)
        {
            Button button = buttons[row, col];
            if (button.IsEnabled)
            {
                button.Content = Grid[row, col] > 0 ? Grid[row, col].ToString() : "";
                button.IsEnabled = false;

                if (Grid[row, col] == 0)
                {
                    // Automatically reveal adjacent cells if empty
                    for (int r = row - 1; r <= row + 1; r++)
                    {
                        for (int c = col - 1; c <= col + 1; c++)
                        {
                            if (r >= 0 && r < Rows && c >= 0 && c < Columns)
                            {
                                RevealCell(r, c);
                            }
                        }
                    }
                }
            }
        }
        public bool CheckWin()
        {
            foreach (Button button in buttons)
            {
                if (button.IsEnabled && Grid[GetRow(button), GetCol(button)] != -1)
                {
                    return false; // There are still non-mine cells that need to be revealed
                }
            }
            return true; // All non-mine cells have been revealed
        }

        public int GetRow(Button button)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    if (buttons[row, col] == button)
                    {
                        return row;
                    }
                }
            }
            return -1;
        }
        public int GetCol(Button button)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    if (buttons[row, col] == button)
                    {
                        return col;
                    }
                }
            }
            return -1;
        }
    }
}
