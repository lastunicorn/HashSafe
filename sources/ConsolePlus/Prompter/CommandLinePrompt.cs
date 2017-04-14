// HashSafe
// Copyright (C) 2017 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using DustInTheWind.ConsolePlus.CommandModel;

namespace DustInTheWind.ConsolePlus.Prompter
{
    public sealed class CommandLinePrompt
    {
        private readonly Display display;

        public IPrompterText PrompterText { get; set; }
        public ICommandSelector CommandSelector { get; set; }

        public CommandLinePrompt()
            : this(new DefaultCommandSelector(new ICommand[0]))
        {
        }

        public CommandLinePrompt(IEnumerable<ICommand> commands)
            : this(new DefaultCommandSelector(commands))
        {
        }

        public CommandLinePrompt(ICommandSelector commandSelector)
        {
            if (commandSelector == null) throw new ArgumentNullException(nameof(commandSelector));

            CommandSelector = commandSelector;

            display = new Display();
        }

        public event EventHandler<ActionExecutingEventArgs> ActionExecuting;
        public event EventHandler<ActionExecutedEventArgs> ActionExecuted;

        public void Display()
        {
            string commandText = null;

            while (string.IsNullOrEmpty(commandText))
                commandText = ReadCommand();

            ProcessCommand(commandText);
        }

        private string ReadCommand()
        {
            if (PrompterText == null)
                PrompterText = new StaticPrompterText();

            string text = PrompterText.ToString();

            return display.ReadCommand(text);
        }

        private void ProcessCommand(string commandText)
        {
            if (CommandSelector == null)
                throw new Exception("A CommandSelector is necessary to select the command to be executed.");

            CommandContext? commandContext = CommandSelector.SelectCommand(commandText);

            if (commandContext == null)
            {
                List<ICommand> similarActions = CommandSelector.FindSimilarCommands(commandText);

                if (similarActions.Count > 0)
                    display.DisplaySimilarActions(similarActions);
                else
                    display.DisplayInvalidCommandError();
            }
            else
            {
                ICommand command = commandContext.Value.Command;
                string[] parameters = commandContext.Value.Parameters;

                ExecuteCommand(command, parameters);
            }
        }

        private void ExecuteCommand(ICommand command, string[] parameters)
        {
            ActionExecutingEventArgs actionExecutingEventArgs = new ActionExecutingEventArgs(command);
            OnActionExecuting(actionExecutingEventArgs);

            command.Execute(parameters);

            ActionExecutedEventArgs actionExecutedEventArgs = new ActionExecutedEventArgs(command, parameters);
            OnActionExecuted(actionExecutedEventArgs);
        }

        private void OnActionExecuting(ActionExecutingEventArgs e)
        {
            ActionExecuting?.Invoke(this, e);
        }

        private void OnActionExecuted(ActionExecutedEventArgs e)
        {
            ActionExecuted?.Invoke(this, e);
        }
    }
}
