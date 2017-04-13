﻿// HashSafe
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
using System.Text.RegularExpressions;
using DustInTheWind.HashSafe.ActionModel;

namespace DustInTheWind.HashSafe.Actions
{
    internal class ExitAction : ActionBase
    {
        public ExitAction()
            : base("exit", "quit", "x")
        {
        }

        public override string Description
        {
            get { return "Exits the game."; }
        }

        public override List<string> Usage
        {
            get { return new List<string> { "<<exit>>", "<<quit>>", "<<x>>" }; }
        }

        protected override List<Regex> CreateMatchers()
        {
            return new List<Regex>
            {
                new Regex(@"^\s*(exit|quit|x)\s*$", RegexOptions.IgnoreCase | RegexOptions.Singleline)
            };
        }

        protected override string[] ExtractParameters(Match match)
        {
            return new string[0];
        }

        public override void Execute(params object[] parameters)
        {
            Program.RequestExit();
        }
    }
}