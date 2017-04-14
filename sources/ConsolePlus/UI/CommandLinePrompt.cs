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
using DustInTheWind.ConsolePlus.ActionModel;

namespace DustInTheWind.ConsolePlus.UI
{
    public sealed class CommandLinePrompt
    {
        private readonly Display display;
        private readonly ActionSet actions;
        private readonly IPrompterText prompterText;

        public CommandLinePrompt(ActionSet actions)
            : this(actions, new PrompterText())
        {
        }

        public CommandLinePrompt(ActionSet actions, IPrompterText prompterText)
        {
            if (actions == null) throw new ArgumentNullException(nameof(actions));
            if (prompterText == null) throw new ArgumentNullException(nameof(prompterText));

            this.actions = actions;
            this.prompterText = prompterText;

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
            string text = prompterText.ToString();
            CustomConsole.WriteEmphasies(text);

            return CustomConsole.ReadAction();
        }

        private void ProcessCommand(string commandText)
        {
            ActionInfo? actionInfo = FindMatchingAction(commandText);

            if (actionInfo == null)
            {
                List<CommandBase> similarActions = FindSimilarCommands(commandText);

                if (similarActions.Count > 0)
                    display.DisplaySimilarActions(similarActions);
                else
                    display.DisplayInvalidCommandError();
            }
            else
            {
                CommandBase command = actionInfo.Value.Command;
                object[] parameters = actionInfo.Value.Parameters;

                ExecuteAction(command, parameters);
            }
        }

        private List<CommandBase> FindSimilarCommands(string commandText)
        {
            return actions
                .Where(x => commandText.Trim().StartsWith(x.Name) || x.Name.StartsWith(commandText.Trim()))
                .ToList();
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
