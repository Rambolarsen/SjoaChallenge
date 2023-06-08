using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SjoaChallenge.API.Data;
using System.Collections.Generic;

namespace SjoaChallenge.API.Functions
{
    public class SolveGet
    {
        private readonly ISolveData _solveData;
        private const string CurrentTask = "currenttask";
        private const string Phrase = "phrase";

        public SolveGet(ISolveData solveData)
        {
            _solveData = solveData;
        }

        [FunctionName("SolveGet")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "solve")] HttpRequest req)
        {
            IDictionary<string, string> queryParams = req.GetQueryParameterDictionary();
            var solved = await _solveData.CheckAnswer(queryParams[CurrentTask], queryParams[Phrase]);
            return new OkObjectResult(solved);
        }
    }
}
