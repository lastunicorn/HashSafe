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
using System.Threading;
using DustInTheWind.HashSafe.ActionModel;
using DustInTheWind.HashSafe.Actions;
using DustInTheWind.HashSafe.Commands;
using DustInTheWind.HashSafe.UI;

namespace DustInTheWind.HashSafe
{
    internal static class Program
    {
        private static bool exitWasRequested;

        private static void Main(string[] args)
        {
            try
            {
                Display display = new Display();
                TargetsProvider targetsProvider = new TargetsProvider();

                ActionSet actionSet = new ActionSet();

                HelpAction helpAction = new HelpAction(display, actionSet);
                actionSet.Add(new HelpCommand(helpAction));

                ExitAction exitAction = new ExitAction();
                actionSet.Add(new ExitCommand(exitAction));
                actionSet.Add(new QuitCommand(exitAction));
                actionSet.Add(new XCommand(exitAction));

                HashAction hashAction = new HashAction(targetsProvider, display);
                actionSet.Add(new HashCommand(hashAction));

                CommandLinePrompt commandLinePrompt = new CommandLinePrompt(display, actionSet);

                while (!exitWasRequested)
                {
                    commandLinePrompt.Display();
                }

                display.DisplayInfo("Bye!");
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                CustomConsole.WriteError("Fatal error");
                CustomConsole.WriteError(ex);

                CustomConsole.Pause();
            }
        }

        public static void RequestExit()
        {
            exitWasRequested = true;
        }
    }
}
