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
                Console.Clear();
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
            { "MainFrameHor",   "-------------+-------------+-------------" },
            { "MainFrameVer",   " |" },
            { "InnerEmpty",     "            " },
            { "InnerHalfEmpty1","      " },
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
        public void WriteOut(bool free = false)
        {
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
                        /* Inner Frame separator
                         *          | 
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
                        for (int i = 0; i < 3; i++)
                        {
                            // Change if active !
                            if (grid[vMain * 3 + i].active)
                                Console.ForegroundColor = colorPalet["ActiveFrame"];
                            else
                                Console.ForegroundColor = colorPalet["InnerFrame"];
                            Console.Write(frame["InnerFrameHor"]);

                            if (i < 2)
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
        }

        public bool Turn(int i)
        {
            return grid[i].WriteInCell(0, 1);
        }
        public void SetCellarActive(int id)
        {
            int i = GetActiveCellar();
            if (i >= 0)
                grid[i].active = false;

            if (id < grid.Length)
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
