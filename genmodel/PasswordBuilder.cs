using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Timers;

namespace com.ovarchenko.tools.generators.password
{
	public class PasswordBuilder
	{
		private PasswordProto proto;
		private HashSet<KeyValuePair<Func<bool>, Exception>> validators = new HashSet<KeyValuePair<Func<bool>, Exception>>();

		public PasswordBuilder()
		{
			proto = new PasswordProto();

			AddValidator(IsValidLengthRestrictions, new ArgumentException(
				string.Format("Minimal password length {0} doesn't fit summary charactersets minimal usage '{1}' in the password.",
					MinLengthSetByRules,proto.Max)));
		}

		public PasswordBuilder(int maxLen)
			: this()
		{
			proto.Max = maxLen;
		}

		public PasswordBuilder(int minLen, int maxLen)
			: this()
		{
			proto.Min = minLen;
			proto.Max = maxLen;
		}

		public PasswordBuilder AddChunk(Chunk newChunk)
		{
			proto.AddRule(newChunk);
			return this;
		}

		public PasswordBuilder SetLength(int n)
		{
			proto.Min = proto.Max = n;
			proto.Length = n;
			return this;
		}
		
		public PasswordBuilder SetMaxLength(int n)
		{
			proto.Max = n;
			proto.Length = n;
			return this;
		}

		public PasswordBuilder SetMinimalLength(int n)
		{
			proto.Min = n;
			return this;
		}


		public PasswordBuilder AddSpecials()
		{
			AddConditional<SpecialChars>(true);
			return this;
		}

		public PasswordBuilder AddConditional<T>(bool condition, int min, int max) where T : Chunk, new()
		{
			if (condition)
			{
				var result = Chunk.GetObject<T>();
				result.Min = min;
				result.Max = max;
				proto.AddRule(result);	
			}
			return this;
		}

		public PasswordBuilder AddConditional<T>(bool condition, int min) where T : Chunk, new()
		{
			if (condition)
			{
				var result = Chunk.GetObject<T>();
				result.Min = min;
				proto.AddRule(result);	
			}
			return this;
		}

		public PasswordBuilder AddConditional<T>(bool condition) where T : Chunk, new()
		{
			if (condition)
			{
				proto.AddRule(Chunk.GetObject<T>());	//or	(T)Activator.CreateInstance(typeof(T)));
			}
			return this;
		}

		public int MinLengthSetByRules
		{
			get
			{
				return proto.Rules.Aggregate(0, (current, sequence) => current + sequence.Min);
			}
		}

		/// <summary>
		/// Validates all minimal char accurance for all defined retrictions fit minimum password length limit
		/// </summary>
		/// <remarks>
		/// It assumes all character rules minimal usage data is summaraized and checked against the  password length
		/// It is FALSE in case rules requires minimum charcters count greater than password length set
		/// </remarks>
		public bool IsValidLengthRestrictions()
		{
			return MinLengthSetByRules < proto.Length;
		}

		private PasswordBuilder AddValidator(Func<bool> predicate, SystemException ex)
		{
			validators.Add(new KeyValuePair<Func<bool>, Exception>(predicate, ex));
			return this;
		}

		private void Validate()
		{
			try
			{
				validators.AsParallel().ForAll(item =>
				{
					if (!item.Key()) throw item.Value;
				}
				);
			}
			// In this design, we stop query processing when the exception occurs. 
			catch (AggregateException e)
			{
				foreach (var ex in e.InnerExceptions)
				{
					Console.WriteLine(ex.Message);
					if (ex is ArgumentException)
						Console.WriteLine("The parameters is invalid, processing is stopped.");
				}
			}
		}

		private static char GetRandomCharacter(string text, Random rng)
		{
			int index = rng.Next(text.Length);
			return text[index];
		}

		public string Generate()
		{
			Validate();
			List<Char> charlist = new List<char>();
			List<Char> altlist = new List<char>();
			int left_generate = proto.Length;

			//HashSet<KeyValuePair<Chunk, List<char>>> alternatives = new HashSet<KeyValuePair<Chunk, List<char>>>();
			var r = new Random();
			//var realPasswordLength = r.Next(proto.Min, proto.Max);
			foreach (var rule in proto.Rules)
			{
				var minusage = rule.Min;
				var maxusage = rule.Max>0?rule.Max:int.MaxValue;
				//				List<Char> altlist = new List<char>();

				while(minusage-->0)
				{
					left_generate--;
					charlist.Add(GetRandomCharacter(rule.Alpha,r));
					if (maxusage-- > 0)
					{
						left_generate--;
						altlist.Add(GetRandomCharacter(rule.Alpha,r));
					}
				}								
				//alternatives.Add(new KeyValuePair<Chunk, List<char>>(rule, altlist));
			}

			int take = proto.Length-MinLengthSetByRules;
			while(left_generate-->0)
			{
				//get random rule				
				altlist.Add(GetRandomCharacter(proto.RandomRule(r).Alpha,r));
			}

			StringBuilder sb = new StringBuilder();
			//we have to include all minimum required chars		
			//alternative sort (r.Next(2) % 2) == 0
			charlist.AddRange(altlist.OrderBy(x=>Guid.NewGuid()).Take(take));
			sb.Append(charlist.OrderBy(x=>Guid.NewGuid()).ToArray());		
			return sb.ToString();
		}
	}
}