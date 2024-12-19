namespace Binary
{
    internal class AStar
    {
        private GameBoard gameBoard;
        private HashSet<GameBoard> visited = new HashSet<GameBoard>();
        private SortedSet<(int, GameBoard)> priorityQueue = new SortedSet<(int, GameBoard)>(Comparer<(int, GameBoard)>.Create((a, b) =>
        {
            int fScoreComparison = a.Item1.CompareTo(b.Item1);
            return fScoreComparison != 0 ? fScoreComparison : a.Item2.GetHashCode().CompareTo(b.Item2.GetHashCode());
        }));

        private HashSet<GameBoard> parents = new HashSet<GameBoard>();

        public AStar(GameBoard gameBoard)
        {
            this.gameBoard = new GameBoard(gameBoard);
        }

        public bool Run()
        {
            int generate = 1;
            int depth = 0;

            priorityQueue.Add((gameBoard.heuristic + gameBoard.cost, gameBoard));

            while (priorityQueue.Count > 0)
            {
                var (_, current) = priorityQueue.Min;
                priorityQueue.Remove(priorityQueue.Min);

                if (current.IsGoalState())
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("Tree Depth: ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(depth);
                    Console.ResetColor();

                    Game.GoalStateOptions(current, generate, visited.Count, "A*");
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
                    if (visited.Contains(child))
                    {
                        var visitedNode = visited.FirstOrDefault(node => node.Equals(child));
                        if (visitedNode != null && visitedNode.cost > child.cost)
                        {
                            visited.Remove(visitedNode);
                            priorityQueue.Add((child.heuristic + child.cost, child));
                        }
                    }
                    else
                    {
                        var existingNode = priorityQueue.FirstOrDefault(item => item.Item2.Equals(child));

                        if (existingNode.Item1 != null && existingNode.Item1 > child.heuristic + child.cost)
                        {
                            priorityQueue.Remove(existingNode);
                            priorityQueue.Add((child.heuristic + child.cost, child));
                        }
                        else
                        {
                            priorityQueue.Add((child.heuristic + child.cost, child));
                            generate++;

                        }
                    }
                }
            }
            return false;
        }
    }
}
