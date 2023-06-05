namespace Terminal
{
    public interface ICommand
    {
        string CommandName { get; }
        string CommandType { get; }
        string CommandTypeDescription { get; }
        string CommandDescription { get; }

    }
}
