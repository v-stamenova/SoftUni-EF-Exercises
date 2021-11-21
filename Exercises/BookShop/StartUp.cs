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
	}
}
