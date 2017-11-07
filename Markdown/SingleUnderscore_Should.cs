using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
	[TestFixture]
	public class SingleUnderscore_Should
	{
		private SingleUnderscore parser;

		[TestCase("_Hello_, _world_", ExpectedResult = "<em>Hello</em>, <em>world</em>",
			TestName = "Two Underscores In A Row")]
		[TestCase("__Hello, _world_", ExpectedResult = "__Hello, <em>world</em>",
			TestName = "Double and Single Underscores")]
		[TestCase("_Hello, world_", ExpectedResult = "<em>Hello, world</em>",
			TestName = "TCorrect Underscores")]
		[TestCase(@"\_Hello, world\_", ExpectedResult = "_Hello, world_",
			TestName = "Screened Underscores")]
		[TestCase(@"\_Hello,\_ _world_", ExpectedResult = "_Hello,_ <em>world</em>", 
			TestName = "Screened and Not Screened Underscores")]
		[TestCase(@"\_Hello\_ \__abc\__", ExpectedResult = @"_Hello_ \__abc\__",
			TestName = "Unscreen single, do not touch double")]
		public string Transform(string markdown)
		{
			parser = new SingleUnderscore(markdown);
			parser.FillEntries();
			return parser.Transform();
		}

		[TestCase("_Hello, world _", ExpectedResult = true, TestName = "Space Before Second Underscore")]
		[TestCase("_ Hello, world_", ExpectedResult = true, TestName = "Space After First Underscore")]
		[TestCase(@"\_Hello, world\_", ExpectedResult = true, TestName = "Screened Underscores")]
		[TestCase("Hello, world_", ExpectedResult = true, TestName = "One Underscore")]
		[TestCase("__Hello, world__", ExpectedResult = true, TestName = "Double Underscores")]
		[TestCase("_1_)", ExpectedResult = true, TestName = "Numbers")]
		[TestCase("_abc __ a", ExpectedResult = true, TestName = "One single One Double")]
		public bool NoCorrectUnderscores(string markdown)
		{
			parser = new SingleUnderscore(markdown);
			parser.FillEntries();
			return !parser.Entries.Any();
		}

		[Test]
		public void CountBackslashes()
		{
			parser = new SingleUnderscore(@"\_Hello, world\_)");
			parser.FillEntries();
			parser.Screens.ShouldAllBeEquivalentTo(new List<int> {0, 14});
		}

		[Test]
		public void CountScreened_And_NotScreened()
		{
			parser = new SingleUnderscore(@"\_Hello,\_ _world_");
			parser.FillEntries();
			parser.Screens.ShouldAllBeEquivalentTo(new List<int> { 0, 8 });
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, TagType>
			{
				{11, TagType.Opening},
				{17, TagType.Closing},
			});
		}

		[Test]
		public void CountCorrectUnderscores()
		{
			parser = new SingleUnderscore("_Hello, world_");
			parser.FillEntries();
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, TagType>
			{
				{0, TagType.Opening},
				{13, TagType.Closing}
			});
		}

		[Test]
		public void CountTwoUnderscoresInRow()
		{
			parser = new SingleUnderscore("_Hello_, _world_");
			parser.FillEntries();
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, TagType>
			{
				{0, TagType.Opening},
				{6, TagType.Closing},
				{9, TagType.Opening},
				{15, TagType.Closing}
			});
		}


		[Test]
		public void DoNotCount_DoubleUnderscores_CountSingle()
		{
			parser = new SingleUnderscore("__Hello, _world_)");
			parser.FillEntries();
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, TagType>
			{
				{9, TagType.Opening},
				{15, TagType.Closing}
			});
		}
	}
}