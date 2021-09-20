using System;

namespace TicTacToeGame
{
    class TicTacToeMain
    {


        static int MainMenu(string gameTitle, string[] menuComponents)
        {
            int choise = 0;
            ConsoleKey key;

            do
            {
                // Upgrading console 

                Console.Clear();

                Console.WriteLine("\n\n\n              " + gameTitle + "\n\n");

                for (int i = 0; i < menuComponents.Length; i++)
                {
                    if (choise == i)
                    {
                        Console.WriteLine("    >> " + menuComponents[i]);
                    }
                    else
                    {
                        Console.WriteLine("       " + menuComponents[i]);
                    }
                }


                // Catching key pressings

                key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (choise > 0)
                        {
                            choise--;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (choise < menuComponents.Length - 1)
                        {
                            choise++;
                        }
                        break;
                    default:
                        break;
                }
            }
            while (key != ConsoleKey.Enter);

            return choise;
        }

        static void Main(string[] args)
        {
            bool isRunning = true;

            while (isRunning)
            {
                int choise = MainMenu("Tic-tac-toe game", new string[] { "1 player", "2 players", "Exit" });

                switch (choise)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        isRunning = false;
                        break;
                }
            }
        }
    }
}
