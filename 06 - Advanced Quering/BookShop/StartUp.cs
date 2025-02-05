namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Castle.DynamicProxy.Generators;
    using Data;
    using Initializer;
    using System.Text;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            Console.WriteLine(GetMostRecentBooks(db));
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var age = Enum.Parse<AgeRestriction>(command, true);

            var output = context.Books
                .Where(b => b.AgeRestriction == age)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            return String.Join("\n", output);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenEdition = EditionType.Gold;

            var wantedBooks = context.Books
                .ToList()
                .OrderBy(b => b.BookId)
                .Where(b => b.EditionType == goldenEdition
                && b.Copies < 5000)
                .Select(b => b.Title)
                .ToList();

            return String.Join("\n", wantedBooks);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .ToList()
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    BTitle = b.Title,
                    BPrice = b.Price,
                })
                .ToList();

            var sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.BTitle} - {book.BPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var output = context.Books
                .ToList()
                .OrderBy(b => b.BookId)
                //.Where(b => b.ReleaseDate.Year != year)
                .Select(b => b.Title)
                .ToList();

            return String.Join("\n", output);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.Split(" ")
                .Select(x => x.ToLower().Trim())
                .ToList();

            List<string> bookOutput = new();

            foreach(var category in categories)
            {
                var books = context.Books
                    .Where(b => b.BookCategories
                        .Any(c => c.Category.Name.ToLower() == category))
                    .Select(b => b.Title)
                    .ToList();

                foreach(var book in books)
                {
                    bookOutput.Add(book);
                }
            }

            bookOutput.Sort();

            return String.Join("\n", bookOutput.ToList());
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var tokenDate = date.Split("-").Select(x => int.Parse(x)).ToList();
            var wantedDate = new DateTime(tokenDate[2], tokenDate[1], tokenDate[0]);

            var books = context.Books
                .ToList()
                .Where(x => DateTime.Compare(x.ReleaseDate.Value, wantedDate) < 1)
                .OrderByDescending(x => x.ReleaseDate.Value.Date)
                .Select(x => new
                {
                    Title = x.Title,
                    EditionType = x.EditionType.ToString(),
                    Price = x.Price,
                })
                .ToList();

            string output = "";
            foreach( var book in books)
            {
                output += book.Title + " - " + book.EditionType + " - $" + $"{book.Price:f2}" + "\n";
            }

            return output;
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var output = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(x => x.FirstName + " " + x.LastName)
                .ToList();

            return String.Join("\n", output);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var output = context.Books
                .Where(a => a.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title)
                .ToList();

            return String.Join("\n", output);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(x => new
                {
                    FullName = x.FirstName + " " + x.LastName,
                    BooksName = x.Books.Select(b => b.Title).ToList()
                })
                .ToList();

            string realOut = "";

            foreach(var author in authors)
            {
                foreach(var book in author.BooksName)
                {
                    realOut += $"{book} ({author.FullName})\n";
                }
            }

            return realOut;
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new { BookCount = a.Books.Select(b => b.Copies).Sum(), AuthorName = a.FirstName + " " + a.LastName })
                .OrderByDescending(b => b.BookCount)
                .ToList();

            string output = "";
            foreach(var author in authors)
            {
                output += $"{author.AuthorName} - {author.BookCount}\n";
            }

            return output;
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new {CatName = c.Name, Price = c.CategoryBooks.Sum(x => x.Book.Price * x.Book.Copies)})
                .OrderByDescending(b => b.Price)
                .ThenBy(b => b.CatName)
                .ToList();

            string output = "";
            foreach (var category in categories)
            {
                output += $"{category.CatName} ${category.Price:f2}\n";
            }

            return output;
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var booksByCat = context.Categories
                .ToList()
                .Select(c => new
                {
                    CatName = c.Name,
                    Books = c.CategoryBooks
                                .Select(cb => new {
                                    BookName = cb.Book.Title,
                                    Release = cb.Book.ReleaseDate.Value.Year })
                                .ToList()
                                .OrderByDescending(b => b.Release)
                                .Take(3)
                })
                .OrderBy(c => c.CatName)
                .ToList();

            var output = "";

            foreach(var category in booksByCat)
            {
                output += $"--{category.CatName}\n";
                foreach(var book in category.Books)
                {
                    output += $"{book.BookName} ({book.Release})\n";
                }
            }

            return output;
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach(var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            foreach(var book in books)
            {
                context.Books.Remove(book);
            }

            return books.Count;
        }
    }
}


