namespace SjoaChallenge
{
    public class Challenge
    {
        public Challenge(string html, string solution ) 
        {
            HTML = html;
            Solution = solution;
        }

        public string HTML { get; private set; }
        public string Solution { get; private set; }
    }
}
