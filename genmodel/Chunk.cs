//-----------------------------------------------------------------------
// <copyright file="Chunk.cs" company="Alexander Varchenko">
//     Copyright (c) Alexander Varchenko. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace com.ovarchenko.tools.generators.password
{
    using System;

    /// <summary>
    /// Chunk encapsulates single password rule
    /// </summary>
    public class Chunk
    {
        /// <summary>
        /// character set
        /// </summary>
        private string alpha;

        /// <summary>
        /// maximum appearance (0 - unlimited)
        /// </summary>
        private int max;

        /// <summary>
        /// minimal appearance (0 - unlimited)
        /// </summary>
        private int min;

        /// <summary>
        /// Gets or sets the alpha.
        /// </summary>
        /// <value>The alpha.</value>
        public string Alpha 
        {
            get { return this.alpha; }
            set { this.alpha = value; }
        }
            
        /// <summary>
        /// Gets or sets the max.
        /// </summary>
        /// <value>The max.</value>
        [Obsolete("At usual we dont have to restrict max characters for password. It wil be removed.", false)]
        public int Max 
        {
            get { return this.max; }
            set { this.max = value; }
        }

        /// <summary>
        /// Gets or sets the minimum.
        /// </summary>
        /// <value>The minimum.</value>
        public int Min 
        {
            get { return this.min; }
            set { this.min = value; }
        }

        /// <summary>
        /// Gets the object instance.
        /// </summary>
        /// <returns>The object.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static Chunk GetObject<T>() 
            where T : Chunk, new()
        {
            return new T();
        }
    }
}