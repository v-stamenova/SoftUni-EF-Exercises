namespace BookShop
{
    using Data;
    using Initializer;
	using System;
	using System.Linq;

	public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
        }

		public static string GetBooksByAgeRestriction(BookShopContext context, string command)
		{
			var booksWithRestriction = context.Books
					.Select(b => new
					{
						b.Title,
						b.AgeRestriction
					})
					.OrderBy(b => b.Title)
					.ToList()
					.Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower());

			return string.Join(Environment.NewLine, booksWithRestriction.Select(x => x.Title).ToList());
		}

		public static string GetGoldenBooks(BookShopContext context)
		{
			var goldenEdition = context.Books
					.Select(b => new
					{
						b.Title,
						b.BookId,
						b.EditionType,
						b.Copies
					})
					.OrderBy(b => b.BookId)
					.ToList()
					.Where(b => b.Copies < 5000 && b.EditionType.ToString() == "Gold");

			return string.Join(Environment.NewLine, goldenEdition.Select(x => x.Title).ToList());
		}
		public static string GetBooksByPrice(BookShopContext context)
		{
			var books = context.Books
				.Where(b => b.Price > 40)
				.Select(b => new
				{
					b.Title,
					b.Price
				})
				.OrderByDescending(b => b.Price)
				.ToList();

			return string.Join(Environment.NewLine, books.Select(x => $"{x.Title} - ${x.Price}"));
		}

	}
}
