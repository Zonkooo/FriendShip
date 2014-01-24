namespace FriendShip
{
	static class Program
	{
		static void Main(string[] args)
		{
			using (GameCore game = new GameCore())
			{
				game.Run();
			}
		}
	}
}
