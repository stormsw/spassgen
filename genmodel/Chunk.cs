
using System;
namespace com.ovarchenko.tools.generators.password
{
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
		[Obsolete("At usual we dont have to restrict max characters for password. It wil be removed.",false)]
		private int max;

		/// <summary>
		/// minimal appearance (0 - unlimited)
		/// </summary>
		private int min;

		public string Alpha
		{
			get { return alpha; }
			set { alpha = value; }
		}
		public int Max
		{
			get { return max; }
			set { max = value; }
		}

		public int Min
		{
			get { return min; }
			set { min = value; }
		}

		public static Chunk GetObject<T> () where T: Chunk, new()
		{
			return new T();
		}

	}
}