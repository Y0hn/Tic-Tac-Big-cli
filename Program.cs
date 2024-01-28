using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tic_Tac_Big
{
    class Program
{
    void Main()
    {
        // inicialization
    }
}
    class BattleField
    {
        static int numberOfBatles = 0;
        Cellar[] grid;
        protected static readonly Dictionary<string, string> players = new Dictionary<string, string>()
        {
            { "1", "X" },
            { "2", "O" },
            { "0X"," \\   / "},
            { "2X","   x   "},
            { "3X"," /   \\ "},
            { "0O","  ---  "},
            { "1O"," (   ) "},
            { "2O","  ---  "},

        };

        public BattleField()
        {
            numberOfBatles++;
            grid = new Cellar[9];
            for (int i = 0; i < 9; i++)
            {
                grid[i] = new Cellar();
            }
        }

        private class Cellar
        {
            Cell[] grid;
            public bool finished { get; set; }
            bool active;
            char winner;

            public Cellar()
            {
                grid = new Cell[9];
                for (int i = 0; i < 9; i++)
                {
                    grid[i] = new Cell();
                }
                finished = false;
                active = false;
                winner = ' ';
            }
            public bool WinCheck()
            {
                bool ver = false;
                bool hor = false;
                bool dia = false;
                char ch;

                // Vertival
                for (int i = 0; i < 3; i++)
                {
                    ch = grid[i].GetValue();
                    if (ch != ' ' && !ver)
                        ver = (grid[i + 3].GetValue() == ch && ch == grid[i + 3].GetValue());
                }

                // Horizontal
                for (int i = 0; i < 7; i += 3)
                {
                    ch = grid[i].GetValue();
                    if (ch != ' ' && !hor)
                        hor = (grid[i + 1].GetValue() == ch && ch == grid[i + 2].GetValue());
                }

                // Diagonal
                ch = grid[4].GetValue();
                dia = ch != ' ' && grid[0].GetValue() == ch && ch == grid[8].GetValue() || dia;
                dia = ch != ' ' && grid[2].GetValue() == ch && ch == grid[6].GetValue() || dia;

                return ver || hor || dia;
            }
            public string ReadRow(int row)
            {
                if (!finished)
                {
                    int i = row * 3;
                    return $" {grid[i].GetValue()} | {grid[i + 1].GetValue()} | {grid[i + 2].GetValue()} ";
                }
                else
                {
                    return players[winner + row.ToString()];
                }
            }
            private class Cell
            {
                public bool ocupied { get; set; }
                char player;

                public Cell()
                {
                    ocupied = false;
                    player = ' ';
                }
                public char GetValue()
                {
                    return player;
                }
                public bool SetValue(char ch)
                {
                    if (!ocupied)
                    {
                        ocupied = true;
                        player = ch;
                        return true;
                    }
                    return false;
                }
            }
        }
    }
}