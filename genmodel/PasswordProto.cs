//-----------------------------------------------------------------------
// <copyright file="PasswordProto.cs" company="Alexander Varchenko">
//     Copyright (c) Alexander Varchenko. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace com.ovarchenko.tools.generators.password
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.Concurrent;


    /// <summary>
    /// Password configuration class for the Builder
    /// </summary>
    public class PasswordProto
    {
        private int length;

        /// <summary>
        /// Password length
        /// </summary>
        public int Length {
            get { return length; }
            set { length = value; }
        }

        //private List<Chunk> rules;
        private ConcurrentBag<Chunk> rules;

        public PasswordProto ()
        {
            //rules = new List<Chunk>();
            rules = new ConcurrentBag<Chunk> ();
        }

        /// <summary>
        /// Get list of set up rules
        /// </summary>
        public IEnumerable<Chunk> Rules {
            get { return rules; }
        }

        /// <summary>
        /// Add new rule description
        /// </summary>
        /// <param name="newChunk"></param>
        public void AddRule (Chunk newChunk)
        {
            rules.Add (newChunk);
        }

        public Chunk RandomRule (Random r)
        {
            Chunk result;
            if (rules.Count > 1) {
                result = rules.ToArray () [r.Next (rules.Count - 1)];				
            } else
                result = rules.First ();
            return result;
        }
    }
}