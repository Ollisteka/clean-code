using System.Linq;
using System.Text.RegularExpressions;

namespace Markdown
{
	internal class DoubleUnderscore : ParserBase, IParsable
	{
		public DoubleUnderscore(string markdown) : base(markdown, "__")
		{
		}

		public DoubleUnderscore() : base("__")
		{
		}

		public string Parse(string markdown)
		{
			Markdown = markdown;
			FillEntries();
			return Transform();
		}

		public override void FillEntries()
		{
			var opening = TagType.Opening;
			for (var i = 0; i < Markdown.Length; i += 1)
			{
				if (i + 2 < Markdown.Length
					&& Markdown[i] == '\\'
					&& Markdown[i + 1] == '_'
					&& Markdown[i + 2] == '_')
				{
					Screens.Add(i);
					i += 2;
					continue;
				}
				if (i + 1 >= Markdown.Length || Markdown[i] != '_' || Markdown[i + 1] != '_') continue;
				if (Entries.Count != 0 && Entries.Keys.Max() + 2 == i)
				{
					Entries.Add(i, opening.Invert());
					i += 1;
					continue;
				}
				Entries.Add(i, opening);
				opening = opening.Invert();
				i += 1;
			}
			if (Entries.Count % 2 != 0)
				Entries.Remove(Entries.Keys.Max());
			CheckForDoubleUnderscoresInsideSingle();
		}

		private void CheckForDoubleUnderscoresInsideSingle()
		{
			var openingTag = ParserTags["_"][TagType.Opening];
			var closingTag = ParserTags["_"][TagType.Closing];
			var regexp = new Regex(openingTag + ".*" + closingTag);
			var results = regexp.Matches(Markdown);
			if (results.Count == 0)
				return;

			for (var position = 0; position < Entries.Keys.Count; position += 2)
			{
				var openingUnderscoreIndex = Entries.ElementAt(position).Key;
				var closingUnderscoreIndex = Entries.ElementAt(position + 1).Key;
				foreach (var result in results)
				{
					var match = result as Match;
					var startIndex = match.Index;
					var lastIndex = startIndex + match.Length - 1;
					if (startIndex > openingUnderscoreIndex || lastIndex < closingUnderscoreIndex) continue;
					Entries.Remove(openingUnderscoreIndex);
					Entries.Remove(closingUnderscoreIndex);
				}
			}
		}
	}
}