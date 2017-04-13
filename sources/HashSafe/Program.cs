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

using System;
using System.Security.Cryptography;

namespace DustInTheWind.HashSafe
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                using (MD5 md5 = MD5.Create())
                {
                    TargetsProvider targetsProvider = new TargetsProvider();
                    Display display = new Display();

                    Processor processor = new Processor(targetsProvider, md5, display);
                    processor.Execute();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            CustomConsole.Pause();
        }
    }
}
