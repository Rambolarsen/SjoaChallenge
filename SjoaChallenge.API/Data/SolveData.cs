using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SjoaChallenge.API.Data
{
    public interface ISolveData
    {
        Task<bool> CheckAnswer(string currentTask, string phrase);
    }


    public class SolveData : ISolveData
    {
        private readonly IDictionary<string, string> _solutions = new Dictionary<string, string>()
        {
            { "Home/Oppgave1/", "Ut paa tur, aldri sur" },
            { "Home/Oppgave2/", "Det er gøy å sove i lavo" },
            { "Home/Oppgave3/", "sjoatur" },
        };
        
        public Task<bool> CheckAnswer(string currentTask, string phrase)
        {
            var valid = _solutions.TryGetValue(currentTask, out var answer);
            if (valid)
            {
                return Task.FromResult(string.Compare(answer, phrase, true) == 0);
            }
            return Task.FromResult(false);
        }
    }
}
