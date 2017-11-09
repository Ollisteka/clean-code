using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
	public abstract class ParserBase
	{
		protected string Markdown;

		private int offset;
		private readonly string specialSymbol;

		protected ParserBase(string markdown, string specialSymbol)
		{
			Markdown = markdown;
			this.specialSymbol = specialSymbol;
		}

		protected ParserBase(string specialSymbol)
		{
			this.specialSymbol = specialSymbol;
		}


		public Dictionary<int, TagType> Entries { get; } = new Dictionary<int, TagType>();

		public List<int> Screens { get; } = new List<int>();

		public abstract Dictionary<TagType, string> Tags { get; }

		public abstract void FillEntries();

		public string Transform()
		{
			var result = new StringBuilder(Markdown);
			if (Screens.FirstOrDefault() < Entries.Keys.FirstOrDefault())
			{
				DeleteScreens(result);
				ReplaceTags(result);
			}
			else
			{
				ReplaceTags(result);
				DeleteScreens(result);
			}
			return result.ToString();
		}

		private void ReplaceTags(StringBuilder result)
		{
			foreach (var entry in Entries)
			{
				result.Remove(entry.Key + offset, specialSymbol.Length);
				result.Insert(entry.Key + offset, Tags[entry.Value]);
				if (entry.Value == TagType.Opening)
					offset += Tags[TagType.Opening].Length - specialSymbol.Length;
				else offset += Tags[TagType.Closing].Length - specialSymbol.Length;
			}
		}

		private void DeleteScreens(StringBuilder result)
		{
			foreach (var position in Screens)
			{
				result.Remove(position + offset, 1);
				offset -= 1;
			}
		}
	}
}