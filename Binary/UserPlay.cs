namespace Binary
{
    internal class UserPlay
    {
        private GameBoard gameBoard;
        public UserPlay(GameBoard gameBoard)
        {
            this.gameBoard = new GameBoard(gameBoard);
        }

        public void Start()
        {
            Console.WriteLine();
            gameBoard.PrintBoard();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Enter light cell (like: 1 2): ");
            Console.ResetColor();

            while (true)
            {
                if (gameBoard.IsGoalState())
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Congratulations! You've solved the puzzle.");
                    Console.ResetColor();
                    break;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("cell: ");
                Console.ResetColor();

                var input = Console.ReadLine();
                if (TryParseInput(input, out int rows, out int cols))
                {
                    gameBoard.MakeMove(rows - 1, cols - 1);
                    Console.WriteLine();
                    gameBoard.PrintBoard();
                }
                else
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please try again.");
                    Console.ResetColor();
                }
            }
        }

        private bool TryParseInput(string input, out int r, out int c)
        {
            var parts = input.Split();

            if (parts.Length == 2 &&
                int.TryParse(parts[0], out r) &&
                int.TryParse(parts[1], out c) &&
                r > 0 && r <= gameBoard.board.GetLength(0) &&
                c > 0 && c <= gameBoard.board.GetLength(1) &&
                gameBoard.board[r - 1, c - 1] == CellType.Light)
            {
                return true;
            }

            r = c = 0;
            return false;
        }
    }
}
