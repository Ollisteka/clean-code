using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
	[TestFixture]
	public class Md_Should
	{
		public Md MdRender;

		[SetUp]
		public void SetUp()
		{
			MdRender = new Md();
		}

		[Test]
		public void Parse_SingleUnderscore_ToItalics()
		{
			MdRender.RenderToHtml("_Hello, world_").Should().Be("<em>Hello, world</em>");
		}
		[Test]
		public void DoNotParse_SingleUnderscore_When_Screened()
		{
			MdRender.RenderToHtml(@"\_Hello, world\_").Should().Be("_Hello, world_");
		}
		[Test]
		public void Parse_DoubleUnderscore_ToBold()
		{
			MdRender.RenderToHtml("__Hello, world__").Should().Be("<b>Hello, world</b>");
		}

		[Test]
		public void ParseTwo_FromThreeUnderscores()
		{
			MdRender.RenderToHtml("_Hello,_ world_").Should().Be("<em>Hello,</em> world_");
		}

		[Test]
		public void Parse_SingleAndDoubleUnderscores()
		{
			MdRender.RenderToHtml("_Hello,_ __world__").Should().Be("<em>Hello,</em> <b>world</b>");
		}
		[Test]
		public void Parse_DoubleAndSingleUnderscores()
		{
			MdRender.RenderToHtml("__Hello,__ _world_").Should().Be("<b>Hello,</b> <em>world</em>");
		}
	}
}