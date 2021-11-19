namespace MusicHub
{
	using System;
	using System.Linq;
	using System.Text;
	using Data;
	using Initializer;

	public class StartUp
	{
		public static void Main(string[] args)
		{
			MusicHubDbContext context =
				new MusicHubDbContext();

			DbInitializer.ResetDatabase(context);
		}

		
	}
}
