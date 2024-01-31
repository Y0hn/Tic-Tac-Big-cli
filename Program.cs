﻿using System.Collections.Generic;
using System;

namespace Tic_Tac_Big
{
    internal class Program
    {
        static void Main()
        {
            BattleField pole = new BattleField();
            Console.ForegroundColor = ConsoleColor.White;
            int input, playerTurn = 1;

            while (!pole.WinnerCheck())
            {
                Console.Clear();
                pole.WriteOut();
                Console.WriteLine("\n");
                if (!pole.IsCellarActive())
                {
                    GetInput($"Choose to set active for player{playerTurn}: ", out input, 1, 10);
                    pole.MakeCellarActive(input-1);
                }
                else
                {
                    GetInput($"Place player {playerTurn} into acitve: ", out input, 1, 10);
                    pole.PlaceIntoActive(input-1, playerTurn);
                }

                // Change Turns
                if      (playerTurn == 1)
                    playerTurn = 2;
                else // if (playerTurn == 2)
                    playerTurn = 1;
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\n\t\tTHE WINNER IS YOU !!");
            Console.ReadKey();
        }
        static void GetInput(string mess, out int input, int min = -1, int max = -1)
        {
            /// Does not include max numbers 
            do
            {
                do
                {
                    Console.Write(mess);
                }
                while (!int.TryParse(Console.ReadLine(), out input));
            }
            while (!((min == -1 || min <= input) && (max == -1 || input < max)));
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
        private static readonly Dictionary<int, string> players = new Dictionary<int, string>()
        {
            { 1, "X" },
            { 2, "O" },

            { 10, " __   __  " },
            { 11, "   \\ \\ / /  " },
            { 12, "  \\ V /   " },
            { 13, "    / . \\   " },
            { 14, " /_/ \\_\\  " },
                     
            { 20, "   ___    " },
            { 21, "   .'   `.  " },
            { 22, "/  .-.  \\ " },
            { 23, "  \\  `-'  / " },
            { 24, " `.___.'  " },

            { 50, "   |   |   " },
            { 51, "---+---+---" },
            { 52, "   |   |   " },
            { 53, "---+---+---" },
            { 54, "   |   |   " }
        };
        private static readonly Dictionary<string, string> frame = new Dictionary<string, string>()
        {
            { "MainFrameHor",   "-------------+-------------+-------------" },
            { "MainFrameVer",   " |" },
            { "InnerEmpty1",     "            " },
            { "InnerEmpty2",     "          " },
            { "InnerHalfEmpty1","    " },
            { "InnerHalfEmpty2",       "     " },
            { "InnerFrameHor",  " ---+---+---" },
            { "InnerFrameVer",  " | " },
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
        public void WriteOut()
        {
            ConsoleColor prevColor = Console.ForegroundColor;
            bool active = true;
            if (GetActiveCellar() < 0)
                active = false;

            // Pocet Cellarov v MainFrame na vysku
            for (int vMain = 0; vMain < 3; vMain++)
            {
                /* Vypisovanie 
                 * 
                 *         |   |    |    |   |    |    |   |   
                 *      ---+---+--- | ---+---+--- | ---+---+---
                 *         |   |    |    |   |    |    |   |   
                 *      ---+---+--- | ---+---+--- | ---+---+---
                 *         |   |    |    |   |    |    |   |   
                 *         
                 */
                for (int row = 0; row < 3; row++)
                {
                    /*  Row
                     * 
                     *   _ | _ | _  |  _ | _ | _  |  _ | _ | _
                     * 
                     */
                    for (int sMain = 0; sMain < 3; sMain++)
                    {
                        /* Inner Row
                         * 
                         *   _ | _ | _ 
                         * 
                         */
                        for (int col = 0;  col < 3; col++)
                        {
                            // first element in row
                            if (col == 0)
                                Console.Write("  ");

                            if (grid[vMain * 3 + sMain].finished)
                            {
                                string s = grid[vMain * 3 + sMain].winner.ToString();
                                if (s == players[1])
                                {
                                    Console.ForegroundColor = colorPalet["Player1"];
                                    Console.Write(players[10 + row*2]);
                                }
                                else if (s == players[2])
                                {
                                    Console.ForegroundColor = colorPalet["Player2"];
                                    Console.Write(players[20 + row * 2]);
                                }
                                break;
                            }
                            else if (active)
                            {

                                string s = grid[vMain * 3 + sMain].GetCellValue(row * 3 + col).ToString();
                                if (s == players[1])
                                    Console.ForegroundColor = colorPalet["Player1"];
                                else if (s == players[2])
                                    Console.ForegroundColor = colorPalet["Player2"];
                                else if (grid[vMain * 3 + sMain].active)
                                {
                                    Console.ForegroundColor = colorPalet["Neutral"];
                                    s = (row * 3 + col + 1).ToString();
                                }
                                Console.Write(s);

                                /* Inner Row Separator
                                 * 
                                 *  | 
                                 *  
                                 */
                                if (col < 2)
                                {
                                    // Change if active !
                                    if (grid[vMain * 3 + sMain].active)
                                        Console.ForegroundColor = colorPalet["ActiveFrame"];
                                    else
                                        Console.ForegroundColor = colorPalet["InnerFrame"];
                                    Console.Write(frame["InnerFrameVer"]);
                                }
                                else
                                    Console.Write(" ");
                            }
                            else
                            {
                                Console.ForegroundColor = colorPalet["Neutral"];
                                string s = frame["InnerEmpty2"];
                                if (row == 1)
                                    s = frame["InnerHalfEmpty1"] + (vMain*3 + sMain+1).ToString() + frame["InnerHalfEmpty2"];
                                Console.Write(s);
                                break;
                            }
                        }
                        /* Main Collum Frame separator
                         * 
                         *              | 
                         */
                        if (sMain < 2)
                        {
                            Console.ForegroundColor = colorPalet["MainFrame"];
                            Console.Write(frame["MainFrameVer"]);
                        }
                    }
                    Console.WriteLine("");

                    /* Row Separator
                     * 
                     *       ---+---+--- | ---+---+--- | ---+---+---
                     */
                    if (row < 2)
                    {
                        for (int sMain = 0; sMain < 3; sMain++)
                        {
                            if (grid[vMain * 3 + sMain].finished)
                            {
                                string s = grid[vMain * 3 + sMain].winner.ToString();
                                if (s == players[1])
                                {
                                    Console.ForegroundColor = colorPalet["Player1"];
                                    Console.Write(players[10 + row*2+1]);
                                }
                                else if (s == players[2])
                                {
                                    Console.ForegroundColor = colorPalet["Player2"];
                                    Console.Write(players[20 + row * 2 + 1]);
                                }
                            }
                            else if (active)
                            {
                                // Change if active !
                                if (grid[vMain * 3 + sMain].active)
                                    Console.ForegroundColor = colorPalet["ActiveFrame"];
                                else
                                    Console.ForegroundColor = colorPalet["InnerFrame"];
                                Console.Write(frame["InnerFrameHor"]);
                            }
                            else
                            {
                                Console.ForegroundColor = colorPalet["Neutral"];
                                Console.Write(frame["InnerEmpty1"]);
                            }

                            if (sMain < 2)
                            {
                                Console.ForegroundColor = colorPalet["MainFrame"];
                                Console.Write(frame["MainFrameVer"]);
                            }
                        }
                        Console.WriteLine("");
                    }
                }

                // Horizontalny separator MainFramu
                if (vMain < 2)
                {
                    Console.ForegroundColor = colorPalet["MainFrame"];
                    Console.WriteLine(frame["MainFrameHor"]);
                }
            }
            Console.ForegroundColor = prevColor;
        }
        public bool WinnerCheck()
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
                    ver = (grid[i + 3].GetValue() == ch && ch == grid[i + 6].GetValue());
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

            /* DEBUG
            for (int i = 0; i < 9; i++)
                Console.WriteLine($"grid[{i}] = {grid[i].GetValue()}!");
            */
            return ver || hor || dia;
        }
        public bool PlaceIntoActive(int id, int player)
        {
            int act = GetActiveCellar();
            if (grid[act].WriteInCell(id, player))
            {
                ChangeActiveCellar(id);
                if (grid[act].WinCheck())
                {
                    grid[act].finished = true;
                    grid[act].winner = players[player].ToCharArray()[0];
                }
                return true;
            }
            return false;
        }
        public bool IsCellarActive()
        {
            return GetActiveCellar() != -1;
        }
        public bool MakeCellarActive(int id)
        {
            if (grid[id].finished)
                return false;

            grid[id].active = true;
            return true;
        }
        private void ChangeActiveCellar(int id)
        {
            int i = GetActiveCellar();
            if (i >= 0)
                grid[i].active = false;

            if (!grid[id].finished)
                grid[id].active = true;
        }
        private int GetActiveCellar()
        {
            for (int i = 0;i < grid.Length;i++)
                if (grid[i].active)
                    return i;
            return -1;
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
            public char GetValue()
            {
                return winner;
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
                        ver = (grid[i + 3].GetValue() == ch && ch == grid[i + 6].GetValue());
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
            public bool WriteInCell(int cellID, int player)
            {
                if (grid[cellID].ocupied)
                    return false;

                grid[cellID].SetValue(players[player].ToCharArray()[0]);
                return true;
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