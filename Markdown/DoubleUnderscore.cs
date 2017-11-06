using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
	internal class DoubleUnderscore : ITransformable
	{
		//ключ - позиция, значение - открывающий ли тег
		private readonly Dictionary<int, bool> entries = new Dictionary<int, bool>();

		private readonly List<int> screens = new List<int>();

		private readonly Dictionary<bool, string> tags = new Dictionary<bool, string>
		{
			{true, "<strong>"},
			{false, "</strong>"}
		};

		private string markdown;

		public DoubleUnderscore()
		{
		}

		public DoubleUnderscore(string markdown)
		{
			this.markdown = markdown;
		}

		public IReadOnlyList<int> Screens => screens;
		public IReadOnlyDictionary<int, bool> Entries => entries;

		public void SetMarkdown(string markdownValue)
		{
			if (markdown is null)
				markdown = markdownValue;
			else
				throw new Exception("The markdownValue value was set in constructor!");
		}

		public void FillEntries()
		{
			var opening = true;
			for (var i = 0; i < markdown.Length; i += 1)
			{
				if (i + 2 < markdown.Length
				    && markdown[i] == '\\'
				    && markdown[i + 1] == '_'
				    && markdown[i + 2] == '_')
				{
					screens.Add(i);
					i += 2;
					continue;
				}
				if (i + 1 >= markdown.Length || markdown[i] != '_' || markdown[i + 1] != '_') continue;

				entries.Add(i, opening);
				opening = !opening;
				i += 1;
			}
		}

		public string Transform()
		{
			var result = new StringBuilder(markdown);
			var offset = 0;
			foreach (var entry in Entries)
			{
				result.Remove(entry.Key + offset, 2);
				result.Insert(entry.Key + offset, tags[entry.Value]);
				if (entry.Value)
					offset += 6;
				else offset += 7;
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