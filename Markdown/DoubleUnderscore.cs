using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Markdown
{
	internal class DoubleUnderscore : ITransformable
	{
		//ключ - позиция, значение - открывающий ли тег
		private readonly Dictionary<int, TagType> entries = new Dictionary<int, TagType>();

		private readonly List<int> screens = new List<int>();

		private readonly Dictionary<TagType, string> tags = new Dictionary<TagType, string>
		{
			{TagType.Opening, "<strong>"},
			{TagType.Closing, "</strong>"}
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
		public IReadOnlyDictionary<int, TagType> Entries => entries;

		public void SetMarkdown(string markdownValue)
		{
			if (markdown is null)
				markdown = markdownValue;
			else
				throw new Exception("The markdownValue value was set in constructor!");
		}

		public void FillEntries()
		{
			var opening = TagType.Opening;
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
				opening = (TagType) ((int)(opening+1) % 2);
				i += 1;
			}
			CheckForDoubleUnderscoresInsideSingle();
		}

		public string Transform()
		{
			var result = new StringBuilder(markdown);
			var offset = 0;
			foreach (var entry in Entries)
			{
				result.Remove(entry.Key + offset, 2);
				result.Insert(entry.Key + offset, tags[entry.Value]);
				if (entry.Value == TagType.Opening)
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

		private void CheckForDoubleUnderscoresInsideSingle()
		{
			var openingTag = SingleUnderscore.Tags[TagType.Opening];
			var closingTag = SingleUnderscore.Tags[TagType.Closing];
			var regexp = new Regex(openingTag + ".*" + closingTag);
			var results = regexp.Matches(markdown);
			if (results.Count == 0)
				return;

			for (var position = 0; position < entries.Keys.Count; position += 2)
			{
				var openingUnderscoreIndex = entries.ElementAt(position).Key;
				var closingUnderscoreIndex = entries.ElementAt(position + 1).Key;
				foreach (var result in results)
				{
					var match = result as Match;
					var startIndex = match.Index;
					var lastIndex = startIndex + match.Length - 1;
					if (startIndex > openingUnderscoreIndex || lastIndex < closingUnderscoreIndex) continue;
					entries.Remove(openingUnderscoreIndex);
					entries.Remove(closingUnderscoreIndex);
				}
			}
		}
	}
}