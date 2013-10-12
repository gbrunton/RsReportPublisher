using System;
using FubuCore.CommandLine;

namespace Publisher
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args == null || args.Length == 0)
			{
				args = new []
					{
						"publish"
					};
			}

			bool success;
			try
			{
				var factory = new CommandFactory();
				factory.SetAppName("ssaportal");
				factory.RegisterCommands(typeof(IFubuCommand).Assembly);
				factory.RegisterCommands(typeof(Program).Assembly);

				var executor = new CommandExecutor(factory);
				success = executor.Execute(args);
			}
			catch (CommandFailureException e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("ERROR: " + e.Message);
				Console.ResetColor();
				success = false;
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("ERROR: " + ex);
				Console.ResetColor();
				success = false;
			}

			return success ? 0 : 1;
		}
	}
}