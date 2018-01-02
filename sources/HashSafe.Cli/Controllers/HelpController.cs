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
using DustInTheWind.ConsoleTools.Mvc;

namespace DustInTheWind.HashSafe.Cli.Controllers
{
    internal class HelpController : IController
    {
        private readonly Display display;

        public string Description => "Displays details about an command. It may be a verb (that you use in the game, like \"look\", \"talk\", etc) or a command (to control the game, like \"menu\", \"save\", etc).";

        public HelpController(Display display)
        {
            if (display == null) throw new ArgumentNullException(nameof(display));
            
            this.display = display;
        }

        public void Execute()
        {
        }
    }
}