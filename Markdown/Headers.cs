using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
	internal class Headers : ITransformable
	{
		public static readonly Dictionary<string, string> OpeningTags = new Dictionary<string, string>
		{
			{"</h1>", "<h1>"},
			{"</h2>", "<h2>"}
		};

		private readonly Dictionary<char, string> closingTags = new Dictionary<char, string>
		{
			{'=', "</h1>"},
			{'-', "</h2>"}
		};

		private bool correctHeader;
		private char key;

		private string markdown;

		public Headers()
		{
		}

		public Headers(string markdown)
		{
			this.markdown = markdown;
		}

		public void SetMarkdown(string markdownValue)
		{
			if (markdown is null)
				markdown = markdownValue;
			else
				throw new Exception("The markdownValue value was set in constructor!");
		}

		public void FillEntries()
		{
			correctHeader = markdown.All(symbol => symbol == '=')
			                || markdown.All(symbol => symbol == '-');
			key = markdown[0];
		}

		public string Transform()
		{
			return correctHeader ? closingTags[key] : markdown;
		}
	}
}