﻿using System.Collections.Generic;
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
		public string Transform(string markdown)
		{
			parser = new SingleUnderscore(markdown);
			parser.FillEntries();
			return parser.Transform();
		}

		[TestCase("_Hello, world _", ExpectedResult = true, TestName = "Space Before Second Underscore")]
		[TestCase("_ Hello, world_", ExpectedResult = true, TestName = "Space After First Underscore")]
		[TestCase(@"\_Hello, world\_)", ExpectedResult = true, TestName = "Screened Underscores")]
		[TestCase("Hello, world_", ExpectedResult = true, TestName = "One Underscore")]
		[TestCase("__Hello, world__)", ExpectedResult = true, TestName = "Double Underscores")]
		[TestCase("_1_)", ExpectedResult = true, TestName = "Numbers")]
		public bool NoCorrectUnderscores(string markdown)
		{
			parser = new SingleUnderscore(markdown);
			parser.FillEntries();
			return parser.Entries.Count == 0;
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
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, bool>
			{
				{11, true},
				{17, false},
			});
		}

		[Test]
		public void CountCorrectUnderscores()
		{
			parser = new SingleUnderscore("_Hello, world_");
			parser.FillEntries();
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, bool> {{0, true}, {13, false}});
		}

		[Test]
		public void CountTwoUnderscoresInRow()
		{
			parser = new SingleUnderscore("_Hello_, _world_");
			parser.FillEntries();
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, bool>
			{
				{0, true},
				{6, false},
				{9, true},
				{15, false}
			});
		}


		[Test]
		public void DoNotCount_DoubleUnderscores_CountSingle()
		{
			parser = new SingleUnderscore("__Hello, _world_)");
			parser.FillEntries();
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, bool>
			{
				{9, true},
				{15, false}
			});
		}
	}
}