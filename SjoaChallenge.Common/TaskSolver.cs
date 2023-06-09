namespace SjoaChallenge.Common
{
    public class TaskSolver
    {
        public TaskSolver(string username, string task)
        {
            Username = username;
            Task = task;
        }

        public string Username { get; private set; }

        public string Task { get; private set; } 
    }
}
