using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tic_Tac_Big
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BattleField pole = new BattleField();
            int input;
            while (true)
            {
                //Console.Clear();
                pole.WriteOut();

                do
                {
                    Console.Write("\n\n\nZadaj id active pola: ");
                }
                while (!int.TryParse(Console.ReadLine(), out input));
                pole.SetCellarActive(input);
            }
        }
    }
    class BattleField
    {
        /**           ***{ GRID DESIGN }***           **      
            
                |   |    |    |   |    |    |   |   
             ---+---+--- | ---+---+--- | ---+---+---
                |   |    |    |   |    |    |   |   
             ---+---+--- | ---+---+--- | ---+---+---
                |   |    |    |   |    |    |   |   
            -------------+-------------+-------------
                |   |    |    |   |    |    |   |   
             ---+---+--- | ---+---+--- | ---+---+---
                |   |    |    |   |    |    |   |   
             ---+---+--- | ---+---+--- | ---+---+---
                |   |    |    |   |    |    |   |    
            -------------+-------------+-------------
                |   |    |    |   |    |    |   |   
             ---+---+--- | ---+---+--- | ---+---+---
                |   |    |    |   |    |    |   |   
             ---+---+--- | ---+---+--- | ---+---+---
                |   |    |    |   |    |    |   |   
         */
        static int numberOfBatles = 0;
        Cellar[] grid;
        public static readonly Dictionary<int, string> players = new Dictionary<int, string>()
        {
            { 1, "X" },
            { 2, "O" },

            { 10, "__   __   " },
            { 11, "\\ \\ / /   " },
            { 12, " \\ V /    " },
            { 13, " / . \\    " },
            { 14, "/_/ \\_\\   " },

            { 20, "   ___    " },
            { 21, " .'   `.  " },
            { 22, "/  .-.  \\ " },
            { 23, "\\  `-'  / " },
            { 24, " `.___.'  " },

            { 50, "   |   |   " },
            { 51, "---+---+---" },
            { 52, "   |   |   " },
            { 53, "---+---+---" },
            { 54, "   |   |   " }
        };
        private static readonly Dictionary<string, string> frame = new Dictionary<string, string>()
        {
            { "MainFrameHor", "-------------+-------------+-------------" },
            { "MainFrameVer", " |" },
            { "MainFrameVer1", " |" },
            { "InnerFrameHor", " ---+---+---" },
            { "InnerFrameVer", "|" },
        };
        private static readonly Dictionary<string, ConsoleColor> colorPalet = new Dictionary<string, ConsoleColor>()
        {
            { "MainFrame", ConsoleColor.Yellow },
            { "InnerFrame", ConsoleColor.White },
            { "Previosly", ConsoleColor.Gray },
            { "ActiveFrame", ConsoleColor.Blue },
            { "X", ConsoleColor.Red },
            { "Player1", ConsoleColor.Red },
            { "O", ConsoleColor.Blue },
            { "Player2", ConsoleColor.Blue },
            { "Neutral", ConsoleColor.White },
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

        public bool Turn(int i)
        {
            return grid[i].WriteInCell(0, 1);
        }
        public void SetCellarActive(int id)
        {
            if (id < grid.Length)
                grid[id].active = true;
        }
        public void WriteOut(bool free = false)
        {
            // Pocet Cellarov 9x3 na vysku MainFrame
            for (int i = 0; i < 3; i++)
            {
                // Pocet Cellarov 9x1 na vysku LongInnerFrame
                for (int j = 0; j < 3; j++)
                {
                    // Pocet Cell 3x1 na sirku Cellarov
                    for (int k = 0; k < 3; k++)
                    {
                        // Pocet Cell 1x1 na sirku LongCell
                        for (int l = 0; l < 3; l++)
                        {
                            // first element in row
                            if (l == 0)
                                Console.Write(" ");

                            string s = grid[i * 3 + j].GetCellValue(k * 3 + l).ToString();

                            if      (s == players[1])
                                Console.ForegroundColor = colorPalet["Player1"];
                            else if (s == players[2])
                                Console.ForegroundColor = colorPalet["Player2"];
                            else if (grid[i * 3 + j].active)
                            {
                                Console.ForegroundColor = colorPalet["Neutral"];
                                s = (j * 3 + l + 1).ToString();
                                //s = n[k].ToString();
                            }
                            else
                                s = " ";

                            Console.Write(" " + s + " ");
                            if (l != 2)
                            {
                                if (grid[i * 3 + j].active)
                                    Console.ForegroundColor = colorPalet["ActiveFrame"];
                                else
                                    Console.ForegroundColor = colorPalet["InnerFrame"];
                                Console.Write(frame["InnerFrameVer"]);
                            }
                        }
                        if (k != 2)
                        {
                            Console.ForegroundColor = colorPalet["MainFrame"];
                            Console.Write(frame["MainFrameVer"]);
                        }
                    }
                    // Odelenie Riadkov v ramci Inner Framu
                    if (j != 2)  
                    {
                        Console.WriteLine("");
                        if (!grid[i * 3+j].finished)
                        {
                            if (grid[i * 3 + j].active)
                                Console.ForegroundColor = colorPalet["ActiveFrame"];
                            else
                                Console.ForegroundColor = colorPalet["InnerFrame"];
                            Console.Write(frame["InnerFrameHor"]);
                        }
                        else
                        {
                            string w = grid[i * 3+j].winner.ToString();
                            int ww = 0;
                            Console.ForegroundColor = colorPalet[w];
                            if (w.Equals(players[1]))
                                ww = 10;
                            else
                                ww = 20;
                            Console.Write(players[ww+j*2]);
                        }
                        Console.ForegroundColor = colorPalet["MainFrame"];
                        Console.Write(frame["MainFrameVer1"]);
                        if (!grid[i * 3+1].finished)
                        {
                            if (grid[i * 3 + j].active)
                                Console.ForegroundColor = colorPalet["ActiveFrame"];
                            else
                                Console.ForegroundColor = colorPalet["InnerFrame"];
                            Console.Write(frame["InnerFrameHor"]);
                        }

                        else
                        {
                            string w = grid[i * 3+1].winner.ToString();
                            int ww = 0;
                            Console.ForegroundColor = colorPalet[w];
                            if (w.Equals(players[1]))
                                ww = 10;
                            else
                                ww = 20;
                            Console.Write(players[ww + j * 2]);
                        }
                        Console.ForegroundColor = colorPalet["MainFrame"];
                        Console.Write(frame["MainFrameVer1"]);
                        if (!grid[i * 3+2].finished)
                        {
                            Console.ForegroundColor = colorPalet["InnerFrame"];
                            Console.Write(frame["InnerFrameHor"]);
                        }

                        else
                        {
                            string w = grid[i * 3+2].winner.ToString();
                            int ww = 0;
                            Console.ForegroundColor = colorPalet[w];
                            if (w.Equals(players[1]))
                                ww = 10;
                            else
                                ww = 20;
                            Console.Write(players[ww + j * 2]);
                        }
                    }
                    // New Line
                    Console.WriteLine("");
                }
                // MainFrame Line
                if (i != 2)
                {
                    Console.ForegroundColor = colorPalet["MainFrame"];
                    Console.WriteLine(frame["MainFrameHor"]);
                }
            }
        }
        private class Cellar
        {
            Cell[] grid;
            public bool finished { get; set; }
            public char winner;
            public bool active;

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
            public char GetCellValue(int id)
            {
                return grid[id].GetValue();
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
            public bool WriteInCell(int cellID, byte player)
            {
                if (grid[cellID].ocupied)
                    return false;

                grid[cellID].SetValue(players[player].ToCharArray()[0]);
                return true;
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
                    return players[1];
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
