using System;

namespace FriendShip
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
            int players = 4;
            int resolution = 1080;
		    try
		    {
		        for (int i = 0; i < args.Length; i++)
		        {
		            if (args[i] == "-scale")
                        resolution = Int32.Parse(args[i + 1]);
                    if (args[i] == "-players")
                        players = Int32.Parse(args[i + 1]);
		        }
		    }
		    catch (Exception)
		    {
		        Console.WriteLine("options are -scale [height of screen in pixels] and -players [nb players <= 4]");
		        throw;
		    }

		    using (GameCore game = new GameCore(resolution, players))
			{
				game.Run();
			}
		}
	}
}
