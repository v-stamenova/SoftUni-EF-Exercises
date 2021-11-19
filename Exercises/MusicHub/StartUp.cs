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


		public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
		{
			StringBuilder builder = new StringBuilder();

			var songs = context.Songs
					.Select(s => new
					{
						s.Name,
						WriterName = s.Writer.Name,
						ProducerName = s.Album.Producer.Name,
						s.Duration,
						Performers = s.SongPerformers
							.Select(p => new
							{
								p.Performer.FirstName,
								p.Performer.LastName
							})
							.ToList()
					})
					.ToList()
					.OrderBy(x => x.Name)
					.ThenBy(x => x.WriterName)
					.ThenBy(x => x.Performers.FirstOrDefault() is null ? null : x.Performers.First().FirstName)
					.Where(x => IsItLonger(duration, x.Duration.TotalMinutes));

			int count = 1;
			foreach (var song in songs)
			{
				var Performer = song.Performers.FirstOrDefault();

				builder.AppendLine($"-Song #{count}");
				builder.AppendLine($"---SongName: {song.Name}");
				builder.AppendLine($"---Writer: {song.WriterName}");
				if (!(Performer is null))
				{
					builder.AppendLine($"---Performer: {Performer.FirstName} {Performer.LastName}");
				}
				else
				{
					builder.AppendLine($"---Performer: ");
				}

				builder.AppendLine($"---AlbumProducer: {song.ProducerName}");
				builder.AppendLine($"---Duration: {song.Duration.ToString("c")}");
				count++;
			}

			return builder.ToString();
		}

		private static bool IsItLonger(int duration, double songDuration)
		{
			if (duration < songDuration)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
