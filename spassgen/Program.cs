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

			p.SetupHelp("?", "help").Callback(text => Console.WriteLine(text));
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
			 .SetDefault (8).WithDescription("Set password length."); //Can be Required()
			
            /*
			int alphaLength = 1;

			p.Setup<int>('c', "alfa-length")
			 .Callback(value => builder.SetLength(value))
			 .SetDefault(8); //Can be Required()*/

            p.Setup<bool> ('s', "special")
			 .Callback (silent => builder.AddConditional<SpecialChars> (silent, 1))
			 .SetDefault (false).WithDescription(string.Format("Use special: {0}",new SpecialChars().Alpha));

            p.Setup<bool> ('d', "digits")
			 .Callback (silent => builder.AddConditional<Numerals> (silent, 1))
			 .SetDefault(false).WithDescription(string.Format("Use numeric characters: {0}", new Numerals().Alpha));

            p.Setup<bool> ('a', "small-chars")
			 .Callback (silent => builder.AddConditional<LowAlpha> (silent, 1))
			 .SetDefault(false).WithDescription(string.Format("Use low alpha: {0}", new LowAlpha().Alpha));

            p.Setup<bool> ('z', "caps-chars")
			 .Callback (silent => builder.AddConditional<CapsAlpha> (silent, 1))
			 .SetDefault(false).WithDescription(string.Format("Use capitals: {0}", new CapsAlpha().Alpha));

			p.Setup<string>('x', "custom")
			 .Callback(value => builder.AddChunk(new Chunk(){Alpha=value,Min=1}))
			 .WithDescription("Use custom charset");

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
