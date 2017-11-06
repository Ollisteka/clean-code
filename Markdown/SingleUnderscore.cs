﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
	public class SingleUnderscore : ITransformable
	{
		//ключ - позиция, значение - открывающий ли тег
		private readonly Dictionary<int, bool> entries = new Dictionary<int, bool>();

		private readonly List<int> screens = new List<int>();

		public static readonly Dictionary<bool, string> Tags = new Dictionary<bool, string>
		{
			{true, "<em>"},
			{false, "</em>"}
		};

		private string markdown;

		public SingleUnderscore()
		{
		}

		public SingleUnderscore(string markdown)
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
				throw new Exception("The markdownValue value is already set!");
		}

		public void FillEntries()
		{
			var opening = true;
			for (var i = 0; i < markdown.Length; i++)
			{
				if (markdown[i] != '_')
					continue;
				// check for double underscores
				if (i+1<markdown.Length && markdown[i+1] == '_')
				{
					i += 1;
					continue;
				}
				// check for screened underscores
				if (i >= 1 && markdown[i - 1] == '\\')
				{
					screens.Add(i - 1);
					continue;
				}
				// check for space after opening underscore
				if (opening && i + 1 < markdown.Length && markdown[i + 1] == ' ')
					continue;
				// check for space before closing iunderscore
				if (!opening && markdown[i - 1] == ' ')
					continue;
				// check for numbers inside
				if (!opening)
				{
					var lastElement = entries.Keys.Max();
					if (markdown.Substring(lastElement, i - lastElement).Any(char.IsDigit))
					{
						entries.Remove(lastElement);
						opening = true;
					}
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
			foreach (var position in screens)
			{
				result.Remove(position + offset, 1);
				offset -= 1;
			}
			foreach (var entry in Entries)
			{
				result.Remove(entry.Key + offset, 1);
				result.Insert(entry.Key + offset, Tags[entry.Value]);
				if (entry.Value)
					offset += 3;
				else offset += 4;
			}
			return result.ToString();
		}
	}
}