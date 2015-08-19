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
    /// Numerals rule.
    /// </summary>
    public class Numerals 
        : Chunk
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="com.ovarchenko.tools.generators.password.Numerals"/> class.
        /// </summary>
        public Numerals()
        {
            this.Alpha = "0123456789";
            this.Min = 1;
        }
    }
    
}