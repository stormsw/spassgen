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
    /// Predefined rule for capital alphabet characters.
    /// </summary>
    public class CapsAlpha : Chunk
    {
        public CapsAlpha ()
        {
            this.Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            this.Min = 1;
        }
    }

    public class LowAlpha : Chunk
    {
        public LowAlpha ()
        {
            Alpha = "abcdefghijklmnopqrstuvwxyz";
            Min = 1;
        }
    }

    public class Numerals : Chunk
    {
        public Numerals ()
        {
            Alpha = "0123456789";
            Min = 1;
        }
    }

    public class Spacers : Chunk
    {
        public Spacers ()
        {
            Alpha = " \t";
            Min = 1;
        }
    }

    public class SpecialChars : Chunk
    {
        public SpecialChars ()
        {
            Alpha = "~!@#$%^&*()_-=+{}[]:;\"'|\\/?.,";
            Min = 1;
        }
    }
}