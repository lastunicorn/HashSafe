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
using DustInTheWind.HashSafe.ActionModel;
using DustInTheWind.HashSafe.UI;

namespace DustInTheWind.HashSafe.Actions
{
    internal class HelpAction : ActionBase
    {
        private readonly ActionSet actions;
        private readonly Display display;

        public override string Description
        {
            get { return "Displays details about an action. It may be a verb (that you use in the game, like \"look\", \"talk\", etc) or a command (to control the game, like \"menu\", \"save\", etc)."; }
        }

        public override List<string> Usage
        {
            get { return new List<string> { "<<help>> <<<verb> >>", "<<help>> <<<command> >>" }; }
        }

        public HelpAction(Display display, ActionSet actions)
            : base("help")
        {
            if (actions == null) throw new ArgumentNullException(nameof(actions));
            if (display == null) throw new ArgumentNullException(nameof(display));

            this.actions = actions;
            this.display = display;
        }

        protected override List<Regex> CreateMatchers()
        {
            return new List<Regex>
            {
                new Regex(@"^\s*help\s*(\s(?'verb'.+))\s*$", RegexOptions.IgnoreCase | RegexOptions.Singleline)
            };
        }

        protected override string[] ExtractParameters(Match match)
        {
            return new[]
            {
                match.Groups["verb"].Value
            };
        }

        public override void Execute(params object[] parameters)
        {
            // todo: read multiple parameters

            string verbName = parameters.Length >= 1
                ? parameters[0] as string
                : null;

            Execute(verbName);
        }

        private void Execute(string verbName)
        {
            IEnumerable<ActionBase> verbsToDisplay = actions
                .Where(x => x.Names.Contains(verbName));

            foreach (ActionBase verb in verbsToDisplay)
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