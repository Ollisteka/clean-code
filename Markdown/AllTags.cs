using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
	static class AllTags
	{
		public static Dictionary<TagType, string> DoubleUnderscores { get; } = new Dictionary<TagType, string>
		{
			{TagType.Opening, "<strong>"},
			{TagType.Closing, "</strong>"}
		};
		public static Dictionary<TagType, string> SingleUnderscore { get; } = new Dictionary<TagType, string>
		{
			{TagType.Opening, "<em>"},
			{TagType.Closing, "</em>"}
		};
		public static Dictionary<TagType, string> BacktickQuotes { get; } = new Dictionary<TagType, string>
		{
			{TagType.Opening, "<code>"},
			{TagType.Closing, "</code>"}
		};
	}
}
