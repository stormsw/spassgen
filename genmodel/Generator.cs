using System;
using System.Collections.Generic;

namespace com.ovarchenko.tools.generators.password
{
	public class CapsAlpha : Chunk
	{
		public CapsAlpha()
		{
			Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			Min = 1;
		}
	}

	public class LowAlpha : Chunk
	{
		public LowAlpha()
		{
			Alpha = "abcdefghijklmnopqrstuvwxyz";
			Min = 1;
		}
	}

	public class Numerals : Chunk
	{
		public Numerals()
		{
			Alpha = "0123456789";
			Min = 1;
		}
	}
	public class Spacers : Chunk
	{
		public Spacers()
		{
			Alpha = " \t";
			Min = 1;
		}
	}

	public class SpecialChars : Chunk
	{
		public SpecialChars()
		{
			Alpha = "~!@#$%^&*()_-=+{}[]:;\"'|\\/?.,";
			Min = 1;
		}
	}
}