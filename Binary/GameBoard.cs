

namespace Binary
{
    public enum CellType { Light, Dark, Stone, Empty }

    internal class GameBoard : IComparable<GameBoard>
    {
        public CellType[,] board;
        private int[,] stoneValues;
        private int rows;
        private int cols;
        public int cost;
        public int heuristic;
        public int Depth = 0;
        public GameBoard? parent;

        public GameBoard(string levelKey, string jsonFilePath = "files/levelsConfig.json")
        {
            var config = Loader.LoadBoardConfig(jsonFilePath, levelKey);
            cost = 0;
            rows = config.Rows;
            cols = config.Cols;
            stoneValues = config.StoneValues;
            board = new CellType[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    board[i, j] = Enum.Parse<CellType>(config.Board[i][j]);
                }
            }
            heuristic = Heuristic();
        }

        public GameBoard(GameBoard other)
        {
            rows = other.rows;
            cols = other.cols;
            heuristic = other.heuristic;
            cost = other.cost;
            stoneValues = (int[,])other.stoneValues.Clone();
            board = new CellType[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    board[i, j] = other.board[i, j];
                }
            }
        }

        public void PrintBoard()
        {
            Console.WriteLine($"╔{new string('═', cols * 2 + 1)}╗");
            for (int i = 0; i < rows; i++)
            {
                Console.Write("║ ");
                for (int j = 0; j < cols; j++)
                {
                    switch (board[i, j])
                    {
                        case CellType.Light:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("L ");
                            break;
                        case CellType.Dark:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write("D ");
                            break;
                        case CellType.Empty:
                            Console.Write("  ");
                            break;
                        case CellType.Stone:
                            if (IsValidStone(i, j))
                                Console.ForegroundColor = ConsoleColor.Green;
                            else Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"{stoneValues[i, j]} ");
                            break;
                    }
                    Console.ResetColor();
                }
                Console.WriteLine("║");
            }
            Console.WriteLine($"╚{new string('═', cols * 2 + 1)}╝");
        }

        public bool CanPlay(int row, int col)
        {
            return board[row, col] == CellType.Light;
        }

        public void MakeMove(int row, int col)
        {
            if (!IsInBounds(row, col) || !CanPlay(row, col)) return;

            board[row, col] = CellType.Dark;
            ToggleAdjacentCells(row, col);
        }

        private void ToggleAdjacentCells(int row, int col)
        {
            var directions = new[] { new[] { -1, 0 }, new[] { 1, 0 }, new[] { 0, -1 }, new[] { 0, 1 } };
            foreach (var dir in directions)
            {
                int newRow = row + dir[0];
                int newCol = col + dir[1];
                if (IsInBounds(newRow, newCol) && board[newRow, newCol] != CellType.Stone && board[newRow, newCol] != CellType.Empty)
                {
                    board[newRow, newCol] = board[newRow, newCol] == CellType.Light ? CellType.Dark : CellType.Light;
                }
            }
        }

        public bool IsInBounds(int row, int col)
        {
            return row >= 0 && row < rows && col >= 0 && col < cols;
        }

        public bool IsGoalState()
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    if (board[i, j] == CellType.Stone && !IsValidStone(i, j)) 
                        return false;
            return true;
        }

        private bool IsValidStone(int row, int col)
        {
            int count = 0;
            int requiredBlues = stoneValues[row, col];
            var directions = new[] { new[] { -1, 0 }, new[] { 1, 0 }, new[] { 0, -1 }, new[] { 0, 1 } };
            foreach (var dir in directions)
            {
                int newRow = row + dir[0];
                int newCol = col + dir[1];
                if (IsInBounds(newRow, newCol) && board[newRow, newCol] == CellType.Light) count++;
            }
            return count == requiredBlues;
        }

        public List<GameBoard> GetPossibleStates()
        {
            List<GameBoard> states = new List<GameBoard>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (CanPlay(i, j))
                    {
                        GameBoard newBoard = new GameBoard(this);
                        newBoard.MakeMove(i, j);
                        newBoard.parent = this;
                        newBoard.cost = this.cost + 1;
                        newBoard.heuristic = newBoard.Heuristic();
                        states.Add(newBoard);
                    }
                }
            }
            return states;
        }

        public void PrintPath()
        {
            GameBoard current = this;
            Stack<GameBoard> path = new Stack<GameBoard>();
            while (current != null)
            {
                path.Push(current);
                current = current.parent;
            }
            Console.WriteLine();
            path.Pop().PrintBoard();
            Console.WriteLine();
            int i = 1;
            while (path.Count > 0)
            {
                current = path.Pop();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine();
                Console.WriteLine("step : " + i++);
                Console.ResetColor();
                current.PrintBoard();
            }
        }

        public int Heuristic()
        {
            int count = 0;
            int requiredBlues = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    requiredBlues = stoneValues[i, j];
                    if (board[i, j] == CellType.Stone) {
                            count += Math.Abs(GoalLightCellCount(i, j)-requiredBlues);
                    }
                }
            }
            return count;
        }

        private int GoalLightCellCount(int row, int col)
        {
            int count = 0;
            var directions = new[] { new[] { -1, 0 }, new[] { 1, 0 }, new[] { 0, -1 }, new[] { 0, 1 } };
            foreach (var dir in directions)
            {
                int newRow = row + dir[0];
                int newCol = col + dir[1];
                if (IsInBounds(newRow, newCol) && board[newRow, newCol] == CellType.Light)
                    count++;
            }
            return count;
        }


        public override bool Equals(object obj)
        {
            if (obj is not GameBoard other)
                return false;

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    if (board[i, j] != other.board[i, j])
                        return false;

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var cell in board)
                hash = hash * 31 + cell.GetHashCode();
            return hash;
        }


        public int CompareTo(GameBoard other)
        {
            if (other == null) return 1;
            return this.cost.CompareTo(other.cost);
        }
    }

}