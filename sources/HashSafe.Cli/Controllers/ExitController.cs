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
using DustInTheWind.ConsoleTools.CommandProviders;
using DustInTheWind.ConsoleTools.Mvc;

namespace DustInTheWind.HashSafe.Cli.Controllers
{
    internal class ExitController : IController
    {
        private readonly ConsoleApplication consoleApplication;

        public string Description => "Exits the game.";

        public ExitController(ConsoleApplication consoleApplication)
        {
            if (consoleApplication == null) throw new ArgumentNullException(nameof(consoleApplication));
            this.consoleApplication = consoleApplication;
        }

        public void Execute(IReadOnlyCollection<UserCommandParameter> parameters)
        {
            consoleApplication.Exit();
        }
    }
}