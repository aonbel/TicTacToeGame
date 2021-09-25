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
                    case '.':
                        return 0;
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

        // Checks can there be new combinations
        static bool canBeNewCombinations(char[][] field)
        {
            // Brute force of all possible combinations for one cell

            for (int i = 0; i < field.Length; i++)
            {
                for (int l = 0; l < field.Length - 2; l++)
                {
                    if (l + 2 < field.Length && ComparisonBetween(field[i][l], field[i][l + 1], field[i][l + 2]) == 0)
                    {
                        return true;
                    }
                
                    if (i + 2 < field.Length && ComparisonBetween(field[i][l], field[i + 1][l], field[i + 2][l]) == 0)
                    {
                        return true;
                    }

                    if (i + 2 < field.Length && l + 2 < field.Length && ComparisonBetween(field[i][l], field[i + 1][l + 1], field[i + 2][l + 2]) == 0)
                    {
                        return true;
                    }

                    if (i + 2 < field.Length && l > 1 && ComparisonBetween(field[i][l], field[i + 1][l - 1], field[i + 2][l - 2]) == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Prepairs variables for game
        static void InitializeNewGame(bool twoPlayers, bool isCross)
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

            Game(ref field, ref combinationZero, ref combinationCross, twoPlayers, isCross);

            // Conclusion

            Console.Clear();

            PrintField(field);
            Console.WriteLine("\nPlayer 1:" + (isCross ? combinationCross:combinationZero) + (twoPlayers?"\nPlayer 2:":"\nBot:") + (isCross ? combinationZero : combinationCross) + "\n");

            if (combinationCross > combinationZero)
            {
                Console.WriteLine(isCross ? "Player 1 wins!!!(press any button)": twoPlayers?"Player 2 wins!!!(press any button)":"Bot wins!!!(press any button)");
            }
            else if (combinationCross == combinationZero)
            {
                Console.WriteLine("Draw!!!(press any button)");
            }
            else 
            {
                Console.WriteLine(isCross ? "Player 2 wins!!!(press any button)" : twoPlayers ? "Player 1 wins!!!(press any button)" : "Bot wins!!!(press any button)");
            }
            Console.ReadKey();
        }

        // Remove not possible combinations and update count of combinations
        static void RemoveNotPossibleCombinationsAndUpdateCountOfCombinations(ref List<Tuple<string[], int[]>> possibleCombinations, char[][] field, ref int combinationZero, ref int combinationCross,char letter, int comparison, int x, int y, int number)
        {
            if (possibleCombinations[number].Item1[1].IndexOf(letter) == -1)
            {
                comparison = 1;
            }

            if (comparison == 0)
            {
                return;
            }

            if (comparison != 1)
            {
                for (int j = 0;j < possibleCombinations.Count; j++)
                {
                    int[] combination = possibleCombinations[j].Item2;
                    switch (letter)
                    {
                        case 'v':
                            if ((x + 1 == combination[0] && y == combination[1]) || (x + 2 == combination[0] && y == combination[1]))
                            {
                                int index1 = possibleCombinations[j].Item1[1].IndexOf(letter);
                                if (index1 != -1 && (field[combination[0]][combination[1]] == field[x][y]))
                                {
                                    possibleCombinations[j] = new Tuple<string[], int[]>(new string[2] { possibleCombinations[j].Item1[0], possibleCombinations[j].Item1[1].Remove(index1, 1) }, possibleCombinations[j].Item2);
                                }
                                else if (index1 == -1)
                                {
                                    return;
                                }
                            }
                            break;
                        case 'h':
                            if ((x == combination[0] && y + 1 == combination[1]) || (x == combination[0] && y + 2 == combination[1]))
                            {
                                int index1 = possibleCombinations[j].Item1[1].IndexOf(letter);
                                if (index1 != -1 && (field[combination[0]][combination[1]] == field[x][y]))
                                {
                                    possibleCombinations[j] = new Tuple<string[], int[]>(new string[2] { possibleCombinations[j].Item1[0], possibleCombinations[j].Item1[1].Remove(index1, 1) }, possibleCombinations[j].Item2);
                                }
                                else if (index1 == -1)
                                {
                                    return;
                                }
                            }
                            break;
                        case 'd':
                            if ((x + 1 == combination[0] && y + 1 == combination[1]) || (x + 2 == combination[0] && y + 2 == combination[1]))
                            {
                                int index1 = possibleCombinations[j].Item1[1].IndexOf(letter);
                                if (index1 != -1 && (field[combination[0]][combination[1]] == field[x][y]))
                                {
                                    possibleCombinations[j] = new Tuple<string[], int[]>(new string[2] { possibleCombinations[j].Item1[0], possibleCombinations[j].Item1[1].Remove(index1, 1) }, possibleCombinations[j].Item2);
                                }
                                else if (index1 == -1)
                                {
                                    return;
                                }
                            }
                            break;
                        case 'a':
                            if ((x + 1 == combination[0] && y - 1 == combination[1]) || (x + 2 == combination[0] && y - 2 == combination[1]))
                            {
                                int index1 = possibleCombinations[j].Item1[1].IndexOf(letter);
                                if (index1 != -1 && (field[combination[0]][combination[1]] == field[x][y]))
                                {
                                    possibleCombinations[j] = new Tuple<string[], int[]>(new string[2] { possibleCombinations[j].Item1[0], possibleCombinations[j].Item1[1].Remove(index1, 1) }, possibleCombinations[j].Item2);
                                }
                                else if (index1 == -1)
                                {
                                    return;
                                }
                            }
                            break;
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


                int indexOfLet = possibleCombinations[number].Item1[1].IndexOf(letter);
                possibleCombinations[number] = new Tuple<string[], int[]>(new string[2] { possibleCombinations[number].Item1[0], possibleCombinations[number].Item1[1].Remove(indexOfLet, 1) }, possibleCombinations[number].Item2);
            }

            int indexOfLetter = possibleCombinations[number].Item1[0].IndexOf(letter);
            possibleCombinations[number] = new Tuple<string[], int[]>(new string[2] { possibleCombinations[number].Item1[0].Remove(indexOfLetter, 1), possibleCombinations[number].Item1[1] }, possibleCombinations[number].Item2);
        }

        // Update new possiple combinations
        // Return true when count of possible combinations is 1 or greater
        // Otherwise, return true
        static bool UpdateCombinations(ref List<Tuple<string[], int[]>> possibleCombinations, char[][] field, ref int combinationZero, ref int combinationCross, int x, int y, int size, ref int countOfCombinations)
        {
            string possibleDirections = String.Empty;
            int comparison;

            countOfCombinations++;

            // Looking for what directions can combination go
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

            // Add this combination to the list of combinations
            possibleCombinations.Add(new Tuple<string[], int[]>(new string[2] { possibleDirections, "hvda" }, new int[2]{ x, y }));

            // Whatching are all combinations correct
            for (int i = possibleCombinations.Count - 1; i > - 1 ;i--)
            {
                int additionalX = possibleCombinations[i].Item2[0], additionalY = possibleCombinations[i].Item2[1];
                possibleDirections = possibleCombinations[i].Item1[0];

                for (int l = 0;l < possibleDirections.Length;l++)
                {
                    switch (possibleDirections[l])
                    {
                        case 'h':
                            comparison = ComparisonBetween(field[additionalX][additionalY], field[additionalX][additionalY + 1], field[additionalX][additionalY + 2]);

                            RemoveNotPossibleCombinationsAndUpdateCountOfCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, 'h', comparison, additionalX, additionalY, i);
                            break;
                        case 'v':
                            comparison = ComparisonBetween(field[additionalX][additionalY], field[additionalX + 1][additionalY], field[additionalX + 2][additionalY]);

                            RemoveNotPossibleCombinationsAndUpdateCountOfCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, 'v', comparison, additionalX, additionalY, i);
                            break;
                        case 'd':
                            comparison = ComparisonBetween(field[additionalX][additionalY], field[additionalX + 1][additionalY + 1], field[additionalX + 2][additionalY + 2]);

                            RemoveNotPossibleCombinationsAndUpdateCountOfCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, 'd', comparison, additionalX, additionalY, i);
                            break;
                        case 'a':
                            comparison = ComparisonBetween(field[additionalX][additionalY], field[additionalX + 1][additionalY - 1], field[additionalX + 2][additionalY - 2]);

                            RemoveNotPossibleCombinationsAndUpdateCountOfCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, 'a', comparison, additionalX, additionalY, i);
                            break;
                    }

                    if (possibleCombinations[i].Item1[0] != possibleDirections)
                    {
                        possibleDirections = possibleCombinations[i].Item1[0];

                        l = -1;
                    }
                }
            }

            if (size * size != possibleCombinations.Count)
            {
                return true;
            }

            for (int i = 0;i < possibleCombinations.Count;i++)
            {
                if (possibleCombinations[i].Item1[0] != String.Empty)
                {
                    return true;
                }
            }

            return false;
        }

        // Starts game
        static void Game(ref char[][] field, ref int  combinationZero, ref int combinationCross, bool twoPlayers, bool isCross)
        {
            int size = field.Length, x = 0, y = 0, n = 0, countOfCombinations = 0;
            List<Tuple<string[], int[]>> possibleCombinations = new List<Tuple<string[], int[]>>();
            bool canContinue = true;

            while (canContinue && canBeNewCombinations(field))
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

                field[size - y][x - 1] = isCross?'*':'0';

                canContinue = UpdateCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, size - y, x - 1, size, ref countOfCombinations);

                if (!canContinue && !canBeNewCombinations(field))
                {
                    break;
                }

                if (twoPlayers)
                {
                    Console.Clear();

                    PrintField(field);

                    Console.WriteLine("Player 2 x coordinate:");
                    if (!ConsoleReadInt(ref x))
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

                    field[size - y][x - 1] = !isCross ? '*' : '0';

                    canContinue = UpdateCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, size - y, x - 1, size, ref countOfCombinations);
                }
                else
                {
                    BotMove(ref field, ref x, ref y);

                    field[x][y] = !isCross ? '*' : '0';


                    canContinue = UpdateCombinations(ref possibleCombinations, field, ref combinationZero, ref combinationCross, x, y, size, ref countOfCombinations);
                }
            }
        }

        static void Main(string[] args)
        {
            bool isRunning = true;

            while (isRunning)
            {
                int choise = MainMenu("Tic-tac-toe game", new string[] { "1 player(cross)", "1 player(zero)", "2 players", "Exit" });

                switch (choise)
                {
                    case 0:
                        InitializeNewGame(false, true);
                        break;
                    case 1:
                        InitializeNewGame(false, false);
                        break;
                    case 2:
                        InitializeNewGame(true, true);
                        break;
                    case 3:
                        isRunning = false;
                        break;
                }
            }
        }
    }
}
