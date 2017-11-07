using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
	internal class Headers : IParsable
	{
		private string markdown;
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


		public Headers()
		{
		}

		public Headers(string markdown)
		{
			this.markdown = markdown;
		}

		public string Parse(string markdown)
		{
			this.markdown = markdown;
			FillEntries();
			return Transform();
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