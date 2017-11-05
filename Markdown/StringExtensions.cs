namespace Markdown
{
	public static class StringExtensions
	{
		public static string ParseDoubleUnderscores(this string markdown)
		{
			var parser = new DoubleUnderscore(markdown);
			parser.FillEntries();
			return parser.Transform();
		}

		public static string ParseSingleUnderscores(this string markdown)
		{
			var parser = new SingleUnderscore(markdown);
			parser.FillEntries();
			return parser.Transform();
		}
	}
}