using System.Text;

namespace Terminal
{
    public interface ITerminal
    {
        string ParseInput(string rawInput);

        ICollection<string> Output();
    }
}