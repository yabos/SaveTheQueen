using System.Collections.Generic;

namespace Lib.Pattern
{
    public class CommandManager
    {
        public delegate void UpdateHistoryEventHandler();

        public event UpdateHistoryEventHandler UpdateHistoryEvent = null;

        private ICommand m_currentCommand;

        private int m_maxCommand = 100;
        private readonly Stack<ICommand> m_redoCommands = new Stack<ICommand>();
        private readonly LinkedList<ICommand> m_undoCommands = new LinkedList<ICommand>();

        public int MaxCommand
        {
            get { return m_maxCommand; }
            set { m_maxCommand = value; }
        }

        public ICommand CurrentCommand
        {
            get { return m_currentCommand; }
            set { m_currentCommand = value; }
        }

        public LinkedList<ICommand> UndoCommands
        {
            get { return m_undoCommands; }
        }

        public Stack<ICommand> RedoCommands
        {
            get { return m_redoCommands; }
        }

        public void Reset()
        {
            UndoCommands.Clear();
            RedoCommands.Clear();

            UpdateHistory();
        }

        private void Overflow()
        {
            if (UndoCommands.Count <= MaxCommand)
            {
                return;
            }
            UndoCommands.RemoveLast();
        }

        private void Add()
        {
            if (m_currentCommand == null) return;
            RedoCommands.Clear();
            UndoCommands.AddFirst(m_currentCommand.Clone());

            Overflow();
            UpdateHistory();
        }

        public ICommand Undo()
        {
            if (UndoCommands.Count == 0) return null;

            ICommand command = UndoCommands.First.Value;
            command.Undo();

            RedoCommands.Push(command);
            UndoCommands.RemoveFirst();

            UpdateHistory();

            return command;
        }

        public ICommand Redo()
        {
            if (RedoCommands.Count == 0) return null;

            ICommand command = RedoCommands.Pop();
            UndoCommands.AddFirst(command);

            command.Redo();

            UpdateHistory();
            return command;
        }

        public bool Execute()
        {
            if (m_currentCommand == null) return false;
            if (m_currentCommand.Execute())
            {
                Add();
                return true;
            }
            else
            {
                m_currentCommand = null;
                return false;
            }
        }

        private void UpdateHistory()
        {
            if (UpdateHistoryEvent != null)
                UpdateHistoryEvent();
        }
    }
}