//-----------------------------------------------------------------------
// <copyright file="PasswordBuilder.cs" company="Alexander Varchenko">
//     Copyright (c) Alexander Varchenko. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace com.ovarchenko.tools.generators.password
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Timers;

    public class PasswordBuilder
    {
        private PasswordProto proto;
        private HashSet<KeyValuePair<Func<bool>, Exception>> validators = new HashSet<KeyValuePair<Func<bool>, Exception>> ();

        public PasswordBuilder ()
        {
            proto = new PasswordProto ();

            AddValidator (IsValidLengthRestrictions, new ArgumentException (
                string.Format ("Minimal password length {0} doesn't fit summary charactersets minimal usage '{1}' in the password.",
                    MinLengthSetByRules, proto.Length)));
        }

        public PasswordBuilder (int maxLen)
            : this ()
        {
            proto.Length = maxLen;
        }

        public PasswordBuilder AddChunk (Chunk newChunk)
        {
            proto.AddRule (newChunk);
            return this;
        }

        public PasswordBuilder SetLength (int n)
        {
            proto.Length = n;
            return this;
        }

        public PasswordBuilder AddConditional<T> (bool condition, int min, int max) where T : Chunk, new()
        {
            if (condition) {
                var result = Chunk.GetObject<T> ();
                result.Min = min;
                result.Max = max;
                proto.AddRule (result);	
            }
            return this;
        }

        public PasswordBuilder AddConditional<T> (bool condition, int min) where T : Chunk, new()
        {
            if (condition) {
                var result = Chunk.GetObject<T> ();
                result.Min = min;
                proto.AddRule (result);	
            }
            return this;
        }

        public PasswordBuilder AddConditional<T> (bool condition) where T : Chunk, new()
        {
            if (condition) {
                proto.AddRule (Chunk.GetObject<T> ());	//or	(T)Activator.CreateInstance(typeof(T)));
            }
            return this;
        }

        public int MinLengthSetByRules {
            get {
                return proto.Rules.Aggregate (0, (current, sequence) => current + sequence.Min);
            }
        }

        /// <summary>
        /// Validates all minimal char accurance for all defined retrictions fit minimum password length limit
        /// </summary>
        /// <remarks>
        /// It assumes all character rules minimal usage data is summaraized and checked against the  password length
        /// It is FALSE in case rules requires minimum charcters count greater than password length set
        /// </remarks>
        public bool IsValidLengthRestrictions ()
        {
            return MinLengthSetByRules < proto.Length;
        }

        private PasswordBuilder AddValidator (Func<bool> predicate, SystemException ex)
        {
            validators.Add (new KeyValuePair<Func<bool>, Exception> (predicate, ex));
            return this;
        }

        private void Validate ()
        {
            try {
                validators.AsParallel ().ForAll (item => {
                    if (!item.Key ())
                        throw item.Value;
                }
                );
            }
			// In this design, we stop query processing when the exception occurs. 
			catch (AggregateException) {
                // TODO: add password builder validator exception class and init with AggregateException info
                throw;
            }
        }

        private static char GetRandomCharacter (string text, Random rng)
        {
            int index = rng.Next (text.Length);
            return text [index];
        }

        public string Generate ()
        {
            Validate ();

            List<Char> charlist = new List<char> ();
            List<Char> altlist = new List<char> ();
            /// we have to generate <passwordlenth> characters = [MinLengthByRules+take] 
            int take = proto.Length - MinLengthSetByRules;
            // at the same rule we may generate in advance up to max usage alternate chars
            int left_generate = proto.Length;
            var randomSource = new Random (Guid.NewGuid ().GetHashCode ());
            foreach (var rule in proto.Rules) {
                var minusage = rule.Min;
                var maxusage = rule.Max > 0 ? rule.Max : int.MaxValue;

                while (minusage-- > 0) {
                    left_generate--;
                    charlist.Add (GetRandomCharacter (rule.Alpha, randomSource));
                    if (maxusage-- > 0) {
                        left_generate--;
                        altlist.Add (GetRandomCharacter (rule.Alpha, randomSource));
                    }
                }								
            }

            while (left_generate-- > 0) {
                //get random rule				
                altlist.Add (GetRandomCharacter (proto.RandomRule (randomSource).Alpha, randomSource));
            }

            StringBuilder result = new StringBuilder ();
            //we have to include all minimum required chars	first and then get random set 
            //alternative sort (r.Next(2) % 2) == 0
            charlist.AddRange (altlist.OrderBy (x => Guid.NewGuid ()).Take (take));
            // shuffle mandatory chars and alternatives
            result.Append (charlist.OrderBy (x => Guid.NewGuid ()).ToArray ());		
            return result.ToString ();
        }
    }
}