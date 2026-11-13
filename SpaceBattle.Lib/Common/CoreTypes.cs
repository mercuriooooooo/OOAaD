namespace SpaceBattle.Lib;

public class CommandException : Exception
{
    public CommandException(string message) : base(message) { }
}
