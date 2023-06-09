using SjoaChallenge.Common;
using SjoaChallenge.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SjoaChallenge.API.Data
{
    public interface ITaskSolverData
    {
        Task SolveTask(TaskSolver taskSolver);
        Task<bool> IsSolved(TaskSolver taskSolver);
    }
    public class TaskSolverData : ITaskSolverData
    {
        private readonly ICollection<TaskSolver> _taskSolvers = new List<TaskSolver>();

        public Task<bool> IsSolved(TaskSolver taskSolver) => 
            Task.FromResult(_taskSolvers.Any(x => x.Username.EqualsIgnoreCase(taskSolver.Username) && x.Task.EqualsIgnoreCase(taskSolver.Task)));

        public async Task SolveTask(TaskSolver taskSolver)
        {
            if (!(await IsSolved(taskSolver)))
                _taskSolvers.Add((taskSolver));
            
            //TODO: add functions, for this and leaderboard update, add to servicecollection
        }
    }
}
