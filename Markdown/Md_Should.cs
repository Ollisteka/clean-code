using NUnit.Framework;

namespace Markdown
{
	[TestFixture]
	public class Md_Should
	{
		[SetUp]
		public void SetUp()
		{
			MdRender = new Md();
		}

		public Md MdRender;

		[TestCase("_Hello,_ __world__", ExpectedResult = "<em>Hello,</em> <strong>world</strong>",
			TestName = "Single And Double Underscores")]
		[TestCase("__Hello,__ _world_", ExpectedResult = "<strong>Hello,</strong> <em>world</em>",
			TestName = "Double And Single Underscores")]
		[TestCase("__abcd _abc_ abcd__", ExpectedResult = "<strong>abcd <em>abc</em> abcd</strong>",
			TestName = "Single Inside Double")]
		public string RenderToHtml(string markdown)
		{
			return MdRender.RenderToHtml(markdown);
		}
	}
}