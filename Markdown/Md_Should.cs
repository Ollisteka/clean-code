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
		[TestCase("__Hello _abc_ world__", ExpectedResult = "<strong>Hello <em>abc</em> world</strong>",
			TestName = "Parse single inside double")]
		[TestCase("_Hello __abc__ world_", ExpectedResult = "<em>Hello __abc__ world</em>",
			TestName = "Do not parse double inside single")]
		[TestCase(@"\_Hello\_ _world_", ExpectedResult = "_Hello_ <em>world</em>",
			TestName = "Screens before tags")]
		[TestCase("___Hello, world___", ExpectedResult = "<strong><em>Hello, world</strong></em>",
			TestName = "Three Underscores")]
		[TestCase("____Hello, world____", ExpectedResult = "<strong><strong>Hello, world</strong></strong>",
			TestName = "Four Underscores")]
		[TestCase("`__Hello, world__`", ExpectedResult = "<code>__Hello, world__</code>",
			TestName = "Backtick Quotes with double underscores")]
		[TestCase("`_Hello, world_`", ExpectedResult = "<code>_Hello, world_</code>",
			TestName = "Backtick Quotes with single underscores")]
		[TestCase(@"`Hello __abc__ _world_`", ExpectedResult = @"<code>Hello __abc__ _world_</code>",
			TestName = "Double and single underscore inside Backtick Quotes")]
		[TestCase(@"`Hello _abc_ __world__`", ExpectedResult = @"<code>Hello _abc_ __world__</code>",
			TestName = "Single and double underscore inside Backtick Quotes")]
		public string RenderToHtml(string markdown)
		{
			return MdRender.RenderToHtml(markdown);
		}
	}
}