namespace Markdown
{
	public static class StringExtensions
	{
		public static string Parse(this string markdown, ITransformable parser)
		{
			parser.SetMarkdown(markdown);
			parser.FillEntries();
			return parser.Transform();
		}
	}
}