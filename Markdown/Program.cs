using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Markdown
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			if (!args.Any())
			{
				Console.WriteLine("Please, specify input file path!\n" +
				                  "You can also specify output file as a second argument");
				Environment.Exit(0);
			}
			var filepath = args[0];
			var f = new FileInfo(filepath);
			var markdown = new StringBuilder();
			using (var reader = new StreamReader(f.FullName))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
					markdown.Append(line + "\n");
			}
			var renderer = new Md();
			var result = renderer.RenderToHtml(markdown.ToString());
			if (args.Length == 1)
				Console.Write(result);
			else
			{
				var output = args[1];
				File.WriteAllText(output, result);
			}
		}
	}
}