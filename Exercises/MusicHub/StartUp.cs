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

		public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
		{
			StringBuilder builder = new StringBuilder();

			var albums = context.Albums
				.Where(x => x.ProducerId == producerId)
				.OrderByDescending(x => x.Price)
				.Select(x => new
				{
					AlbumName = x.Name,
					x.ReleaseDate,
					ProducerName = x.Producer.Name,
					AlbumPrice = x.Price,
					Songs = x.Songs
						.Where(s => s.Album.Id == x.Id)
						.Select(s => new
						{
							SongName = s.Name,
							s.Price,
							WriterName = s.Writer.Name
						})
						.OrderByDescending(s => s.SongName)
						.ThenBy(s => s.WriterName)
						.ToList()
				});

			foreach (var album in albums)
			{
				builder.AppendLine($"-AlbumName: {album.AlbumName}");
				builder.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM'/'dd'/'yyyy")}");
				builder.AppendLine($"-ProducerName: {album.ProducerName}");
				builder.AppendLine($"-Songs:");

				int count = 1;
				foreach (var song in album.Songs)
				{
					builder.AppendLine($"---#{count}");
					builder.AppendLine($"---SongName: {song.SongName}");
					builder.AppendLine($"---Price: {song.Price.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}");
					builder.AppendLine($"---Writer: {song.WriterName}");
					count++;
				}

				builder.AppendLine($"-AlbumPrice: {album.AlbumPrice.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)}");
			}

			return builder.ToString();
		}
	}
}
