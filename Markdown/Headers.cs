using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	class Headers : ITransformable
	{
		private readonly Dictionary<char, string> closingTags = new Dictionary<char, string>
		{
			{'=', "</h1>"},
			{'-', "</h2>"},
		};
		public static readonly Dictionary<string, string> OpeningTags = new Dictionary<string, string>
		{
			{"</h1>", "<h1>"},
			{"</h2>", "<h2>"},
		};

		private readonly string markdown;
		private bool correctHeader;
		private char key;

		public Headers(string markdown)
		{
			this.markdown = markdown;
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
