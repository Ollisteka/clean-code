namespace Markdown
{
	public interface ITransformable
	{
		void SetMarkdown(string markdownValue);
		void FillEntries();
		string Transform();
	}
}