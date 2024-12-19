namespace Binary
{
    internal class Game
    {
        private GameBoard? gameBoard;
        private UserPlay? userPlay;
        private BFS? bfs;
        private DFS? dfs;
        private UCS? ucs;
        private HC? hc;
        private AStar? aStar;

        private bool win;
        private int key;

        public void StartGame()
        {
            bool gameRunning = true;
            while (gameRunning)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Enter the level (1, 2, ... 10): ");
                Console.Write("Level: ");
                Console.ResetColor();

                if (int.TryParse(Console.ReadLine(), out key) && key >= 1 && key <= 10)
                {
                    try
                    {
                        gameBoard = new GameBoard("level" + key);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        continue;
                    }
                    Console.Clear();
                    gameRunning = PlayGame();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Invalid input. Please enter a number between 1 and 10: ");
                    Console.ResetColor();
                }
            }
        }

        public bool PlayGame()
        {
            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("0 - Exit");
                Console.WriteLine("1 - Choose another level");
                Console.WriteLine("2 - User Play");
                Console.WriteLine("3 - DFS");
                Console.WriteLine("4 - BFS");
                Console.WriteLine("5 - UCS");
                Console.WriteLine("6 - HC");
                Console.WriteLine("7 - A*");


                Console.ResetColor();
                Console.Write("Enter your choice: ");

                if (int.TryParse(Console.ReadLine(), out int action))
                {
                    Console.Clear();
                    switch (action)
                    {
                        case 0:
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Exiting the game.");
                            Console.ResetColor();
                            return false;

                        case 1: return true;
                        case 2:
                            userPlay = new UserPlay(gameBoard);
                            userPlay.Start(); break;
                        case 3:
                            dfs = new DFS(gameBoard);
                            Start("DFS");
                            win = dfs.Run();
                            break;
                        case 4:
                            bfs = new BFS(gameBoard);
                            Start("BFS");
                            win = bfs.Run();
                            break;
                        case 5:
                            ucs = new UCS(gameBoard);
                            Start("UCS");
                            win = ucs.Run();
                            break;
                        case 6:
                            hc = new HC(gameBoard);
                            Start("HC");
                            win = hc.Run();
                            break;
                        case 7:
                            aStar = new AStar(gameBoard);
                            Start("A*");
                            win = aStar.Run();
                            break;
                        default:
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Option not available yet. Please choose again: ");
                            Console.ResetColor();
                            Console.WriteLine();
                            break;
                    }

                    if (win == false)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No solution found !");
                        Console.ResetColor();
                    }

                    if (action == 2 || action == 3 || action == 4 || action == 5 || action == 6 || action == 7)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Do you want to play again?");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("0 - No, Exit");
                        Console.WriteLine("1 - Yes, Reset");
                        Console.ResetColor();
                        Console.WriteLine();

                        if (int.TryParse(Console.ReadLine(), out int retryAction))
                        {
                            Console.Clear();
                            if (retryAction == 1)
                            {
                                return true;
                            }
                            else if (retryAction == 0)
                            {
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Exiting the game.");
                                Console.ResetColor();
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Invalid input. Please enter a number: ");
                    Console.ResetColor();
                    Console.WriteLine();

                }
            }

        }

        private void Start( string name)
        {
            Console.WriteLine();
            gameBoard.PrintBoard();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Starting "+name+" to solve the puzzle...");
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void GoalStateOptions(GameBoard gameBoard ,int generate,int visited,string name) 
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Puzzle solved using "+name+"!");
            Console.ResetColor();
            Console.WriteLine();

            gameBoard.PrintBoard();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("generated: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(generate);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("visited: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(visited);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("Number of solution path nodes : ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(gameBoard.cost);
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("do you wanna print solution path ? ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("0 - No");
            Console.WriteLine("1 - Yes");
            Console.ResetColor();
            Console.Write("Enter your choice: ");


            if (int.TryParse(Console.ReadLine(), out int action))
            {
                Console.Clear();
                switch (action)
                {
                    case 0: break;
                    case 1:
                        gameBoard.PrintPath();
                        Console.ReadLine();
                        Console.Clear();
                        break;
                }
            }
        }
    }
}