﻿using System.Runtime.InteropServices;

namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using BookShop.Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                //DbInitializer.ResetDatabase(db);

                string input = Console.ReadLine();

                string result = GetBooksByPrice(db);

                Console.WriteLine(result);
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            //var age = Enum.Parse<AgeRestriction>(command, true);

            StringBuilder sb = new StringBuilder();

            var books = context
                .Books
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(b => new
                {
                    b.Title
                })
                .OrderBy(b=>b.Title)
                .ToList();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }


            return sb.ToString().TrimEnd();


        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Select(b=>new
                {
                    b.EditionType,
                    b.Copies,
                    b.BookId,
                    b.Title
                })
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(b => b.BookId)
                .ToList();


            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }


            return sb.ToString().TrimEnd();










            //var books = context.Books
            //    .Select(b => new
            //    {
            //        b.EditionType,
            //        b.Copies,
            //        b.BookId,
            //        b.Title
            //    })
            //    .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
            //    .OrderBy(b => b.BookId)
            //    .ToList();

            //StringBuilder sb = new StringBuilder();

            //foreach (var book in books)
            //{
            //    sb.AppendLine(book.Title);
            //}

            //return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var prices = context
                .Books
                .Select(p => new
                {
                    p.Price,
                    p.Title
                })
                .Where(p => p.Price > 40)
                .OrderByDescending(p=>p.Price)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var price in prices)
            {
                sb.AppendLine($"{price.Title} - ${price.Price:F2}");
            }


            return sb.ToString().TrimEnd();

        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Select(b => new
                {
                    b.ReleaseDate,
                    b.BookId,
                    b.Title
                })
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            List<string> booksTitles = new List<string>();


            foreach (var categori in categories)
            {

                var books = context
                    .Books
                    .Where(b => b.BookCategories
                        .Any(c => c.Category.Name == input))
                    .Select(a => new
                    {
                        a.Title
                    })
                    .ToList();

                foreach (var book in books)
                {
                    booksTitles
                }


            }




            //foreach (var item in items)
            //{
            //    var books = context.Books
            //        .Where(b => b.BookCategories.Any(c => c.Category.Name.ToLower() == item.ToLower()))
            //        .Select(b => new
            //        {
            //            b.Title
            //        })

            // dto 

            //        .ToList();

            //    foreach (var book in books)
            //    {
            //        booksTitles.Add(book.Title);
            //    }
            //}

            //StringBuilder sb = new StringBuilder();

            //foreach (var title in booksTitles.OrderBy(x => x))
            //{
            //    sb.AppendLine(title);
            //}

            //return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dateTime = DateTime.ParseExact(date, "dd-MM-yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

            var books = context.Books
                .Select(b => new
                {
                    b.ReleaseDate,
                    b.Title,
                    b.Price,
                    b.EditionType
                })
                .Where(b => b.ReleaseDate < dateTime)
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName
                })
                .OrderBy(a => a.FullName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine(author.FullName);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Title.IndexOf(input, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(b => new
                {
                    b.Title
                })
                .OrderBy(b => b.Title)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Author.LastName.StartsWith(input, StringComparison.OrdinalIgnoreCase))
                .Select(b => new
                {
                    b.BookId,
                    b.Title,
                    AuthorName = b.Author.FirstName + " " + b.Author.LastName
                })
                .OrderBy(b => b.BookId)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            return context.Books.Where(b => b.Title.Length > lengthCheck).Count();
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    AuthorName = a.FirstName + " " + a.LastName,
                    Count = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(b => b.Count)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.AuthorName} - {author.Count}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Money = c.CategoryBooks.Sum(i => i.Book.Price * i.Book.Copies)
                })
                .OrderByDescending(b => b.Money)
                .ThenBy(c => c.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"{category.Name} ${category.Money:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var items = context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new
                {
                    CategoryName = c.Name,
                    BooksNames = c.CategoryBooks
                    .Select(b => new
                    {
                        b.Book.Title,
                        b.Book.ReleaseDate,
                        b.Book.ReleaseDate.Value.Year
                    })
                    .OrderByDescending(b => b.ReleaseDate)
                    .Take(3)
                    .ToList()
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var category in items)
            {
                sb.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.BooksNames)
                {
                    sb.AppendLine($"{book.Title} ({book.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books.Where(b => b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books.Where(b => b.Copies < 4200);
            int count = books.Count();

            foreach (var book in books)
            {
                context.Books.Remove(book);
            }

            context.SaveChanges();

            return count;
        }
    }
}