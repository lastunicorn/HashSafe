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

using System.Collections.Generic;

namespace DustInTheWind.ConsolePlus.CommandModel
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        IEnumerable<string> Usage { get; }

        /// <summary>
        /// Tries to parse the request and, if succeeded, a new context <see cref="CommandContext"/> is returned,
        /// filled with all the necessary data to execute the command.
        /// If the request cannot be parsed, <c>null</c> is returned.
        /// </summary>
        /// <param name="commandText">The request that has to be handled.</param>
        /// <returns>A new instance of <see cref="CommandContext"/> if the request can be handled by the current instance
        /// or <c>null</c> if the request cannot be handled.</returns>
        CommandContext? ParseAndCreateContext(string commandText);

        /// <summary>
        /// Executes the associated action.
        /// </summary>
        /// <param name="parameters">A list of parameters necessary for the execution of the action.</param>
        void Execute(params string[] parameters);
    }
}