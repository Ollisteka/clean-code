using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
	[TestFixture]
	public class SingleUnderscore_Should
	{
		private SingleUnderscore parser;

		[Test]
		public void CountCorrectUnderscores()
		{
			parser = new SingleUnderscore("_Hello, world_");
			parser.FillEntries();
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, bool>{{0, true},{13,false}});
		}
		[Test]
		public void TransformCorrectUnderscores()
		{
			parser = new SingleUnderscore("_Hello, world_");
			parser.FillEntries();
			parser.Transform().Should().Be("<em>Hello, world</em>");
		}
		[Test]
		public void DoNotCount_DoubleUnderscores()
		{
			parser = new SingleUnderscore("__Hello, world__)");
			parser.FillEntries();
			parser.Entries.Should().BeEmpty();
		}
		[Test]
		public void DoNotCount_DoubleUnderscores_CountSingle()
		{
			parser = new SingleUnderscore("__Hello, _world_)");
			parser.FillEntries();
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, bool>
			{
				{ 9, true },
				{ 15, false },
			});
		}
		[Test]
		public void DoNotTransform_DoubleUnderscores_TransformSingle()
		{
			parser = new SingleUnderscore("__Hello, _world_");
			parser.FillEntries();
			parser.Transform().Should().Be("__Hello, <em>world</em>");
		}
		[Test]
		public void DoNotCount_OneUnderscore()
		{
			parser = new SingleUnderscore("Hello, world_)");
			parser.FillEntries();
			parser.Entries.Should().BeEmpty();
		}
		[Test]
		public void Transform_ScreenedUnderscore()
		{
			parser = new SingleUnderscore(@"\_Hello, world\_");
			parser.FillEntries();
			parser.Transform().Should().Be("_Hello, world_");
		}
		[Test]
		public void DoNotCount_ScreenedUnderscore()
		{
			parser = new SingleUnderscore(@"\_Hello, world\_)");
			parser.FillEntries();
			parser.Entries.Should().BeEmpty();
			parser.Screens.ShouldAllBeEquivalentTo(new List<int> { 0, 14 });
		}
		[Test]
		public void DoNotCount_When_SpaceAfterFirstUnderscore()
		{
			parser = new SingleUnderscore("_ Hello, world_)");
			parser.FillEntries();
			parser.Entries.Should().BeEmpty();
		}
		[Test]
		public void DoNotCount_When_SpaceBeforSecondUnderscore()
		{
			parser = new SingleUnderscore("_Hello, world _)");
			parser.FillEntries();
			parser.Entries.Should().BeEmpty();
		}


		[Test]
		public void CountTwoUnderscoresInRow()
		{
			parser = new SingleUnderscore("_Hello_, _world_");
			parser.FillEntries();
			parser.Entries.ShouldAllBeEquivalentTo(new Dictionary<int, bool>
			{
				{ 0, true },
				{ 6, false },
				{ 9, true },
				{ 15, false }
			});
		}
		[Test]
		public void TransformTwoUnderscoresInRow()
		{
			parser = new SingleUnderscore("_Hello_, _world_");
			parser.FillEntries();
			parser.Transform().Should().Be("<em>Hello</em>, <em>world</em>");
		}

	}
}