using System;
using System.IO;
using System.Linq;

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
			var filepath = new FileInfo(args[0]);
			var renderer = new Md();
			var result = renderer.RenderToHtml(File.ReadAllText(filepath.FullName));
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