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
using System.Linq;
using DustInTheWind.HashSafe.ActionModel;
using DustInTheWind.HashSafe.Commands;

namespace DustInTheWind.HashSafe.UI
{
    internal sealed class CommandLinePrompt
    {
        private readonly Display display;
        private readonly ActionSet actions;
        private readonly IPrompterText prompterText;

        public CommandLinePrompt(Display display, ActionSet actions)
            : this(display, actions, new PrompterText())
        {
        }

        public CommandLinePrompt(Display display, ActionSet actions, IPrompterText prompterText)
        {
            if (display == null) throw new ArgumentNullException(nameof(display));
            if (actions == null) throw new ArgumentNullException(nameof(actions));
            if (prompterText == null) throw new ArgumentNullException(nameof(prompterText));

            this.display = display;
            this.actions = actions;
            this.prompterText = prompterText;
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
            string text = prompterText.ToString();
            CustomConsole.WriteEmphasies(text);

            return CustomConsole.ReadAction();
        }

        private void ProcessCommand(string commandText)
        {
            CommandBase command;
            object[] parameters;

            ActionInfo? actionInfo = FindMatchingAction(commandText);

            if (actionInfo == null)
            {
                List<CommandBase> similarActions = actions
                    .Where(x => commandText.TrimStart().StartsWith(x.Name))
                    .ToList();

                if (similarActions.Count > 0)
                {
                    command = actions.First(x => x is HelpCommand);
                    parameters = similarActions.Select(x => (object)x.Name).ToArray();
                }
                else
                {
                    display.DisplayInvalidCommandError();
                    return;
                }
            }
            else
            {
                command = actionInfo.Value.Command;
                parameters = actionInfo.Value.Parameters;
            }

            ExecuteAction(command, parameters);
        }

        private void ExecuteAction(CommandBase command, object[] parameters)
        {
            ActionExecutingEventArgs actionExecutingEventArgs = new ActionExecutingEventArgs(command);
            OnActionExecuting(actionExecutingEventArgs);

            command.Execute(parameters);

            ActionExecutedEventArgs actionExecutedEventArgs = new ActionExecutedEventArgs(command, parameters);
            OnActionExecuted(actionExecutedEventArgs);
        }

        private ActionInfo? FindMatchingAction(string command)
        {
            return actions
                .Select(x => x.Parse(command))
                .FirstOrDefault(x => x != null);
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
