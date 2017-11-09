using NUnit.Framework;

namespace Markdown
{
	[TestFixture]
	public class BacktickQuotes_Should
	{
		private BacktickQuotes parser;

		[TestCase("`Hello`", ExpectedResult = "<code>Hello</code>",
			TestName = "Simple")]
		[TestCase("\\`Hello`", ExpectedResult = "`Hello`",
			TestName = "Screened and unpaired quotes")]
		[TestCase(@"\`Hello\`", ExpectedResult = "`Hello`",
			TestName = "Screened quotes")]
		[TestCase(@"`Hello _abc_ world`", ExpectedResult = @"<code>Hello \_abc\_ world</code>",
			TestName = "Single underscore inside")]
		[TestCase(@"`Hello _abc_ _cdbc_ world`", ExpectedResult = @"<code>Hello \_abc\_ \_cdbc\_ world</code>",
			TestName = "Several single underscore inside")]
		[TestCase(@"`Hello __abc__ world`", ExpectedResult = @"<code>Hello \__abc\__ world</code>",
			TestName = "Double underscore inside")]
		[TestCase(@"`Hello __abc__ _world_`", ExpectedResult = @"<code>Hello \__abc\__ \_world\_</code>",
			TestName = "Double and single underscore inside")]
		[TestCase(@"`Hello _abc_ __world__`", ExpectedResult = @"<code>Hello \_abc\_ \__world\__</code>",
			TestName = "Single and double underscore inside")]
		[TestCase(@"`_abc_`", ExpectedResult = @"<code>\_abc\_</code>",
			TestName = "Single inside")]
		[TestCase(@"`_abc, bcd_`", ExpectedResult = @"<code>\_abc, bcd\_</code>",
			TestName = "Punctuation inside")]
		public string Transform(string markdown)
		{
			parser = new BacktickQuotes(markdown);
			parser.FillEntries();
			return parser.Transform();
		}
	}
}