namespace Binary
{
    internal class BFS
    {
        private GameBoard gameBoard;
        private HashSet<GameBoard> visited = new HashSet<GameBoard>();
        private Queue<GameBoard> queue = new Queue<GameBoard>();
        public BFS(GameBoard gameBoard)
        {
            this.gameBoard = new GameBoard(gameBoard);
        }

        public bool Run()
        {
            queue.Enqueue(gameBoard);
            int generate = 1;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.IsGoalState())
                {
                    Game.GoalStateOptions(current, generate, visited.Count, "BFS");
                    return true;
                }
                visited.Add(current);
                foreach (GameBoard child in current.GetPossibleStates())
                {
                    if (!visited.Contains(child))
                    {
                        queue.Enqueue(child);
                        generate++;
                    }
                }
            }
            return false;
        }
    }
}
