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
using DustInTheWind.ConsolePlus.Prompter;
using DustInTheWind.ConsolePlus.UI;
using DustInTheWind.HashSafe.Actions;
using DustInTheWind.HashSafe.Commands;

namespace DustInTheWind.HashSafe
{
    internal class ApplicationEnvironment
    {
        private Display display;
        private TargetsProvider targetsProvider;

        private bool exitWasRequested;

        public void Run()
        {
            try
            {
                display = new Display();
                targetsProvider = new TargetsProvider();

                CommandSet commands = CreateCommands();

                CommandLinePrompt commandLinePrompt = new CommandLinePrompt(commands)
                {
                    PrompterText = new StaticPrompterText { Text = "HashSafe> " }
                };

                while (!exitWasRequested)
                    commandLinePrompt.Display();

                display.DisplayInfo("Bye!");
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                CustomConsole.WriteError("Fatal error");
                CustomConsole.WriteError(ex);

                CustomConsole.Pause();
            }
        }

        private CommandSet CreateCommands()
        {
            CommandSet commands = new CommandSet();

            HelpAction helpAction = new HelpAction(display, commands);
            commands.Add(new HelpCommand(helpAction));

            ExitAction exitAction = new ExitAction(this);
            commands.Add(new ExitCommand(exitAction));
            commands.Add(new QuitCommand(exitAction));
            commands.Add(new XCommand(exitAction));

            HashAction hashAction = new HashAction(targetsProvider, display);
            commands.Add(new HashCommand(hashAction));

            return commands;
        }

        public void RequestExit()
        {
            exitWasRequested = true;
        }
    }
}