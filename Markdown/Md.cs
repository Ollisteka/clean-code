using System.Linq;

namespace Markdown
{
	public class Md
	{
		public string RenderToHtml(string markdown)
		{
			var result = markdown.Split('\n');;
			for (var i = 0; i < result.Length; i++)
			{
				result[i] = ParseLine(result[i]);
				if (Headers.OpeningTags.ContainsKey(result[i]))
					result[i - 1] = Headers.OpeningTags[result[i]] + result[i - 1];
			}
			return string.Join("\n", result); //TODO
		}

		private static string ParseLine(string markdown)
		{
			return markdown.ParseSingleUnderscores().ParseDoubleUnderscores().ParseHeaders();
		}
	}
}