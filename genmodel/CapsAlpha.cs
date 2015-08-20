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
    /// Predefined rule for capital alphabet characters.
    /// </summary>
    public class CapsAlpha 
        : LowAlpha
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="com.ovarchenko.tools.generators.password.CapsAlpha"/> class.
        /// </summary>
        public CapsAlpha ()
        {
            this.Alpha = this.Alpha.ToUpper();// = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            this.Min = 1;
        }
    }

}