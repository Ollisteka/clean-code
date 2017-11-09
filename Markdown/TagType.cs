namespace Markdown
{
	public enum TagType
	{
		Opening,
		Closing
	}

	public static class TagTypeExtensions
	{
		public static TagType Invert(this TagType tag)
		{
			return (TagType)((int)(tag + 1) % 2);
		}
	}
}