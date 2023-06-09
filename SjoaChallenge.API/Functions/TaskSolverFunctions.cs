using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using SjoaChallenge.API.Data;
using System.Collections.Generic;
using System.Text.Json;
using SjoaChallenge.Common;

namespace SjoaChallenge.API.Functions
{
    public class TaskSolverGet
    {
        private readonly ITaskSolverData _taskSolverData;
        private const string CurrentTask = "currenttask";
        private const string Username = "username";

        public TaskSolverGet(ITaskSolverData taskSolverData)
        {
            _taskSolverData = taskSolverData;
        }

        [FunctionName("TaskSolverGet")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasksolver")] HttpRequest req)
        {
            IDictionary<string, string> queryParams = req.GetQueryParameterDictionary();
            var taskSolver = new TaskSolver(queryParams[Username], queryParams[CurrentTask]);
            var isSolved = await _taskSolverData.IsSolved(taskSolver);
            return new OkObjectResult(isSolved);
        }
    }

    public class TaskSolverPost
    {
        private readonly ITaskSolverData _taskSolverData;
        private const string CurrentTask = "currenttask";
        private const string Username = "username";

        public TaskSolverPost(ITaskSolverData taskSolverData)
        {
            _taskSolverData = taskSolverData;
        }

        [FunctionName("TaskSolverPost")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tasksolver")] HttpRequest req)
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var taskSolver = JsonSerializer.Deserialize<TaskSolver>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await _taskSolverData.SolveTask(taskSolver);
            return new OkResult();
        }
    }
}
