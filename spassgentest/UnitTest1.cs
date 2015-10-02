using OV.Tools.Generators.Password;
using System;
using System.Linq;
using Xunit;

namespace OV.Tools.Generators.Password
{
	public class UnitTest1
	{
		[Fact]
		public void TestMethod1()
		{
			PasswordBuilder pb = new PasswordBuilder();
            pb.AddChunk(new Chunk() { Alpha = "a", Min = 2 });
            pb.AddChunk(new Chunk() { Alpha = "b", Min = 2 });
            Assert.Equal(pb.MinLengthSetByRules, 4);

		}
	}
}
