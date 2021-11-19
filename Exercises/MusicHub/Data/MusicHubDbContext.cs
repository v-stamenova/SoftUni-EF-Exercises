namespace MusicHub.Data
{
	using Microsoft.EntityFrameworkCore;
	using MusicHub.Data.Models;

	public class MusicHubDbContext : DbContext
	{
		public MusicHubDbContext()
		{
		}

		public MusicHubDbContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<Song> Songs { get; set; }
		public DbSet<Album> Albums { get; set; }
		public DbSet<Performer> Performers { get; set; }
		public DbSet<Producer> Producers { get; set; }
		public DbSet<Writer> Writers { get; set; }
		public DbSet<SongPerformer> SongsPerformers { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Server=LAPTOP-BCK1H973\\SQLEXPRESS;Database=StudentSystem;Integrated Security=True;");
			}

			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Song>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.Name)
					.HasMaxLength(20)
					.IsRequired();

				entity.Property(e => e.Duration)
					.IsRequired();

				entity.Property(e => e.CreatedOn)
					.IsRequired();

				entity.Property(e => e.Genre)
					.IsRequired();

				entity.Property(e => e.Price)
					.IsRequired();

				entity.HasOne(e => e.Album)
					.WithMany(a => a.Songs)
					.HasForeignKey(e => e.AlbumId);

				entity.HasOne(e => e.Writer)
					.WithMany(w => w.Songs)
					.HasForeignKey(e => e.WriterId);
			});

			builder.Entity<Album>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.Name)
					.HasMaxLength(40)
					.IsRequired();

				entity.Property(e => e.ReleaseDate)
					.IsRequired();

				entity.Property(e => e.Price);

				entity.HasOne(e => e.Producer)
					.WithMany(p => p.Albums)
					.HasForeignKey(e => e.ProducerId);
			});

			builder.Entity<Performer>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.FirstName)
					.HasMaxLength(20)
					.IsRequired();

				entity.Property(e => e.LastName)
					.HasMaxLength(20)
					.IsRequired();

				entity.Property(e => e.Age)
					.IsRequired();

				entity.Property(e => e.NetWorth)
					.IsRequired();
			});

			builder.Entity<Producer>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity.Property(e => e.Name)
					.HasMaxLength(30)
					.IsRequired();

				entity.Property(e => e.Pseudonym);
			});

			builder.Entity<SongPerformer>(entity =>
			{
				entity.HasKey(e => new { e.SongId, e.PerformerId });

				entity.HasOne(e => e.Song)
					.WithMany(s => s.SongPerformers)
					.HasForeignKey(e => e.SongId);

				entity.HasOne(e => e.Performer)
					.WithMany(p => p.PerformerSongs)
					.HasForeignKey(e => e.PerformerId);
			});
		}
	}
}
