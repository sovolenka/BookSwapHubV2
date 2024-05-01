using Moq;
using Data.Models;
using Data;
using Business.Services;
using Microsoft.EntityFrameworkCore;


namespace Tests;

public class BookPostingServiceTests
{
    [Fact]
    public void GetById_ValidId_ReturnsBookPosting()
    {
        // Arrange
        var testData = new List<BookPosting>
        {
            new BookPosting { Id = 1, OwnerId = "owner1", BookId = 1 },
            new BookPosting { Id = 2, OwnerId = "owner2", BookId = 2 },
            new BookPosting { Id = 3, OwnerId = "owner3", BookId = 3 }
        };
        var mockDbSet = new Mock<DbSet<BookPosting>>();
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.Provider).Returns(testData.AsQueryable().Provider);
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.Expression).Returns(testData.AsQueryable().Expression);
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.ElementType).Returns(testData.AsQueryable().ElementType);
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.GetEnumerator()).Returns(testData.AsQueryable().GetEnumerator());

        var mockContext = new Mock<PostgresContext>();
        mockContext.Setup(c => c.BookPostings).Returns(mockDbSet.Object);
        var service = new BookPostingService(mockContext.Object);

        // Act
        var result = service.GetById(2);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Id);
    }

    [Fact]
    public void GetByOwnerId_ValidOwnerId_ReturnsBookPostings()
    {
        // Arrange
        var ownerId = "owner1";
        var testData = new List<BookPosting>
        {
            new BookPosting { Id = 1, OwnerId = "owner1", BookId = 1 },
            new BookPosting { Id = 2, OwnerId = "owner2", BookId = 2 },
            new BookPosting { Id = 3, OwnerId = "owner1", BookId = 3 }
        };
        var mockDbSet = new Mock<DbSet<BookPosting>>();
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.Provider).Returns(testData.AsQueryable().Provider);
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.Expression).Returns(testData.AsQueryable().Expression);
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.ElementType).Returns(testData.AsQueryable().ElementType);
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.GetEnumerator()).Returns(testData.AsQueryable().GetEnumerator());

        var mockContext = new Mock<PostgresContext>();
        mockContext.Setup(c => c.BookPostings).Returns(mockDbSet.Object);
        var service = new BookPostingService(mockContext.Object);

        // Act
        var result = service.GetByOwnerId(ownerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count()); // Should return 2 postings for owner1
    }

    [Fact]
    public void Add_ValidBookPosting_ReturnsAddedBookPosting()
    {
        // Arrange
        var testData = new List<BookPosting>();
        var mockDbSet = new Mock<DbSet<BookPosting>>();
        mockDbSet.Setup(m => m.Add(It.IsAny<BookPosting>()))
                 .Callback<BookPosting>(bookPosting => testData.Add(bookPosting));
        var mockContext = new Mock<PostgresContext>();
        mockContext.Setup(c => c.BookPostings).Returns(mockDbSet.Object);
        var service = new BookPostingService(mockContext.Object);
        var newBookPosting = new BookPosting { Id = 4, OwnerId = "owner4", BookId = 4 };

        // Act
        var result = service.Add(newBookPosting);

        // Assert
        Assert.NotNull(result);
        Assert.Single(testData);
        Assert.Equal(newBookPosting.Id, testData[0].Id);
    }

    [Fact]
    public void Update_ValidBookPosting_ReturnsUpdatedBookPosting()
    {
        // Arrange
        var testData = new List<BookPosting>
        {
            new BookPosting { Id = 1, OwnerId = "owner1", BookId = 1 }
        };
        var mockDbSet = new Mock<DbSet<BookPosting>>();
        mockDbSet.Setup(m => m.Update(It.IsAny<BookPosting>()));
        var mockContext = new Mock<PostgresContext>();
        mockContext.Setup(c => c.BookPostings).Returns(mockDbSet.Object);
        var service = new BookPostingService(mockContext.Object);
        var updatedBookPosting = new BookPosting { Id = 1, OwnerId = "owner1", BookId = 2 };

        // Act
        var result = service.Update(updatedBookPosting);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.BookId); // Should be updated to BookId 2
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

        // Act
        service.Delete(1);

        // Assert
        Assert.Empty(testData); // Should be removed
    }

    [Fact]
    public void Search_ValidQuery_ReturnsMatchingBookPostings()
    {
        // Arrange
        var testData = new List<BookPosting>
        {
            new BookPosting { Id = 1, OwnerId = "owner1", BookId = 1, Book = new Book { Name = "Book 1", Author = "Author 1" } },
            new BookPosting { Id = 2, OwnerId = "owner2", BookId = 2, Book = new Book { Name = "Book 2", Author = "Author 2" } },
            new BookPosting { Id = 3, OwnerId = "owner3", BookId = 3, Book = new Book { Name = "Book 3", Author = "Author 1" } }
        };
        var mockDbSet = new Mock<DbSet<BookPosting>>();
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.Provider).Returns(testData.AsQueryable().Provider);
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.Expression).Returns(testData.AsQueryable().Expression);
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.ElementType).Returns(testData.AsQueryable().ElementType);
        mockDbSet.As<IQueryable<BookPosting>>().Setup(m => m.GetEnumerator()).Returns(testData.AsQueryable().GetEnumerator());
        var mockContext = new Mock<PostgresContext>();
        mockContext.Setup(c => c.BookPostings).Returns(mockDbSet.Object);
        var service = new BookPostingService(mockContext.Object);
        var query = "author 1";

        // Act
        var result = service.Search(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count()); // Should return 2 postings with "Author 1"
    }
}
