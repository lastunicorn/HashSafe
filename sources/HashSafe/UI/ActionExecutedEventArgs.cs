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
using DustInTheWind.HashSafe.ActionModel;

namespace DustInTheWind.HashSafe.UI
{
    internal class ActionExecutedEventArgs : EventArgs
    {
        public ActionBase Action { get; private set; }
        public object[] Parameters { get; private set; }

        public ActionExecutedEventArgs(ActionBase action, object[] parameters)
        {
            if (action == null) throw new ArgumentNullException("action");
            if (parameters == null) throw new ArgumentNullException("parameters");

            Action = action;
            Parameters = parameters;
        }
    }
}