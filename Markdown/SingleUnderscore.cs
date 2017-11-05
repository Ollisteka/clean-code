using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
	public class SingleUnderscore : ITransformable
	{
		//ключ - позиция, значение - открывающий ли тег
		private readonly Dictionary<int, bool> entries = new Dictionary<int, bool>();

		private readonly string markdown;
		private readonly List<int> screens = new List<int>();

		private readonly Dictionary<bool, string> tags = new Dictionary<bool, string>
		{
			{true, "<em>"},
			{false, "</em>"}
		};

		public SingleUnderscore(string markdown)
		{
			this.markdown = markdown;
		}

		public IReadOnlyList<int> Screens => screens;
		public IReadOnlyDictionary<int, bool> Entries => entries;

		public void FillEntries()
		{
			var opening = true;
			for (var i = 0; i < markdown.Length; i++)
			{
				if (markdown[i] != '_')
					continue;
				// check for double underscores
				if (entries.ContainsKey(i - 1))
				{
					entries.Remove(i - 1);
					opening = !opening;
					continue;
				}
				// check for space after opening underscore
				if (opening && i + 1 < markdown.Length && markdown[i + 1] == ' ')
					continue;
				// check for space before closing iunderscore
				if (!opening && markdown[i - 1] == ' ')
					continue;
				// check for screened underscores
				if (i >= 1 && markdown[i - 1] == '\\')
				{
					entries.Remove(i);
					screens.Add(i - 1);
					opening = !opening;
					continue;
				}
				entries.Add(i, opening);
				opening = !opening;
			}
			if (entries.Count % 2 != 0)
				entries.Remove(entries.Keys.Max());
		}

		public string Transform()
		{
			var result = new StringBuilder(markdown);
			var offset = 0;
			foreach (var entry in Entries)
			{
				result.Remove(entry.Key + offset, 1);
				result.Insert(entry.Key + offset, tags[entry.Value]);
				if (entry.Value)
					offset += 3;
				else offset += 4;
			}
			foreach (var position in screens)
			{
				result.Remove(position + offset, 1);
				offset -= 1;
			}
			return result.ToString();
		}
	}
}