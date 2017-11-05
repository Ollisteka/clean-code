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