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
using System.Threading;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Mvc;
using DustInTheWind.HashSafe.Cli.Controllers;
using Ninject;

namespace DustInTheWind.HashSafe.Cli
{
    internal class ApplicationEnvironment
    {
        private readonly ConsoleApplication consoleApplication;

        public ApplicationEnvironment()
        {
            IKernel kernel = CreateAndConfigureKernel();

            consoleApplication = kernel.Get<ConsoleApplication>();

            consoleApplication.ConfigureRoutes(new List<Route>
            {
                new Route("help", typeof(HelpController)),
                new Route("hash", typeof(HashController)),
                new Route("exit", typeof(ExitController)),
            });

            consoleApplication.ServiceProvider = new NinjectServiceProvider(kernel);
        }

        private static IKernel CreateAndConfigureKernel()
        {
            IKernel kernel = new StandardKernel();

            kernel.Bind<ConsoleApplication>().To<ConsoleApplication>().InSingletonScope();
            kernel.Bind<Display>().To<Display>().InSingletonScope();
            kernel.Bind<TargetsProvider>().To<TargetsProvider>().InSingletonScope();

            return kernel;
        }

        public void Run()
        {
            try
            {
                consoleApplication.Run();
                
                CustomConsole.WriteLine("Bye!");
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                CustomConsole.WriteError("Fatal error");
                CustomConsole.WriteError(ex);

                CustomConsole.Pause();
            }
        }
    }
}