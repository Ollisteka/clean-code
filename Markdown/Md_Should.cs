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
		[TestCase("This is header1\n===", ExpectedResult = "<h1>This is header1\n</h1>",
			TestName = "Header 1")]
		[TestCase("This is header2\n-", ExpectedResult = "<h2>This is header2\n</h2>",
			TestName = "Header 2")]
		[TestCase("This is not a header\n-+", ExpectedResult = "This is not a header\n-+",
			TestName = "Not header symbol")]
		[TestCase("This is not a header---", ExpectedResult = "This is not a header---",
			TestName = "No new line for header")]
		public string RenderToHtml(string markdown)
		{
			return MdRender.RenderToHtml(markdown);
		}
	}
}