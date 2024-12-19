namespace Binary
{
    internal class DFS
    {
        private GameBoard gameBoard;
        private HashSet<GameBoard> visited = new HashSet<GameBoard>();
        private Stack<GameBoard> stack = new Stack<GameBoard>();
        public DFS(GameBoard gameBoard)
        {
            this.gameBoard = new GameBoard(gameBoard);
        }

        public bool Run()
        {
            stack.Push(gameBoard);
            int generate = 1;

            while (stack.Count > 0)
            {
                GameBoard current = stack.Pop();

                if (current.IsGoalState())
                {
                    Game.GoalStateOptions(current,generate,visited.Count,"DFS");
                    return true;
                }
                visited.Add(current);
                foreach (GameBoard child in current.GetPossibleStates()) 
                {
                    if (!visited.Contains(child))
                    {
                        stack.Push(child);
                        generate++;
                    }
                }
            }
            return false;
        }
    }
}
