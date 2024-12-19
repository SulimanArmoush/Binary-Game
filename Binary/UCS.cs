namespace Binary
{
    internal class UCS
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

        public UCS(GameBoard gameBoard)
        {
            this.gameBoard = new GameBoard(gameBoard);
        }

        public bool Run()
        {
            int generate = 1;
            priorityQueue.Add((gameBoard.cost, gameBoard));

            while (priorityQueue.Count > 0)
            {
                var (_, current) = priorityQueue.Min;
                priorityQueue.Remove(priorityQueue.Min());

                if (current.IsGoalState())
                {
                    Game.GoalStateOptions(current, generate, visited.Count, "UCS");
                    return true;
                }

                visited.Add(current);

                foreach (GameBoard child in current.GetPossibleStates())
                {
                    if (!visited.Contains(child))
                    {
                        priorityQueue.Add((child.cost, child));
                        generate++;
                    }
                }
            }
            return false;
        }
    }
}
