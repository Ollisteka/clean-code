using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Markdown
{
	class BacktickQuotes : ParserBase, IParsable
	{
		private readonly List<string> allSpecialSymbols = new List<string>{"_", "__"};
		public BacktickQuotes(string markdown) : base(markdown, "`")
		{
		}

		public BacktickQuotes() : base("`")
		{
		}

		private bool insideTagsScreened = false;
		public override Dictionary<TagType, string> Tags => AllTags.BacktickQuotes;
		public override void FillEntries()
		{
			var opening = TagType.Opening;
			for (var i = 0; i < Markdown.Length; i++)
			{
				if (Markdown[i]!='`')
					continue;
				//check for screened quotes
				if (i >= 1 && Markdown[i - 1] == '\\')
				{
					Screens.Add(i-1);
					continue;
				}
				Entries.Add(i, opening);
				opening = opening.Invert();
			}
			if (Entries.Count % 2 != 0)
				Entries.Remove(Entries.Keys.Max());
			if (!insideTagsScreened)
				ScreenInsideTags();
		}

		private void ScreenInsideTags()
		{
			var finalString = new StringBuilder(Markdown);
			for (var i = 0; i < Entries.Count; i += 2)
				foreach (var symbol in allSpecialSymbols)
				{
					var offset = 0;
					var regexpr = new Regex($@"(?<!\\|{symbol})({symbol})(?:[\p{{P}}\w\s-[{symbol}]])+?(?<!\\)({symbol})");
					var results = regexpr.Matches(finalString.ToString(), Entries.ElementAt(i).Key);
					foreach (var result in results)
					{
						var match = result as Match;
						finalString.Insert(match.Groups[1].Index + offset, "\\");
						finalString.Insert(match.Groups[2].Index + offset + 1, "\\");
						offset += 2;
					}
				}
			Entries.Clear();
			Screens.Clear();
			Markdown = finalString.ToString();
			insideTagsScreened = true;
			FillEntries();
		}

		public string Parse(string markdown)
		{
			Markdown = markdown;
			FillEntries();
			return Transform();
		}
	}
}
