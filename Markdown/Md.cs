using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
	public class Md
	{
		public string RenderToHtml(string markdown)
		{
			var result = markdown.Split('\n');
			for (var i = 0; i < result.Length; i++)
			{
				if (string.IsNullOrEmpty(result[i]))
					continue;
				result[i] = ParseLine(result[i]);
				if (Headers.OpeningTags.ContainsKey(result[i]))
					result[i - 1] = Headers.OpeningTags[result[i]] + result[i - 1];
			}
			return string.Join("\n", result);
		}

		private static string ParseLine(string markdown)
		{
			// Чтобы двойные подчёркивания не заменялись на теги внутри одинарных,
			// парсить одинарные нужно ДО двойных
			var parsers = new List<IParsable> {new SingleUnderscore(), new DoubleUnderscore(), new Headers()};
			return parsers.Aggregate(markdown, (current, parser) => parser.Parse(current));
		}
	}
}