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
using System.Text.RegularExpressions;

namespace DustInTheWind.ConsolePlus.ActionModel
{
    public abstract class CommandBase
    {
        private readonly IAction action;
        public string Name { get; }
        public string Description => action.Description;
        public abstract IEnumerable<string> Usage { get; }
        private List<Regex> matchers;
        
        protected CommandBase(string name, IAction action)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (action == null) throw new ArgumentNullException(nameof(action));

            Name = name;
            this.action = action;
        }

        protected abstract List<Regex> CreateMatchers();

        public CommandContext? ParseAndCreateContext(string command)
        {
            string trimmedCommand = command.Trim();

            EnsureMatchers();

            Match match = matchers
                .Select(x => x.Match(trimmedCommand))
                .FirstOrDefault(x => x.Success);

            if (match == null)
                return null;

            return new CommandContext
            {
                Command = this,
                Parameters = ExtractParameters(match)
            };
        }

        private void EnsureMatchers()
        {
            if (matchers == null)
                matchers = CreateMatchers() ?? new List<Regex>();
        }

        protected abstract string[] ExtractParameters(Match match);

        public void Execute(params object[] parameters)
        {
            action.Execute(parameters);
        }
    }
}