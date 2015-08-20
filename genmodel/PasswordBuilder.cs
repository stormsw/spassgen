//-----------------------------------------------------------------------
// <copyright file="PasswordBuilder.cs" company="Alexander Varchenko">
//     Copyright (c) Alexander Varchenko. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace OV.Tools.Generators.Password
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Timers;

    /// <summary>
    /// Password builder.
    /// </summary>
    public class PasswordBuilder
    {
        /// <summary>
        /// The proto.
        /// </summary>
        private PasswordProto proto;

        /// <summary>
        /// The validators.
        /// </summary>
        private HashSet<KeyValuePair<Func<bool>, Exception>> validators = new HashSet<KeyValuePair<Func<bool>, Exception>> ();

        /// <summary>
        /// Gets the minimum length set by rules.
        /// </summary>
        /// <value>The minimum length set by rules.</value>
        public int MinLengthSetByRules
        {
            get 
            {
                return this.proto.Rules.Aggregate(0, (current, sequence) => current + sequence.Min);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="com.ovarchenko.tools.generators.password.PasswordBuilder"/> class.
        /// </summary>
        public PasswordBuilder()
        {
            this.proto = new PasswordProto();

            this.AddValidator(IsValidLengthRestrictions, new ArgumentException (
                string.Format("Minimal password length {0} doesn't fit summary charactersets minimal usage '{1}' in the password.",
                    this.MinLengthSetByRules, this.proto.Length)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="com.ovarchenko.tools.generators.password.PasswordBuilder"/> class.
        /// </summary>
        /// <param name="maxLen">Max length.</param>
        public PasswordBuilder(int maxLen)
            : this()
        {
            this.proto.Length = maxLen;
        }

        /// <summary>
        /// Adds the chunk.
        /// </summary>
        /// <returns>The chunk.</returns>
        /// <param name="newChunk">New chunk.</param>
        public PasswordBuilder AddChunk(Chunk newChunk)
        {
            this.proto.AddRule(newChunk);
            return this;
        }

        /// <summary>
        /// Sets the length.
        /// </summary>
        /// <returns>The length.</returns>
        /// <param name="n">N.</param>
        public PasswordBuilder SetLength(int n)
        {
            this.proto.Length = n;
            return this;
        }

        /// <summary>
        /// Adds the conditional.
        /// </summary>
        /// <returns>The conditional.</returns>
        /// <param name="condition">If set to <c>true</c> condition.</param>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public PasswordBuilder AddConditional<T>(bool condition, int min, int max) 
            where T : Chunk, new()
        {
            if (condition) 
            {
                var result = Chunk.GetObject<T>();
                result.Min = min;
                result.Max = max;
                this.proto.AddRule(result);	
            }

            return this;
        }

        /// <summary>
        /// Adds the conditional.
        /// </summary>
        /// <returns>Self for fluent syntax support</returns>
        /// <param name="condition">If set to <c>true</c> condition.</param>
        /// <param name="min">Minimum.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public PasswordBuilder AddConditional<T>(bool condition, int min) 
            where T : Chunk, new()
        {
            if (condition) 
            {
                var result = Chunk.GetObject<T>();
                result.Min = min;
                this.proto.AddRule(result);
            }

            return this;
        }

        /// <summary>
        /// Adds the conditional.
        /// </summary>
        /// <returns>The conditional.</returns>
        /// <param name="condition">If set to <c>true</c> condition.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public PasswordBuilder AddConditional<T>(bool condition) 
            where T : Chunk, new()
        {
            if (condition) 
            {
                this.proto.AddRule(Chunk.GetObject<T>());	// oer	(T)Activator.CreateInstance(typeof(T)));
            }

            return this;
        }


        /// <summary>
        /// Validates all minimal char accurance for all defined retrictions fit minimum password length limit
        /// </summary>
        /// <remarks>
        /// It assumes all character rules minimal usage data is summaraized and checked against the  password length
        /// It is FALSE in case rules requires minimum charcters count greater than password length set
        /// </remarks>
        /// <returns>True if length of password greate or equal the charctersets minimal usage</returns>
        public bool IsValidLengthRestrictions()
        {
            return this.MinLengthSetByRules <= this.proto.Length;
        }

        /// <summary>
        /// Adds the validator.
        /// </summary>
        /// <returns>self instance for fluent syntax</returns>
        /// <param name="predicate">Predicate</param>
        /// <param name="ex">Exception</param>
        private PasswordBuilder AddValidator(Func<bool> predicate, SystemException ex)
        {
            this.validators.Add(new KeyValuePair<Func<bool>, Exception>(predicate, ex));
            return this;
        }

        /// <summary>
        /// Validate this instance.
        /// </summary>
        private void Validate()
        {
            try 
            {
                this.validators.AsParallel().ForAll (
                    item => 
                    {
                        if (!item.Key()){ throw item.Value;};
                    });
            }// In this design, we stop query processing when the exception occurs. 
			catch (AggregateException)
            {
                // TODO: add password builder validator exception class and init with AggregateException info
                throw;
            }
        }

        /// <summary>
        /// Gets the random character.
        /// </summary>
        /// <returns>The random character.</returns>
        /// <param name="text">Text.</param>
        /// <param name="rng">Rng.</param>
        private static char GetRandomCharacter(string text, Random rng)
        {
            int index = rng.Next(text.Length);
            return text[index];
        }

        /// <summary>
        /// Generate this instance.
        /// </summary>
        public string Generate()
        {
            Validate();

            List<char> charlist = new List<char>();
            List<char> altlist = new List<char>();

            /// we have to generate <passwordlenth> characters = [MinLengthByRules+take] 
            int take = this.proto.Length - this.MinLengthSetByRules;

            // at the same rule we may generate in advance up to max usage alternate chars
            int left_generate = this.proto.Length;
            var randomSource = new Random(Guid.NewGuid().GetHashCode());
            foreach (var rule in this.proto.Rules) 
            {
                var minusage = rule.Min;
                var maxusage = rule.Max > 0 ? rule.Max : int.MaxValue;

                while (minusage-- > 0)
                {
                    left_generate--;
                    charlist.Add(GetRandomCharacter(rule.Alpha, randomSource));
                    if (maxusage-- > 0) 
                    {
                        left_generate--;
                        altlist.Add(GetRandomCharacter(rule.Alpha, randomSource));
                    }
                }
            }

            while (left_generate-- > 0) 
            {
                // get random rule				
                altlist.Add(GetRandomCharacter(proto.RandomRule(randomSource).Alpha, randomSource));
            }

            StringBuilder result = new StringBuilder();

            // we have to include all minimum required chars	first and then get random set 
            // alternative sort (r.Next(2) % 2) == 0
            charlist.AddRange(altlist.OrderBy(x => Guid.NewGuid()).Take(take));

            // shuffle mandatory chars and alternatives
            result.Append(charlist.OrderBy(x => Guid.NewGuid()).ToArray());		
            return result.ToString();
        }
    }
}