using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
	public class SingleUnderscore : ParserBase, IParsable
	{
		public SingleUnderscore(string markdown) : base(markdown, "_")
		{
		}

		public SingleUnderscore() : base("_")
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
			for (var i = 0; i < Markdown.Length; i++)
			{
				if (Markdown[i] != '_')
					continue;
				// check for double underscores
				if (i + 1 < Markdown.Length && Markdown[i + 1] == '_')
				{
					i += 1;
					continue;
				}
				// check for screened underscores
				if (i >= 1 && Markdown[i - 1] == '\\')
				{
					Screens.Add(i - 1);
					continue;
				}
				// check for space before closing underscore
				if (opening == TagType.Closing && Markdown[i - 1] == ' ')
				{
					Entries.Remove(Entries.Keys.Max());
					opening = TagType.Opening;
				}
				// check for space after opening underscore
				if (opening == TagType.Opening && i + 1 < Markdown.Length && Markdown[i + 1] == ' ')
					continue;
				// check for numbers inside
				if (opening == TagType.Closing)
				{
					var lastElement = Entries.Keys.Max();
					if (Markdown.Substring(lastElement, i - lastElement).Any(char.IsDigit))
					{
						Entries.Remove(lastElement);
						opening = TagType.Opening;
					}
				}
				Entries.Add(i, opening);
				opening = opening.Invert();
			}
			if (Entries.Count % 2 != 0)
				Entries.Remove(Entries.Keys.Max());
		}
	}
}