using System;
using System.Collections.Generic;

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

        // Returns the field with bot's moving
        static void BotMove(ref char[][] field, ref int x, ref int y)
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
                        x = i;
                        y = l;
                        return;
                    }
                }
            }
        }

        // Prints field
        static void PrintField(char[][] field)
        {
            for (int i = 0; i < field.Length; i++)
            {
                Console.Write('\n');
                Console.Write((field.Length - i) + "|");
                for (int l = 0; l < field.Length; l++)
                {
                    Console.Write(field[i][l] + "|");
                }
            }

            Console.Write('\n');

            for (int i = 0; i <= field.Length; i++)
            {
                Console.Write(i + "|");
            }

            Console.WriteLine('\n');
        }

        // Checks can there be new combinations
        static bool canBeNewCombinations(char[][] field)
        {
            // Brute force of all possible combinations for one cell

            for (int i = 0; i < field.Length; i++)
            {
                for (int l = 0; l < field.Length - 2; l++)
                {
                    try
                    {
                        if (field[i][l + 1] == field[i][l + 2] && field[i][l] == '.')
                        {
                            return true;
                        }

                        if (field[i + 1][l] == field[i + 2][l] && field[i][l] == '.')
                        {
                            return true;
                        }

                        if (field[i + 1][l + 1] == field[i + 2][l + 2] && field[i][l] == '.')
                        {
                            return true;
                        }

                        if (field[i + 1][l - 1] == field[i + 2][l - 2] && field[i][l] == '.')
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        // nothing
                    }
                }
            }

            return false;
        }

        // Prepairs variables for game
        static void InitializeNewGame(bool twoPlayers)
        {
            // Input size of field and field prepairing

            int size = 0,combinationZero = 0, combinationCross = 0;
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

            Game(ref field, ref combinationZero, ref combinationCross, twoPlayers);

            // Conclusion

            Console.Clear();

            PrintField(field);
            Console.WriteLine("\nPlayer 1:" + combinationCross + (twoPlayers?"\nPlayer 2:":"\nBot:") + combinationZero + "\n");

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

        // Comparison between three symbols
        // Throws:
        // 0 - when 3 symbols can become combination
        // 1 - when 3 symbols cannot become combination
        // 2 - when 3 symbols are combination of *
        // 3 - when 3 symbols are combination of 0
        static int ComparisonBetween(char c1, char c2, char c3)
        {
            // Checking if all symbols are combination
            if (c1 == c2 && c1 == c3)
            {
                switch (c1)
                {
                    case '*':
                        return 2;
                    case '0':
                        return 3;
                }
            }

            // If all symbols are different they cannot become a combination
            if (c1 != c2 && c2 != c3 && c1 != c3)
            {
                return 1;
            }

            // If there is a dot and two identical symbols(or two dots and one symbol) they can become a combination
            if (c1 == '.' || c2 == '.' || c3 == '.')
            {
                return 0;
            }

            // Else they cannot become a combination
            return 1;
        }

        // Remove not possible combinations and update count of combinations
        static void RemoveNotPossibleCombinationsAndUpdateCountOfCombinations(ref List<Tuple<string, int[]>> possibleCombinations, char[][] field, ref int combinationZero, ref int combinationCross,char letter, int comparison, int x, int y, int number, ref int iter)
        {
            if (comparison == 0)
            {
                return;
            }

            if (comparison > 1)
            {
                for (int j = 0;j < possibleCombinations.Count; j++)
                {
                    int[] combination = possibleCombinations[j].Item2;
                    switch (letter)
                    {
                        case 'v':
                            if ((x + 1 == combination[0] && y == combination[1]) || (x + 2 == combination[0] && y == combination[1]))
                            {
                                if (possibleCombinations[j].Item1.IndexOf(letter) > -1 && (field[x + 1][y] == field[x][y] || field[x + 2][y] == field[x][y]))
                                {
                                   possibleCombinations[j] = new Tuple<string, int[]>(possibleCombinations[j].Item1.Remove(possibleCombinations[j].Item1.IndexOf(letter)), possibleCombinations[j].Item2);
                                }
                            }
                            break;
                        case 'h':
                            if ((x == combination[0] && y + 1 == combination[1]) || (x == combination[0] && y + 2 == combination[1]))
                            {
                                if (possibleCombinations[j].Item1.IndexOf(letter) > -1 && (field[x][y + 1] == field[x][y] || field[x][y + 2] == field[x][y]))
                                {
                                    possibleCombinations[j] = new Tuple<string, int[]>(possibleCombinations[j].Item1.Remove(possibleCombinations[j].Item1.IndexOf(letter)), possibleCombinations[j].Item2);
                                }
                            }
                            break;
                        case 'd':
                            if ((x + 1 == combination[0] && y + 1 == combination[1]) || (x + 2 == combination[0] && y + 2 == combination[1]))
                            {
                                if (possibleCombinations[j].Item1.IndexOf(letter) > -1 && (field[x + 1][y + 1] == field[x][y] || field[x + 2][y + 2] == field[x][y]))
                                {
                                    possibleCombinations[j] = new Tuple<string, int[]>(possibleCombinations[j].Item1.Remove(possibleCombinations[j].Item1.IndexOf(letter)), possibleCombinations[j].Item2);
                                }
                            }
                            break;
                        case 'a':
                            if ((x + 1 == combination[0] && y - 1 == combination[1]) || (x + 2 == combination[0] && y - 2 == combination[1]))
                            {
                                if (possibleCombinations[j].Item1.IndexOf(letter) > -1 && (field[x + 1][y - 1] == field[x][y] || field[x + 2][y - 2] == field[x][y]))
                                {
                                    possibleCombinations[j] = new Tuple<string, int[]>(possibleCombinations[j].Item1.Remove(possibleCombinations[j].Item1.IndexOf(letter)), possibleCombinations[j].Item2);
                                }
                            }
                            break;
                    }
                }
            }

            if (comparison == 2)
            {
                combinationCross++;
            }
            else if (comparison == 3)
            {
                combinationZero++;
            }

            possibleCombinations[number] = new Tuple<string, int[]>(possibleCombinations[number].Item1.Remove(possibleCombinations[number].Item1.IndexOf(letter)), possibleCombinations[number].Item2);
            iter--;
        }

        // Update new possiple combinations
        static void UpdateCombinations(ref List<Tuple<string, int[]>> possibleCombinations, char[][] field ,ref int combinationZero, ref int combinationCross, int x, int y, int size)
        {
            string possibleDirections = String.Empty;
            int comparison;

            if (x + 2 < size)
            {
                possibleDirections += 'v';
            }
            if (y + 2 < size)
            {
                possibleDirections += 'h';
            }
            if (x + 2 < size && y + 2 < size)
            {
                possibleDirections += 'd';
            }
            if (x + 2 < size && y > 1)
            {
                possibleDirections += 'a';
            }

            possibleCombinations.Add(new Tuple<string, int[]>(possibleDirections, new int[2]{ x, y }));

            for (int i = 0;i < possibleCombinations.Count;i++)
            {
                int additionalX = possibleCombinations[i].Item2[0], additionalY = possibleCombinations[i].Item2[1];
                possibleDirections = possibleCombinations[i].Item1;

                for (int l = 0;l < possibleDirections.Length;l++)
                {
                    switch (possibleDirections[l])
                    {
                        case 'h':
                            comparison = ComparisonBetween(field[additionalX][additionalY], field[additionalX][additionalY + 1], field[additionalX][additionalY + 2]);

                            RemoveNotPossibleCombinationsAndUpdateCountOfCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, possibleDirections[l], comparison, additionalX, additionalY, i, ref l);
                            possibleDirections = possibleCombinations[i].Item1;
                            break;
                        case 'v':
                            comparison = ComparisonBetween(field[additionalX][additionalY], field[additionalX + 1][additionalY], field[additionalX + 2][additionalY]);

                            RemoveNotPossibleCombinationsAndUpdateCountOfCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, possibleDirections[l], comparison, additionalX, additionalY, i, ref l);
                            possibleDirections = possibleCombinations[i].Item1;
                            break;
                        case 'd':
                            comparison = ComparisonBetween(field[additionalX][additionalY], field[additionalX + 1][additionalY + 1], field[additionalX + 2][additionalY + 2]);

                            RemoveNotPossibleCombinationsAndUpdateCountOfCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, possibleDirections[l], comparison, additionalX, additionalY, i, ref l);
                            possibleDirections = possibleCombinations[i].Item1;
                            break;
                        case 'a':
                            comparison = ComparisonBetween(field[additionalX][additionalY], field[additionalX + 1][additionalY - 1], field[additionalX + 2][additionalY - 2]);

                            RemoveNotPossibleCombinationsAndUpdateCountOfCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, possibleDirections[l], comparison, additionalX, additionalY, i, ref l);
                            possibleDirections = possibleCombinations[i].Item1;
                            break;
                    }
                }
                
                if (possibleDirections == String.Empty)
                {
                    possibleCombinations.RemoveAt(i--);
                }
            }
        }

        // Starts game
        static void Game(ref char[][] field, ref int  combinationZero, ref int combinationCross,bool twoPlayers)
        {
            int size = field.Length, x = 0, y = 0, n = 0;
            List<Tuple<string, int[]>> possibleCombinations = new List<Tuple<string, int[]>>();

            while (possibleCombinations.Count > 0 || canBeNewCombinations(field))
            {
                n++;
                Console.Clear();

                PrintField(field);

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

                if ((x > size || y > size) || field[size - y][x - 1] != '.' || (x == 0 || y == 0))
                {
                    Console.WriteLine("Please input cell that is not * or 0(press any button)");
                    Console.ReadKey();
                    continue;
                }

                field[size - y][x - 1] = '*';


                UpdateCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, size - y, x - 1, size);

                if (!(possibleCombinations.Count > 0 || canBeNewCombinations(field)) || (size % 2 == 1 && ((size*size) - 1) / 2 == n))
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

                    if ((x > size || y > size) || field[size - y][x - 1] != '.')
                    {
                        Console.WriteLine("Please input cell that is not * or 0(press any button)");
                        Console.ReadKey();
                        continue;
                    }

                    field[size - y][y - 1] = '0';

                    UpdateCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, size - y, x - 1, size);
                }
                else
                {
                    BotMove(ref field, ref x, ref y);

                    field[x][y] = '0';

                    UpdateCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, x, y, size);
                }
            }
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
                        InitializeNewGame(false);
                        break;
                    case 1:
                        InitializeNewGame(true);
                        break;
                    case 2:
                        isRunning = false;
                        break;
                }
            }
        }
    }
}
