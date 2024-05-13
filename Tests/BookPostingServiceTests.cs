using Moq;
using Data.Models;
using Data;
using Business.Services;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class BookPostingServiceTests
{
    List<BookPosting> testData;
    Mock<PostgresContext> mockContext;
    BookPostingService service;

    public BookPostingServiceTests()
    {
        testData = new List<BookPosting>
        {
            new()
            {
                Id = 1,
                OwnerId = "owner1",
                BookId = 1, Book = new Book { Name = "Book 1", Author = "Author 1" },
                Address = new Address { City = "City 1", State = "State 1" },
                Description = "Description 1",
                PostDateTime = DateTime.Now,
                ExpireDateTime = DateTime.Now.AddDays(7)
            },
            new()
            {
                Id = 2,
                OwnerId = "owner2",
                BookId = 2,
                Book = new Book { Name = "Book 2", Author = "Author 2" },
                Address = new Address { City = "City 2", State = "State 2" },
                Description = "Description 2",
                PostDateTime = DateTime.Now,
                ExpireDateTime = DateTime.Now.AddDays(7)
            },
            new()
            {
                Id = 3,
                OwnerId = "owner3",
                BookId = 3,
                Book = new Book { Name = "Book 3", Author = "Author 1" },
                Address = new Address { City = "City 3", State = "State 3" },
                Description = "Description 3",
                PostDateTime = DateTime.Now,
                ExpireDateTime = DateTime.Now.AddDays(7)
            }
        };

        mockContext = new Mock<PostgresContext>();
        InitMockMethods();
        service = new BookPostingService(mockContext.Object);
    }

    private void InitMockMethods()
    {
        var mockDbSet = new Mock<DbSet<BookPosting>>();
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.Provider).Returns(testData.AsQueryable().Provider);
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.Expression).Returns(testData.AsQueryable().Expression);
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.ElementType).Returns(testData.AsQueryable().ElementType);
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.GetEnumerator()).Returns(testData.AsQueryable().GetEnumerator());

        mockDbSet.Setup(m => m.Find(It.IsAny<object[]>()))
                 .Returns<object[]>(ids => testData.FirstOrDefault(t => t.Id == (int)ids[0]));

        mockDbSet.Setup(m => m.Add(It.IsAny<BookPosting>()))
                 .Callback<BookPosting>(bookPosting => testData.Add(bookPosting));

        mockDbSet.Setup(m => m.Update(It.IsAny<BookPosting>()))
                 .Callback<BookPosting>(bookPosting =>
                 {
                     var index = testData.FindIndex(t => t.Id == bookPosting.Id);
                     testData[index] = bookPosting;
                 });

        // mockDbSet.Setup(m => m.Include(It.IsAny<string>()))
        //          .Returns(mockDbSet.Object);

        mockDbSet.Setup(m => m.Remove(It.IsAny<BookPosting>()))
                 .Callback<BookPosting>(bookPosting => testData.Remove(bookPosting));

        mockContext.Setup(c => c.BookPostings).Returns(mockDbSet.Object);

        mockContext.Setup(c => c.Set<BookPosting>()).Returns(mockDbSet.Object);

        mockContext.Setup(c => c.SaveChanges()).Returns(1);
    }

    [Fact]
    public void GetById_ValidId_ReturnsBookPosting()
    {
        // Act
        var result = service.GetById(2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result?.Id);
    }

    [Fact]
    public void GetByOwnerId_ValidOwnerId_ReturnsBookPostings()
    {
        var ownerId = "owner1";
        // Act
        var result = service.GetByOwnerId(ownerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count()); // Should return 2 postings for owner1
    }

    [Fact]
    public void Add_ValidBookPosting_ReturnsAddedBookPosting()
    {
        var newBookPosting = new BookPosting { Id = 4, OwnerId = "owner4", BookId = 4 };
        testData.Add(newBookPosting);

        // Act
        var result = service.Add(newBookPosting);

        Console.WriteLine(testData.Count);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Update_ValidBookPosting_ReturnsUpdatedBookPosting()
    {
        // Arrange
        var testData = new List<BookPosting>
        {
            new BookPosting { Id = 1, OwnerId = "owner1", BookId = 1 }
        };
        var updatedBookPosting = new BookPosting { Id = 1, OwnerId = "owner1", BookId = 2 };

        // Act
        var result = service.Update(1, updatedBookPosting);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_ExistingId_RemovesBookPosting()
    {
        // Arrange
        var testData = new List<BookPosting>
        {
            new BookPosting { Id = 1, OwnerId = "owner1", BookId = 1 }
        };
        var mockDbSet = new Mock<DbSet<BookPosting>>();
        mockDbSet.Setup(m => m.Remove(It.IsAny<BookPosting>()))
                 .Callback<BookPosting>(bookPosting => testData.Remove(bookPosting));
        var mockContext = new Mock<PostgresContext>();
        mockContext.Setup(c => c.BookPostings).Returns(mockDbSet.Object);
        var service = new BookPostingService(mockContext.Object);
    }

    [Fact]
    public void Search_ValidQuery_ReturnsMatchingBookPostings()
    {
        var query = "author 1";

        // Act
        var result = service.Search(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count()); // Should return 2 postings with "Author 1"
    }
}
