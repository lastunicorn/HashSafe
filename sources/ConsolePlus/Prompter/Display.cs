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
using DustInTheWind.ConsolePlus.UI;

namespace DustInTheWind.ConsolePlus.Prompter
{
    internal class Display
    {
        public void DisplayInvalidCommandError()
        {
            CustomConsole.WriteLine("Invalid command");
            CustomConsole.WriteLine();
        }

        public void DisplaySimilarActions(IEnumerable<ICommand> similarActions)
        {
            IEnumerable<string> actionNames = similarActions
                .Select(x => x.Name);

            string actionNamesConcatenated = string.Join(", ", actionNames);
            CustomConsole.WriteLine($"Did you meen: {actionNamesConcatenated}");
            CustomConsole.WriteLine();
        }

        public string ReadCommand(string prompterText)
        {
            CustomConsole.WriteEmphasies(prompterText);
            return CustomConsole.ReadLine(ConsoleColor.Yellow);
        }
    }
}