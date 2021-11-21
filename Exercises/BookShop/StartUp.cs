namespace BookShop
{
    using Data;
    using Initializer;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

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

		public static string GetBooksNotReleasedIn(BookShopContext context, int year)
		{
			var booksNotReleased = context.Books
					.Select(b => new
					{
						b.Title,
						b.BookId,
						b.ReleaseDate
					})
					.ToList()
					.Where(b => b.ReleaseDate.Value.Year != year)
					.OrderBy(b => b.BookId);

			return string.Join(Environment.NewLine, booksNotReleased.Select(x => x.Title));
		}

		public static string GetBooksByCategory(BookShopContext context, string input)
		{
			List<string> categories = input.ToLower().Split(' ').ToList();

			var booksTitles = context.Books
				.Where(b => b.BookCategories
					.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
				.OrderBy(b => b.Title)
				.Select(b => b.Title)
				.ToList();

			return string.Join(Environment.NewLine, booksTitles);
		}

		public static string GetBooksReleasedBefore(BookShopContext context, string date)
		{
			DateTime releaseDate = DateTime.ParseExact(date, "dd-MM-yyyy", null);

			var books = context.Books
				.Where(b => b.ReleaseDate < releaseDate)
				.OrderByDescending(b => b.ReleaseDate)
				.Select(b => new { b.Title, b.EditionType, b.Price })
				.ToList();

			return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}"));
		}

		public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
		{
			var authorsNames = context.Authors
					.Where(x => x.FirstName.EndsWith(input))
					.Select(x => new
					{
						x.FirstName,
						x.LastName
					})
					.OrderBy(x => x.FirstName)
					.ThenBy(x => x.LastName)
					.ToList();

			return string.Join(Environment.NewLine, authorsNames.Select(a => $"{a.FirstName} {a.LastName}"));
		}

		public static string GetBookTitlesContaining(BookShopContext context, string input)
		{
			var books = context.Books
					.Where(b => b.Title.ToLower().Contains(input.ToLower()))
					.Select(b => b.Title)
					.OrderBy(b => b)
					.ToList();

			return string.Join(Environment.NewLine, books);
		}

		public static string GetBooksByAuthor(BookShopContext context, string input)
		{
			var books = context.Books
				.Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
				.Select(b => new
				{
					b.Title,
					b.Author.FirstName,
					b.Author.LastName,
					b.BookId
				})
				.OrderBy(b => b.BookId)
				.ToList();

			return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} ({b.FirstName} {b.LastName})"));
		}

		public static int CountBooks(BookShopContext context, int lengthCheck)
		{
			int count = context.Books
				.Where(b => b.Title.Length > lengthCheck)
				.Count();

			return count;
		}

		public static string CountCopiesByAuthor(BookShopContext context)
		{
			var authors = context.Authors
				.Select(a => new
				{
					a.FirstName,
					a.LastName,
					BooksCount = a.Books.Sum(x => x.Copies)
				})
				.OrderByDescending(x => x.BooksCount)
				.ToList();

			return string.Join(Environment.NewLine, authors.Select(x => $"{x.FirstName} {x.LastName} - {x.BooksCount}"));
		}

		public static string GetTotalProfitByCategory(BookShopContext context)
		{
			var catProfit = context.Categories
				.Select(c => new
				{
					c.Name,
					Profit = c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
				})
				.OrderByDescending(c => c.Profit)
				.ThenBy(c => c.Name)
				.ToList();

			return string.Join(Environment.NewLine, catProfit.Select(x => $"{x.Name} ${x.Profit.ToString("0.00")}"));
		}

		public static string GetMostRecentBooks(BookShopContext context)
		{
			var catBooks = context.Categories
				.Select(c => new
				{
					c.Name,
					RecentBooks = c.CategoryBooks.Select(b => new
					{
						b.Book.Title,
						b.Book.ReleaseDate
					})
					.OrderByDescending(x => x.ReleaseDate).Take(3)
				})
				.OrderBy(c => c.Name)
				.ToList();

			StringBuilder builder = new StringBuilder();

			foreach (var cat in catBooks)
			{
				builder.AppendLine($"--{cat.Name}");

				foreach (var book in cat.RecentBooks)
				{
					builder.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
				}
			}

			return builder.ToString();
		}
	}
}
