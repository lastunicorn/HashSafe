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

namespace DustInTheWind.HashSafe.ActionModel
{
    internal abstract class ActionBase
    {
        public string Name
        {
            get { return Names.Count > 0 ? Names[0] : null; }
        }

        public List<string> Names { get; private set; }
        public abstract string Description { get; }
        public abstract List<string> Usage { get; }
        private List<Regex> matchers;

        private IEnumerable<Regex> Matchers
        {
            get
            {
                if (matchers == null)
                    matchers = CreateMatchers() ?? new List<Regex>();

                return matchers;
            }
        }

        protected ActionBase(params string[] names)
        {
            if (names == null) throw new ArgumentNullException("names");
            Names = new List<string>(names);
        }

        protected abstract List<Regex> CreateMatchers();

        public ActionInfo? Parse(string command)
        {
            string trimmedCommand = command.Trim();

            Match match = Matchers
                .Select(x => x.Match(trimmedCommand))
                .FirstOrDefault(x => x.Success);

            if (match == null)
                return null;

            return new ActionInfo
            {
                Action = this,
                Parameters = ExtractParameters(match)
            };
        }

        protected abstract string[] ExtractParameters(Match match);

        public abstract void Execute(params object[] parameters);
    }
}