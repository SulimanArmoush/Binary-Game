namespace Binary
{
    internal class HC
    {
        private GameBoard gameBoard;
        private HashSet<GameBoard> visited = new HashSet<GameBoard>();
        private SortedSet<(int, GameBoard)> priorityQueue = new SortedSet<(int, GameBoard)>(Comparer<(int, GameBoard)>.Create((a, b) =>
        {
            int heuristicComparison = a.Item1.CompareTo(b.Item1);
            if (heuristicComparison != 0)
                return heuristicComparison;

            return a.Item2.GetHashCode().CompareTo(b.Item2.GetHashCode());
        }));

        private HashSet<GameBoard> parents = new HashSet<GameBoard>();


        public HC(GameBoard gameBoard)
        {
            this.gameBoard = new GameBoard(gameBoard);
        }

        public bool Run()
        {
            int generate = 1;
            int depth = 0;

            priorityQueue.Add((gameBoard.heuristic, gameBoard));

            while (priorityQueue.Count > 0)
            {
                var (_, current) = priorityQueue.Min;
                priorityQueue.Remove(priorityQueue.Min);

                if (current.IsGoalState())
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("Tree Depth: ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(parents.Count);
                    Console.ResetColor();


                    Game.GoalStateOptions(current, generate, visited.Count, "HC");
                    return true;
                }

                visited.Add(current);

                if (!parents.Contains(current.parent))
                {
                    depth++;
                }
                parents.Add(current.parent);

                foreach (GameBoard child in current.GetPossibleStates())
                {
                    if (!visited.Contains(child))
                    {
                        priorityQueue.Add((child.heuristic, child));
                        generate++;
                    }
                }
            }
            return false;
        }
    }
}
