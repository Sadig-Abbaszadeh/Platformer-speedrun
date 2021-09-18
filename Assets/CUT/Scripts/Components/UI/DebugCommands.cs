using System;

namespace DartsGames
{
    public class DebugCommandBase
    {
        private string commandID, commandDescription, commandFormat;

        public string CommandID => commandID;
        public string CommandDescription => commandDescription;
        public string CommandFormat => commandFormat;

        // ctor
        public DebugCommandBase(string commandID, string commandDescription, string commandFormat)
        {
            this.commandID = commandID;
            this.commandDescription = commandDescription;
            this.commandFormat = commandFormat;
        }
    }

    public class DebugCommand : DebugCommandBase
    {
        private Action action;

        public DebugCommand(string commandID, string commandDescription, string commandFormat, Action action) : base(commandID, commandDescription, commandFormat)
        {
            this.action = action;
        }

        public void Invoke() => action();
    }

    public class DebugCommand<T> : DebugCommandBase
    {
        Action<T> action;

        public DebugCommand(string commandID, string commandDescription, string commandFormat, Action<T> action) : base(commandID, commandDescription, commandFormat)
        {
            this.action = action;
        }

        public void Invoke(T param) => action(param);
    }
}