namespace OV.Tools.Generators.Password
{
	using NUnit.Framework;
	using System;

	[TestFixture ()]
	public class Test
	{
		[Test()]
		public void TestCaseCunkGetObjects ()
		{          
            Assert.IsInstanceOf<LowAlpha>(Chunk.GetObject<LowAlpha>());
		}

        [Test()]
        public void TestCasePasswordLength()
        {
            PasswordBuilder pb = new PasswordBuilder ();
            pb.AddChunk (new Chunk (){ Alpha = "a", Min=10 });
            pb.SetLength (1);
            Assert.Catch<AggregateException> (()=>pb.Generate ());
        }

        [Test()]
        public void TestCasePasswordSameChars()
        {
            PasswordBuilder pb = new PasswordBuilder ();
            pb.AddChunk (new Chunk (){ Alpha = "a", Min=1 });
            pb.SetLength (1);
            Assert.AreEqual (pb.Generate (), "a");
        }

        [Test()]
        public void TestCasePasswordMinCharsByRules1()
        {
            PasswordBuilder pb = new PasswordBuilder ();
            pb.AddChunk (new Chunk (){ Alpha = "a", Min=2 });
            Assert.AreEqual (pb.MinLengthSetByRules, 2);

        }

        [Test()]
        public void TestCasePasswordMinCharsByRules2()
        {
            PasswordBuilder pb = new PasswordBuilder ();
            pb.AddChunk (new Chunk (){ Alpha = "a", Min=2 });
            pb.AddChunk (new Chunk (){ Alpha = "b", Min=2 });
            Assert.AreEqual (pb.MinLengthSetByRules, 4);

        }
	}
}