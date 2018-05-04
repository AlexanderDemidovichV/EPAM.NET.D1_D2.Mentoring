using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using MongoDBDemo.Entities;

namespace MongoDBDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MongoDB.Driver.MongoClient();
            var db = client.GetDatabase("Demo");
            var col = db.GetCollection<Book>("Books");

            var id = new ObjectId("5aeb4b91013d041414d95fe9");
            var books = col.Find(b => b.Id == id).Limit(5).ToListAsync().Result;

            #region Init

            if (col.Count(FilterDefinition<Book>.Empty) == 0) {
                col.InsertMany(new[] { new Book {
                    Name = "Hobbit",
                    Author = "Tolkien",
                    Count = 5,
                    Genre = new[] { "fantasy" },
                    Year = 2014
                }, new Book() {
                    Name = "Lord of the rings",
                    Author = "Tolkien",
                    Count = 3,
                    Genre = new[] { "fantasy" },
                    Year = 2015
                }, new Book() {
                    Name = "Kolobok",
                    Count = 10,
                    Genre = new[] { "kids" },
                    Year = 2000
                }, new Book() {
                    Name = "Repka",
                    Count = 11,
                    Genre = new[] { "kids" },
                    Year = 2000
                }, new Book() {
                    Name = "Dyadya Stiopa",
                    Author = "Mihalkov",
                    Count = 1,
                    Genre = new[] { "kids" },
                    Year = 2001
                } });
            }

            #endregion




            //

            var booksWithCountMoreThanOne = col.Find(b => b.Count > 1).ToListAsync().Result;
            var result1 = booksWithCountMoreThanOne.Select(b => new {name = b.Name});
            foreach (var book in result1) {
                Console.WriteLine(book.name);
            }

            Console.WriteLine();
            var result2 = col.Find(b => b.Count > 1).SortBy(b => b.Name).ToListAsync().Result;
            foreach (var book in result2) {
                Console.WriteLine(book.Name);
            }

            Console.WriteLine();
            var result3 = col.Find(b => b.Count > 1).Limit(3).ToListAsync().Result;
            foreach (var book in result3) {
                Console.WriteLine(book.Name);
            }

            Console.WriteLine();
            var result4 = col.Find(b => b.Count > 1).ToListAsync().Result.Aggregate(0, (x, y) => x + y.Count);
            Console.WriteLine(result4);

            Console.WriteLine();
            var result3_1_1 = col.Find(b => b.Count > 0).SortByDescending(b => b.Count).FirstOrDefault();
            Console.WriteLine(result3_1_1.Name);

            Console.WriteLine();
            var result3_1_2 = col.Find(b => b.Count > 0).SortBy(b => b.Count).FirstOrDefault();
            Console.WriteLine(result3_1_2.Name);

            Console.WriteLine();
            var result44 = col.DistinctAsync<string>("Author", new BsonDocument()).Result.ToList();
            foreach (var author in result44) {
                Console.WriteLine(author);
            }

            Console.WriteLine();
            var result5 = col.Find(b => b.Author == null).ToListAsync().Result;
            foreach (var book in result5) {
                Console.WriteLine(book.Name);
            }

            col.UpdateMany(FilterDefinition<Book>.Empty, Builders<Book>.Update.Inc(b => b.Count, 1));


            var ttt = col.Find(Builders<Book>.Filter.Where(b => b.Genre.Any(g => g == "fantasy"))).ToListAsync().Result;

            col.UpdateMany(Builders<Book>.Filter.Where(b => b.Genre.Any(g => g == "fantasy")), 
                Builders<Book>.Update.AddToSet(b => b.Genre, "favority"));


            col.DeleteMany(Builders<Book>.Filter.Lt(b => b.Count, 3));

            col.DeleteMany(Builders<Book>.Filter.Empty);

            //foreach (var book in col.up) {
            //    Console.WriteLine(book.Name);
            //}
        }
    }
}
