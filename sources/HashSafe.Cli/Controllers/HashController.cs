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
using System.Security.Cryptography;
using DustInTheWind.ConsoleTools.Mvc;

namespace DustInTheWind.HashSafe.Cli.Controllers
{
    internal class HashController : IController
    {
        private readonly TargetsProvider targetsProvider;
        private readonly Display display;

        public string Description => "Calculates the hashes for all the targets in the proj file.";

        public HashController(TargetsProvider targetsProvider, Display display)
        {
            if (targetsProvider == null) throw new ArgumentNullException(nameof(targetsProvider));
            if (display == null) throw new ArgumentNullException(nameof(display));

            this.targetsProvider = targetsProvider;
            this.display = display;
        }

        public void Execute()
        {
            using (MD5 md5 = MD5.Create())
            {
                Processor processor = new Processor(targetsProvider, md5);
                processor.TargetProcessed += HandleTargetProcessed;

                try
                {
                    processor.Execute();
                }
                finally
                {
                    processor.TargetProcessed -= HandleTargetProcessed;
                    display.Summary(processor.ElapsedTime);
                }
            }
        }

        private void HandleTargetProcessed(object sender, TargetProcessedEventArgs e)
        {
            if (e.Hash != null)
                display.DisplayFileHash(e.Target, e.Hash);
            else
                display.DisplayTargetNotFound(e.Target);
        }
    }
}