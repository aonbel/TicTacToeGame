using System;

namespace TicTacToeGame
{
    class TicTacToeMain
    {
        // Reads int from console
        static bool ConsoleReadInt(ref int source)
        {
            try
            {
                source = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        // Returns menu choice of player
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
        
        // Returns value of combinations on the field
        static void CheckCombinations(char[][] field,ref int combinationZero,ref int combinationCross)
        {
            combinationZero = 0;
            combinationCross = 0;

            // Brute force of all possible combinations for one cell

            for (int i = 0;i < field.Length;i++)
            {
                for (int l = 0;l < field.Length - 2;l++)
                {
                    if (field[i][l] == field[i][l + 1] && field[i][l] == field[i][l + 2])
                    {
                        if (field[i][l] == '0')
                        {
                            combinationZero++;
                            l += 2;
                        }
                        else if (field[i][l] == '*')
                        {
                            combinationCross++;
                            l += 2;
                        }
                    }
                }
            }

            for (int i = 0; i < field.Length; i++)
            {
                for (int l = 0; l < field.Length - 2; l++)
                {
                    if (field[l][i] == field[l + 1][i] && field[l][i] == field[l + 2][i])
                    {
                        if (field[l][i] == '0')
                        {
                            combinationZero++;
                            l += 2;
                        }
                        else if (field[l][i] == '*')
                        {
                            combinationCross++;
                            l += 2;
                        }
                    }
                }
            }

            for (int i = field.Length - 3; i >=  -field.Length + 3; i--)
            {
                for (int l = 0; l < 3 + i; l++)
                {
                    if (l + 2 < i + 3 && field[i][l] == field[i+1][l+1] && field[i][l] == field[i + 2][l + 2])
                    {
                        if (field[l][i] == '0')
                        {
                            combinationZero++;
                            l += 2;
                        }
                        else if  (field[l][i] == '*')
                        {
                            combinationCross++;
                            l += 2;
                        }
                    }
                }
            }
        }

        // Checks can the game be continued
        static bool CanBeContinued(char[][] field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                for (int l = 0; l < field.Length; l++)
                {
                    if (field[i][l] == '.')
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Returns the field with bot's moving
        static void BotMove(ref char[][] field)
        {
            int pointCount = 0;

            for (int i = 0; i < field.Length; i++)
            {
                for (int l = 0; l < field.Length; l++)
                {
                    if (field[i][l] == '.')
                    {
                        pointCount++;
                    }
                }
            }

            Random random = new Random();
            int move = random.Next(pointCount);

            for (int i = 0; i < field.Length; i++)
            {
                for (int l = 0; l < field.Length; l++)
                {
                    if (field[i][l] == '.')
                    {
                        move--;
                    }

                    if (move == -1)
                    {
                        field[i][l] = '0';
                        return;
                    }
                }
            }
        }

        // Prints field
        static void PrintField(char[][] field)
        {
            Console.Write('\n');
            for (int i = 0; i < field.Length; i++)
            {
                for (int l = 0; l < field.Length; l++)
                {
                    Console.Write(field[i][l]);
                }
                Console.Write('\n');
            }
        }

        // Starts game
        static void GameStart(bool twoPlayers)
        {
            // Input size of field and field prepairing

            int size = 0;
            bool isFirstTime = true;

            while (size < 3 || size > 30)
            {
                Console.Clear();
                if (!isFirstTime)
                {
                    Console.WriteLine("Incorrect size: size will be greater than 2 and less than 30. Value can not contain char.");
                }
                else
                {
                    isFirstTime = false;
                }

                Console.WriteLine("Please input size of the field:");

                ConsoleReadInt(ref size);
            }

            char[][] field = new char[size][];

            for(int it = 0;it < size;it++)
            {
                field[it] = new char[size];
                for (int j = 0; j < size; j++)
                {
                    field[it][j] = '.';
                }
            }

            // Begin to play

            while (CanBeContinued(field))
            {
                Console.Clear();

                PrintField(field);

                int x = 0, y = 0;

                Console.WriteLine("Player 1 x coordinate:");
                if (!ConsoleReadInt(ref x))
                {
                    Console.WriteLine("Input error(press any button)");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine("Player 1 y coordinate:");
                if (!ConsoleReadInt(ref y))
                {
                    Console.WriteLine("Input error(press any button)");
                    Console.ReadKey();
                    continue;
                }

                if ((x > size || y > size) || field[x - 1][y - 1] != '.')
                {
                    Console.WriteLine("Please input cell that is not * or 0(press any button)");
                    Console.ReadKey();
                    continue;
                }
                field[x - 1][y - 1] = '*';

                if (!CanBeContinued(field))
                {
                    break;
                }

                if (twoPlayers)
                {
                    Console.Clear();

                    PrintField(field);

                    
                    Console.WriteLine("Player 2 x coordinate:");
                    if (!ConsoleReadInt(ref y))
                    {
                        Console.WriteLine("Input error(press any button)");
                        Console.ReadKey();
                        continue;
                    }

                    Console.WriteLine("Player 2 y coordinate:");
                    if (!ConsoleReadInt(ref y))
                    {
                        Console.WriteLine("Input error(press any button)");
                        Console.ReadKey();
                        continue;
                    }

                    if ((x > size || y > size) || field[x - 1][y - 1] != '.')
                    {
                        Console.WriteLine("Please input cell that is not * or 0(press any button)");
                        Console.ReadKey();
                        continue;
                    }

                    field[x - 1][y - 1] = '0';
                }
                else
                {
                    BotMove(ref field);
                }
            }

            // Conclusion

            Console.Clear();

            int combinationZero = 0, combinationCross = 0;

            CheckCombinations(field, ref combinationZero, ref combinationCross);

            if (combinationCross > combinationZero)
            {
                Console.WriteLine("Player 1 wins!!!(press any button)");
            }
            else if (combinationCross == combinationZero)
            {
                Console.WriteLine("Draw!!!(press any button)");
            }
            else 
            {
                if (twoPlayers)
                {
                    Console.WriteLine("Player 2 wins!!!(press any button)");
                }
                else
                {
                    Console.WriteLine("Bot wins!!!(press any button)");
                }
            }
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            bool isRunning = true;

            while (isRunning)
            {
                int choise = MainMenu("Tic-tac-toe game", new string[] { "1 player(with bot)", "2 players", "Exit" });

                switch (choise)
                {
                    case 0:
                        GameStart(false);
                        break;
                    case 1:
                        GameStart(true);
                        break;
                    case 2:
                        isRunning = false;
                        break;
                }
            }
        }
    }
}
