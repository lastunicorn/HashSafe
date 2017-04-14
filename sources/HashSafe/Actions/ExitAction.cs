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
using DustInTheWind.ConsolePlus.CommandModel;

namespace DustInTheWind.HashSafe.Actions
{
    internal class ExitAction : IAction
    {
        private readonly ApplicationEnvironment applicationEnvironment;

        public string Description => "Exits the game.";

        public ExitAction(ApplicationEnvironment applicationEnvironment)
        {
            if (applicationEnvironment == null) throw new ArgumentNullException(nameof(applicationEnvironment));
            this.applicationEnvironment = applicationEnvironment;
        }

        public void Execute(params string[] parameters)
        {
            applicationEnvironment.RequestExit();
        }
    }
}