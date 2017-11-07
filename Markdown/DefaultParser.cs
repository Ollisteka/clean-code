using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
	public abstract class DefaultParser
	{
		protected string Markdown;
		protected string SpecialSymbol;

		protected DefaultParser(string markdown, string specialSymbol)
		{
			Markdown = markdown;
			SpecialSymbol = specialSymbol;
		}

		protected DefaultParser(string specialSymbol)
		{
			SpecialSymbol = specialSymbol;
		}


		public abstract Dictionary<int, TagType> Entries { get; }

		public abstract List<int> Screens { get; }

		public abstract Dictionary<TagType, string> Tags { get; }

		public virtual void FillEntries()
		{
		}

		private int offset;
		public virtual string Transform()
		{
			var result = new StringBuilder(Markdown);
			if (Screens.Any())
				if (Screens.First() <= Entries.Keys.FirstOrDefault())
				{
					DeleteScreens(result);
					ReplaceTags(result);
				}
				else
				{
					ReplaceTags(result);
					DeleteScreens(result);
				}
			else ReplaceTags(result);
			return result.ToString();
		}

		private void ReplaceTags(StringBuilder result)
		{
			foreach (var entry in Entries)
			{
				result.Remove(entry.Key + offset, SpecialSymbol.Length);
				result.Insert(entry.Key + offset, Tags[entry.Value]);
				if (entry.Value == TagType.Opening)
					offset += Tags[TagType.Opening].Length - SpecialSymbol.Length;
				else offset += Tags[TagType.Closing].Length - SpecialSymbol.Length;
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