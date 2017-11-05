using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
	public class Md
	{
		private int offset;
		public string RenderToHtml(string markdown)
		{
			var result = markdown.Split('\n').Select(ParseLine).ToList();
			return string.Join("\n", result); //TODO
		}

		private string ParseLine(string markdown)
		{
			var underscores = new List<int>();
			var doubleUnderscores = new List<int>();
			for (var i = 0; i < markdown.Length; i++)
			{
				if (markdown[i] != '_') continue;

				if (underscores.Contains(i - 1))
				{
					underscores.Remove(i - 1);
					doubleUnderscores.Add(i - 1);
				}
				else
				{
					underscores.Add(i);
				}
			}
			markdown = ParseSingleUnderscores(markdown, underscores);
			return ParseDoubleUnderscores(doubleUnderscores, markdown);
		}

		private string ParseSingleUnderscores(string markdown, List<int> underscores)
		{
			if (underscores.Count == 0)
				return markdown;
			var res = new StringBuilder(markdown);
			var closing = false;
			for (var i = 0; i < underscores.Count; i++)
			{
				if (i == underscores.Count - 1 && !closing && underscores.Count % 2 != 0)
					break;
				var position = underscores[i];
				if (position >= 1 && res[position - 1 + offset] == '\\')
				{
					res.Remove(position - 1 + offset, 1);
					offset -= 1;
					continue;
				}
				res.Remove(position + offset, 1);
				res.Insert(position + offset, closing ? "</em>" : "<em>");
				offset += 3;
				closing = !closing;
			}
			offset += 1;
			return res.ToString();
		}

		private string ParseDoubleUnderscores(List<int> doubleUnderscores, string markdown)
		{
			if (doubleUnderscores.Count == 0)
				return markdown;
			var res = new StringBuilder(markdown);
			var closing = false;
			for (var i = 0; i < doubleUnderscores.Count; i++)
			{
				if (i == doubleUnderscores.Count - 1 && !closing && doubleUnderscores.Count % 2 != 0)
					break;
				var position = doubleUnderscores[i];
				res.Remove(position + offset, 2);
				res.Insert(position + offset, closing ? "</b>" : "<b>");
				offset += 1;
				closing = !closing;
			}
			return res.ToString();
		}
	}
}