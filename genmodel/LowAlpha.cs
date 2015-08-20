//-----------------------------------------------------------------------
// <copyright file="Generator.cs" company="Alexander Varchenko">
//     Copyright (c) Alexander Varchenko. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace OV.Tools.Generators.Password
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Low alpha.
    /// </summary>
    public class LowAlpha 
        : Chunk
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="com.ovarchenko.tools.generators.password.LowAlpha"/> class.
        /// </summary>
        public LowAlpha()
        {
            this.Alpha = "abcdefghijklmnopqrstuvwxyz";
            this.Min = 1;
        }
    }
    
}