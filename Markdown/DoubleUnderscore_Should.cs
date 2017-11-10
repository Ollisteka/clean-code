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
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, TagType>
			{
				{0, TagType.Opening},
				{14, TagType.Closing}
			});
		}

		[Test]
		public void CountTwoPairsInRow()
		{
			var parser = new DoubleUnderscore("__Hello,__ __world__");
			parser.FillEntries();
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, TagType>
			{
				{0, TagType.Opening},
				{8, TagType.Closing},
				{11, TagType.Opening},
				{18, TagType.Closing}
			});
		}

		[Test]
		public void CountFourUnderscores()
		{
			var parser = new DoubleUnderscore("____Hello, world____");
			parser.FillEntries();
			parser.Entries.ShouldBeEquivalentTo(new Dictionary<int, TagType>
			{
				{0, TagType.Opening},
				{2, TagType.Opening},
				{16, TagType.Closing},
				{18, TagType.Closing}
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

		[TestCase("__Hello,__ __world__", ExpectedResult = "<strong>Hello,</strong> <strong>world</strong>",
			TestName = "Two Pairs in a Row")]
		[TestCase(@"\__Hello,\__ world", ExpectedResult = "__Hello,__ world",
			TestName = "Screened Underscores")]
		[TestCase("__Hello, world__", ExpectedResult = "<strong>Hello, world</strong>",
			TestName = "Correct Underscores")]
		[TestCase("___Hello, world___", ExpectedResult = "<strong>_Hello, world</strong>_",
			TestName = "Three Underscores")]
		public string Transform(string markdown)
		{
			var parser = new DoubleUnderscore(markdown);
			parser.FillEntries();
			return parser.Transform();
		}
	}
}