namespace Markdown
{
	public interface ITransformable
	{
		void FillEntries();
		string Transform();
	}
}