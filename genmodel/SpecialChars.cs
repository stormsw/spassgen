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
    /// Special chars.
    /// </summary>
    public class SpecialChars 
        : Chunk
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="com.ovarchenko.tools.generators.password.SpecialChars"/> class.
        /// </summary>
        public SpecialChars()
        {
            this.Alpha = "~!@#$%^&*()_-=+{}[]:;\"'|\\/?.,";
            this.Min = 1;
        }
    }
}