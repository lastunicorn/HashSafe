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
using Ninject;

namespace DustInTheWind.HashSafe.Cli
{
    /// <summary>
    /// Adapter class to expose the Ninject kernel (<see cref="T:Ninject.IKernel" />)
    /// as an <see cref="T:System.IServiceProvider" />.
    /// </summary>
    internal class NinjectServiceProvider : IServiceProvider
    {
        private readonly IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectServiceProvider"/> class with
        /// the underlying Ninject kernel.
        /// </summary>
        /// <param name="kernel"></param>
        public NinjectServiceProvider(IKernel kernel)
        {
            if (kernel == null) throw new ArgumentNullException(nameof(kernel));
            this.kernel = kernel;
        }

        /// <summary>
        /// Returns an instance of the specified type.
        /// </summary>
        public object GetService(Type serviceType)
        {
            return kernel.Get(serviceType);
        }
    }
}