//-----------------------------------------------------------------------
// <copyright file="Generator.cs" company="Alexander Varchenko">
//     Copyright (c) Alexander Varchenko. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace com.ovarchenko.tools.generators.password
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Spacers characters rule.
    /// </summary>
    public class Spacers 
        : Chunk
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="com.ovarchenko.tools.generators.password.Spacers"/> class.
        /// </summary>
        public Spacers()
        {
            this.Alpha = " \t";
            this.Min = 1;
        }
    }
    
}