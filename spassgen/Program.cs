//-----------------------------------------------------------------------
// <copyright file="Chunk.cs" company="Alexander Varchenko">
//     Copyright (c) Alexander Varchenko. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace OV.Tools.Generators.Password
{

    using Fclp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        static void Main (string[] args)
        {
            // for examples look here: http://fclp.github.io/fluent-command-line-parser/
            var p = new FluentCommandLineParser ();
            var builder = new PasswordBuilder ();
            /* 
			p.Setup<int>('n',"minimal")
			 .Callback(min => builder.SetMinimalLength(min))
			 .SetDefault(8);

			p.Setup<int>('m')
			 .Callback(value => builder.SetMaxLength( value))
			 .SetDefault(8); //Can be Required()
			*/
            p.Setup<int> ('l', "length")
			 .Callback (value => builder.SetLength (value))
			 .SetDefault (8); //Can be Required()
			
            /*
			int alphaLength = 1;

			p.Setup<int>('c', "alfa-length")
			 .Callback(value => builder.SetLength(value))
			 .SetDefault(8); //Can be Required()*/

            p.Setup<bool> ('s', "special")
			 .Callback (silent => builder.AddConditional<SpecialChars> (true, 1))
			 .SetDefault (false);

            p.Setup<bool> ('d', "digits")
			 .Callback (silent => builder.AddConditional<Numerals> (true, 2))
			 .SetDefault (true);

            p.Setup<bool> ('a', "small-chars")
			 .Callback (silent => builder.AddConditional<LowAlpha> (true, 3))
			 .SetDefault (true);

            p.Setup<bool> ('z', "caps-chars")
			 .Callback (silent => builder.AddConditional<CapsAlpha> (true, 1))
			 .SetDefault (true);

            p.Parse (args);

            try 
            {
                string password = builder.Generate ();	
                Console.WriteLine (password);
            } 
			// TODO: use special PasswordBuilder exception there
			catch (AggregateException e) 
            {
                foreach (var ex in e.InnerExceptions) 
                {
                    Console.WriteLine(ex.Message);
                    if (ex is ArgumentException) {
                        Console.WriteLine ("The parameters is invalid, processing is stopped.");
                    }
                }
            }
        }
    }
}
