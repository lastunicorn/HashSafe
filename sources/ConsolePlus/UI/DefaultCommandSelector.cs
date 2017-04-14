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
    public sealed class DefaultCommandSelector : ICommandSelector
    {
        private readonly IEnumerable<CommandBase> commands;

        public DefaultCommandSelector(IEnumerable<CommandBase> commands)
        {
            if (commands == null) throw new ArgumentNullException(nameof(commands));
            this.commands = commands;
        }

        public CommandContext? SelectCommand(string commandText)
        {
            return commands
                .Select(x => x.ParseAndCreateContext(commandText))
                .FirstOrDefault(x => x != null);
        }

        public List<CommandBase> FindSimilarCommands(string commandText)
        {
            return commands
                .Where(x => commandText.Trim().StartsWith(x.Name) || x.Name.StartsWith(commandText.Trim()))
                .ToList();
        }
    }
}