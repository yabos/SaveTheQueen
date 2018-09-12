namespace Lib.Pattern
{
    public interface ICommand
    {
        void Redo();
        void Undo();

        bool Execute();

        string Name { get; }
        ICommand Clone();
    }
}