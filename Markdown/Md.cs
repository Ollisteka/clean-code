using System.Linq;

namespace Markdown
{
	public class Md
	{
		public string RenderToHtml(string markdown)
		{
			var result = markdown.Split('\n').Select(ParseLine).ToList();
			return string.Join("\n", result); //TODO
		}

		private static string ParseLine(string markdown)
		{
			return markdown.ParseSingleUnderscores().ParseDoubleUnderscores();
		}
	}
}