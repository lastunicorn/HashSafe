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
using DustInTheWind.HashSafe.Actions;

namespace DustInTheWind.HashSafe.UI
{
    internal class CommandLinePrompt
    {
        private readonly Display display;
        private readonly ActionSet actions;

        public CommandLinePrompt(Display display, ActionSet actions)
        {
            if (display == null) throw new ArgumentNullException(nameof(display));
            if (actions == null) throw new ArgumentNullException(nameof(actions));

            this.display = display;
            this.actions = actions;
        }

        public event EventHandler<ActionExecutingEventArgs> ActionExecuting;
        public event EventHandler<ActionExecutedEventArgs> ActionExecuted;

        public void Display()
        {
            string command = null;

            while (string.IsNullOrEmpty(command))
                command = ReadCommand();

            ProcessCommand(command);
        }

        private string ReadCommand()
        {
            CustomConsole.WriteEmphasies("HashSafe> ");
            return CustomConsole.ReadAction();
        }

        private void ProcessCommand(string command)
        {
            ActionBase action;
            object[] parameters;

            ActionInfo? actionInfo = FindMatchingAction(command);

            if (actionInfo == null)
            {
                List<ActionBase> similarActions = actions
                    .Where(x => x.Names.Any(z => command.TrimStart().StartsWith(z)))
                    .ToList();

                if (similarActions.Count > 0)
                {
                    action = actions.First(x => x is HelpAction);
                    parameters = similarActions.Select(x => (object)x.Name).ToArray();
                }
                else
                {
                    display.DisplayInfo("Invalid command");
                    return;
                }
            }
            else
            {
                action = actionInfo.Value.Action;
                parameters = actionInfo.Value.Parameters;
            }

            ExecuteAction(action, parameters);
        }

        private void ExecuteAction(ActionBase action, object[] parameters)
        {
            ActionExecutingEventArgs actionExecutingEventArgs = new ActionExecutingEventArgs(action);
            OnActionExecuting(actionExecutingEventArgs);

            action.Execute(parameters);

            ActionExecutedEventArgs actionExecutedEventArgs = new ActionExecutedEventArgs(action, parameters);
            OnActionExecuted(actionExecutedEventArgs);
        }

        private ActionInfo? FindMatchingAction(string command)
        {
            return actions
                .Select(x => x.Parse(command))
                .FirstOrDefault(x => x != null);
        }

        protected virtual void OnActionExecuting(ActionExecutingEventArgs e)
        {
            ActionExecuting?.Invoke(this, e);
        }

        protected virtual void OnActionExecuted(ActionExecutedEventArgs e)
        {
            ActionExecuted?.Invoke(this, e);
        }
    }
}
