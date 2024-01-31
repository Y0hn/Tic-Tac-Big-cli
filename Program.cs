using System;

namespace Tic_Tac_Big
{
    internal class Program
    {
        static void Main()
        {
            int input, playerTurn = 1;

            do
            {
                BattleField pole = new BattleField();
                Console.ForegroundColor = ConsoleColor.White;

                while (!pole.WinnerCheck())
                {
                    Console.Clear();
                    pole.WriteOut();
                    Console.WriteLine("\n");
                    if (!pole.IsCellarActive())
                    {
                        do
                        { GetInput($"Choose place set active for |player {playerTurn}|: ", out input, 1, 10); }
                        while (!pole.MakeCellarActive(input - 1));
                        continue;
                    }
                    else
                    {
                        do
                        { GetInput($"Place |player {playerTurn}| into active: ", out input, 1, 10); } 
                        while (!pole.PlaceIntoActive(input - 1, playerTurn));
                    }

                    // Change Turns
                    if (playerTurn == 1)
                        playerTurn = 2;
                    else // if (playerTurn == 2)
                        playerTurn = 1;
                }

                Console.Clear();
                Console.ForegroundColor = BattleField.GetColor(GetWinner(playerTurn).ToString());
                Console.WriteLine($"\n\n\t\tTHE WINNER IS PLAYER {GetWinner(playerTurn)} !!");
            }
            while (GetContinue());
            Console.Clear();
        }
        static void GetInput(string mess, out int input, int min = -1, int max = -1, char splitter = '|')
        {
            /// Does not include max numbers 
            
            string[] s = mess.Split(splitter);
            string playerTurn = s[1].Trim().Split(' ')[1];

            do
            {
                do
                {
                    Console.ForegroundColor = BattleField.GetColor("Base");
                    Console.Write(s[0]);

                    Console.ForegroundColor = BattleField.GetColor(playerTurn);
                    Console.Write(s[1]);

                    Console.ForegroundColor = BattleField.GetColor("Base");
                    Console.Write(s[2]);
                }
                while (!int.TryParse(Console.ReadLine(), out input));
            }
            while (!((min == -1 || min <= input) && (max == -1 || input < max)));
        }
        static int GetWinner(int playerOnTurn)
        {
            /// Inverses the winner becouse winner is the one who was on turn last time
            if (playerOnTurn == 1)
                return 2;
            else // if (playerOnTurn == 2)
                return 1;
        }
        static bool GetContinue()
        {
            Console.ForegroundColor = BattleField.GetColor("Base");
            Console.Write("\n\nDo you want a remach? [y/N]  ");
            string s = Console.ReadLine();
            if (s == "") 
                return false;
            return Console.ReadLine().ToLower().Trim().ToCharArray()[0].Equals('y');
        }
    }
}