using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBDemo.Entities;
using NUnit.Framework;

namespace MongoDBDemo
{
    [TestFixture]
    public class Tasks
    {
        private IMongoCollection<Book> books;

        [SetUp]
        public void Setup()
        {
            var client = new MongoClient();
            var db = client.GetDatabase("Demo");
            books = db.GetCollection<Book>("Books");
            InitBooksCollection();
        }

        private void InitBooksCollection()
        {
            if (books.Count(FilterDefinition<Book>.Empty) == 0) {
                books.InsertMany(new[] 
                {
                    new Book 
                    {
                        Name = "Hobbit",
                        Author = "Tolkien",
                        Count = 5,
                        Genre = new[] { "fantasy" },
                        Year = 2014
                    },
                    new Book 
                    {
                        Name = "Lord of the rings",
                        Author = "Tolkien",
                        Count = 3,
                        Genre = new[] { "fantasy" },
                        Year = 2015
                    },
                    new Book 
                    {
                        Name = "Kolobok",
                        Count = 10,
                        Genre = new[] { "kids" },
                        Year = 2000
                    },
                    new Book 
                    {
                        Name = "Repka",
                        Count = 11,
                        Genre = new[] { "kids" },
                        Year = 2000
                    },
                    new Book 
                    {
                        Name = "Dyadya Stiopa",
                        Author = "Mihalkov",
                        Count = 1,
                        Genre = new[] { "kids" },
                        Year = 2001
                    } 
                });
            }
        }

        [TearDown]
        public void TearDown()
        {
            books.DeleteMany(Builders<Book>.Filter.Empty);
        }

        [Test]
        public void BooksWithCountMoreThanOne()
        {
            var booksWithCountMoreThanOne = books.Find(b => b.Count > 1).ToList();
            Assert.AreEqual(4, booksWithCountMoreThanOne.Count);

            var bookNames = booksWithCountMoreThanOne.Select(b => new { name = b.Name }).ToList();

            var sorted = books.Find(b => b.Count > 1).SortBy(b => b.Name).ToList();

            var limit = books.Find(b => b.Count > 1).Limit(3).ToList();
            Assert.AreEqual(limit.Count, 3);

            var booksCount = booksWithCountMoreThanOne.Aggregate(0, (x, y) => x + y.Count);
            Assert.AreEqual(29, booksCount);
        }

        [Test]
        public void BooksWithMaxAndMinCount()
        {
            var bookWithMaxCount = books.Find(b => b.Count > 0).SortByDescending(b => b.Count).FirstOrDefault();
            Assert.AreEqual("Repka", bookWithMaxCount.Name);

            var bookWithMinCount = books.Find(b => b.Count > 0).SortBy(b => b.Count).FirstOrDefault();
            Assert.AreEqual("Dyadya Stiopa", bookWithMinCount.Name);
        }

        [Test]
        public void AuthorList()
        {
            var authors = books.Distinct<string>("Author", new BsonDocument()).ToList();
            Assert.AreEqual(3, authors.Count);
        }

        [Test]
        public void BooksWithoutAuthor()
        {
            var booksWithoutAuthor = books.Find(b => b.Author == null);
            Assert.AreEqual(2, booksWithoutAuthor.Count());
        }

        [Test]
        public void BooksCountIncByOne()
        {
            books.UpdateMany(FilterDefinition<Book>.Empty, Builders<Book>.Update.Inc(b => b.Count, 1));
            Assert.AreEqual(2, books.Find(b => b.Name == "Dyadya Stiopa").First().Count);
        }

        [Test]
        public void AddGenre()
        {
            books.UpdateMany(Builders<Book>.Filter.Where(b => b.Genre.Any(g => g == "fantasy")),
                Builders<Book>.Update.AddToSet(b => b.Genre, "favority"));

            Assert.Contains("favority", books.Find(Builders<Book>.Filter.Where(b => b.Genre.Any(g => g == "fantasy"))).First().Genre);
        }

        [Test]
        public void RemoveBooks()
        {
            books.DeleteMany(Builders<Book>.Filter.Lt(b => b.Count, 3));
            Assert.AreEqual(4, books.Find(b => b.Count > 1).Count());
        }
    }
}
