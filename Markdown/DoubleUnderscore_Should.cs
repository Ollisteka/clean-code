using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
	[TestFixture]
	public class DoubleUnderscore_Should
	{
		[Test]
		public void CountCorrectUndercores()
		{
			var parser = new DoubleUnderscore("__Hello, world__");
			parser.FillEntries();
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, bool>
			{
				{0, true},
				{14, false}
			});
		}

		[Test]
		public void CountTwoPairsInRow()
		{
			var parser = new DoubleUnderscore("__Hello,__ __world__");
			parser.FillEntries();
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, bool>
			{
				{0, true},
				{8, false},
				{11, true},
				{18, false}
			});
		}

		[Test]
		public void DoNotCountScreenedUnderscores()
		{
			var parser = new DoubleUnderscore(@"\__Hello,\__ world");
			parser.FillEntries();
			parser.Entries.Should().BeEmpty();
			parser.Screens.ShouldAllBeEquivalentTo(new List<int> {0, 9});
		}

		[Test]
		public void TransformCorrectUndercores()
		{
			var parser = new DoubleUnderscore("__Hello, world__");
			parser.FillEntries();
			parser.Transform().Should().Be("<b>Hello, world</b>");
		}

		[Test]
		public void TransformScreenedUnderscores()
		{
			var parser = new DoubleUnderscore(@"\__Hello,\__ world");
			parser.FillEntries();
			parser.Transform().Should().Be("__Hello,__ world");
		}

		[Test]
		public void TransformTwoPairsInRow()
		{
			var parser = new DoubleUnderscore("__Hello,__ __world__");
			parser.FillEntries();
			parser.Transform().Should().Be("<b>Hello,</b> <b>world</b>");
		}
	}
}