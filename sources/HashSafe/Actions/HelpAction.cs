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
using DustInTheWind.ConsolePlus.CommandModel;

namespace DustInTheWind.HashSafe.Actions
{
    internal class HelpAction : IAction
    {
        private readonly CommandSet commands;
        private readonly Display display;

        public string Description => "Displays details about an command. It may be a verb (that you use in the game, like \"look\", \"talk\", etc) or a command (to control the game, like \"menu\", \"save\", etc).";

        public HelpAction(Display display, CommandSet commands)
        {
            if (commands == null) throw new ArgumentNullException(nameof(commands));
            if (display == null) throw new ArgumentNullException(nameof(display));

            this.commands = commands;
            this.display = display;
        }

        public void Execute(params string[] parameters)
        {
            // todo: read multiple parameters

            string verbName = parameters.Length >= 1
                ? parameters[0]
                : null;

            Execute(verbName);
        }

        private void Execute(string verbName)
        {
            IEnumerable<ICommand> verbsToDisplay = commands
                .Where(x => x.Name == verbName);

            foreach (ICommand verb in verbsToDisplay)
            {
                display.DisplayInfo(verb.Name);
                display.DisplayInfo(verb.Description);

                foreach (string usage in verb.Usage)
                    display.DisplayInfo(usage);

                display.DisplayInfo("");
            }
        }
    }
}